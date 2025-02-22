using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using TileGame.Enums;
using TileGame.Models;
using System.Linq;

public class Board
{
    public Config Config { get; private set; }
    public ObservableCollection<Tile> TileGrid { get; private set; }

    private Board(Config config)
    {
        Config = config;
        TileGrid = new ObservableCollection<Tile>();
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

    private void Initialize(double screenWidth, double screenHeight)
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
}
