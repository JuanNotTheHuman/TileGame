using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using TileGame.Enums;
using TileGame.Models;
using System.Linq;
using System.Text.Json.Serialization;
namespace TileGame.Models
{
    [Serializable]
    public class Board
    {
        public Config Config { get; set; }
        [JsonInclude]
        public Collection<Tile> TileGrid { get; set; }
        public Board(Config config)
        {
            Config = config;
            TileGrid = new Collection<Tile>();
        }
        [JsonConstructor]
        public Board(Config config, Collection<Tile> tileGrid)
        {
            Config = config;
            TileGrid = tileGrid ?? new ObservableCollection<Tile>();
        }
        public static Task<Board> CreateAsync(double screenWidth, double screenHeight, Config config)
        {
            var board = new Board(config);
            return Task.Run(() =>
            {
                board.Initialize(screenWidth, screenHeight);
                return board;
            });
        }
        public static Task<Board> CreateAsync(double screenWidth, double screenHeight, Config config, Collection<Tile> tilegrid)
        {
            var board = new Board(config, tilegrid);
            return Task.Run(() =>
            {
                board.Initialize(screenWidth, screenHeight);
                return board;
            });
        }
        public void Initialize(double screenWidth, double screenHeight)
        {
            int columns = (int)(screenWidth / Tile.Size);
            int rows = (int)(screenHeight / Tile.Size);
            var totalTiles = columns * rows;
            List<WeightedRandomItem<TileType>> Tiles = new List<WeightedRandomItem<TileType>>
        {
            new WeightedRandomItem<TileType>(TileType.GrassA, Config.Tiles.BaseGeneration[TileType.GrassA].SpawnChance),
            new WeightedRandomItem<TileType>(TileType.GrassB, Config.Tiles.BaseGeneration[TileType.GrassB].SpawnChance),
            new WeightedRandomItem<TileType>(TileType.GrassC, Config.Tiles.BaseGeneration[TileType.GrassC].SpawnChance),
        };
            var random = new Random(Config.Seed);
            for (int i = 0; i < totalTiles; i++)
            {
                int row = i / columns;
                int column = i % columns;
                var type = WeightedRandom.GetRandom(Tiles);
                var tile = new Tile(type, Config.Tiles.BaseGeneration[type].Health, Tile.Size * column, Tile.Size * row);
                lock (TileGrid)
                {
                    TileGrid.Add(tile);
                }
            }
            foreach (KeyValuePair<TileType, ForegroundTileConfig> foreground in Config.Tiles.ForegroundGeneration)
            {
                for (int i = 0; i < foreground.Value.Max; i++)
                {
                    Tile bgtile;
                    lock (TileGrid)
                    {
                        bgtile = TileGrid.OrderBy(_ => random.Next()).First();
                    }
                    if (foreground.Value.SpawnableOn.Contains(bgtile.Type) && TileGrid.Count(r => r.Collides(bgtile)) < 2)
                    {
                        lock (TileGrid)
                        {
                            TileGrid.Add(new Tile(foreground.Key, foreground.Value.Health, bgtile.X, bgtile.Y));
                        }
                    }
                }
            }
        }
        public override string ToString()
        {
            return $"{{Config: {Config},TileGrid: {$"{{{string.Join(",\n",TileGrid.Select(r=>r))}}}"}}}";
        }
    }

}


