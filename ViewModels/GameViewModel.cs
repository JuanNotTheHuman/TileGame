using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TileGame.Enums;
using TileGame.Helpers;
using TileGame.Models;
namespace TileGame.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private BoardViewModel _board;
        private PlayerViewModel _player;
        public PlayerViewModel PlayerViewModel
        {
            get => _player;
            set
            {
                if(_player != value)
                {
                    _player = value;
                    OnPropertyChanged(nameof(PlayerViewModel));
                }
            }
        }
        private DispatcherTimer Timer { get; set; }
        private Random Random { get; } = new Random();
        public ICommand TileClick { get; }
        public Config Config { get; }
        public BoardViewModel BoardViewModel
        {
            get { return _board; }
            set
            {
                _board = value;
                OnPropertyChanged(nameof(BoardViewModel));
            }
        }
        private void BoardTileClicked(TileViewModel tile)
        {
            tile.Health--;
            if (tile.Health <= 0)
            {

                BoardViewModel.Tiles.Remove(tile);
                if (tile.Type == TileType.GrassC)
                {
                    BoardViewModel.Tiles.Add(new TileViewModel(new Tile(TileType.GrassB, Config.Tiles.BaseGeneration[TileType.GrassB].Health, tile.X, tile.Y)));

                }
                if (Config.Tiles.DeathDrops.ContainsKey(tile.Type))
                {
                    foreach (TileDrop drop in Config.Tiles.DeathDrops[tile.Type])
                    {
                        PlayerViewModel.Inventory.Items[drop.Type].Count += drop.Count;
                    }
                }
                return;
            }
            if (Config.Tiles.ClickDrops.ContainsKey(tile.Type))
            {
                foreach (TileDrop drop in Config.Tiles.ClickDrops[tile.Type])
                {
                    PlayerViewModel.Inventory.Items[drop.Type].Count += drop.Count;
                }
            }
        }
        private GameViewModel(Config config)
        {
            Config = config;
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(async (sender, e) => { await Tick(sender, e); });
            Timer.Interval = new TimeSpan(Config.Tick.Interval);
            TileClick = new RelayCommand<TileViewModel>(BoardTileClicked);
            PlayerViewModel = new PlayerViewModel(new Player());
        }

        public static async Task<GameViewModel> CreateAsync()
        {
            var config = new Config();
            var gameViewModel = new GameViewModel(config);
            var board = await Board.CreateAsync(1920, 1080, config);
            gameViewModel.BoardViewModel = new BoardViewModel(board);
            gameViewModel.Timer.Start();
            return gameViewModel;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async Task Tick(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Tick");
                int transforms = 0;
                List<TileViewModel> tiles = new List<TileViewModel>();
                var tilesSnapshot = new ObservableCollection<TileViewModel>(BoardViewModel.Tiles);
                if(tilesSnapshot.Count(r => tilesSnapshot.Count(r1 => r != r1 && Tile.Collides(r.Tile, r1.Tile)) >= 3) > 0)
                {
                    Debug.WriteLine($"Tiles with at least 3 overlapping: {tilesSnapshot.Count(r => tilesSnapshot.Count(r1 => r != r1 && Tile.Collides(r.Tile, r1.Tile)) >= 3)}");
                }
                while (transforms < Config.Tick.MaxTransform)
                {
                    foreach (var config in Config.Tiles.ForegroundGeneration)
                    {
                        if (transforms >= Config.Tick.MaxTransform)
                        {
                            break;
                        }
                        int tileCount = 0;
                        tileCount = tilesSnapshot.Count(r => r.Type == config.Key);
                        if (config.Value.Max < tileCount){
                            continue;
                        }
                        if (config.Value.SpawnChance < Random.NextDouble())
                        {
                            continue;
                        }
                        TileViewModel bgTile = tilesSnapshot[Random.Next(BoardViewModel.Tiles.Count())];
                        if (!config.Value.SpawnableOn.Contains(bgTile.Type))
                        {
                            continue;
                        }
                        if ((tilesSnapshot.Count(r => Tile.Collides(r.Tile, bgTile.Tile)) > 2)||tiles.Any(r=>Tile.Collides(r.Tile,bgTile.Tile)))
                        {
                            continue;
                        }
                        tiles.Add(new TileViewModel(new Tile(config.Key, config.Value.Health, bgTile.X, bgTile.Y)));
                        transforms++;
                    }
                    foreach (var config in Config.Tiles.BaseGeneration)
                    {
                        if (config.Value is ForegroundTileConfig fconfig)
                        {
                            int cnt;
                            cnt = tilesSnapshot.Count(r => r.Type == config.Key);
                            if (fconfig.Max > cnt) continue;
                            if (config.Value.SpawnChance < Random.NextDouble()) continue;
                            var tl = tilesSnapshot[Random.Next(tilesSnapshot.Count())];
                            if (!fconfig.SpawnableOn.Contains(tl.Tile.Type)) break;
                            int c = tilesSnapshot.Count(r => Tile.Collides(r.Tile, tl.Tile));
                            if (c > 2) continue;
                            if (tl.Type == config.Key) continue;
                            await UpdateTileType(tl, config.Key);
                            transforms++;
                        }
                    }
                }
                if (tiles.Count > 0) await AddTilesToDisplay(tiles);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        private async Task AddTilesToDisplay(List<TileViewModel> tiles)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                var board = BoardViewModel.Tiles;
                var distinctTiles = tiles.Where(r =>!tiles.Any(r1 => r != r1 && Tile.Collides(r.Tile, r1.Tile)));
                foreach (var tile in distinctTiles)
                {
                    board.Add(tile);
                    await Task.Delay(5);
                    Debug.WriteLine($"Finalized: Added a {tile.Type} at ({tile.X}, {tile.Y})");
                }
                BoardViewModel.OnPropertyChanged(nameof(BoardViewModel.Tiles));
                Debug.WriteLine($"Total Tiles After Update: {board.Count}");
            });
        }

        private async Task UpdateTileType(TileViewModel tile, TileType newType)
        {
            await Application.Current.Dispatcher.InvokeAsync(async () =>
            {
                await Task.Delay(10);
                tile.Type = newType;
                tile.Health = Config.Tiles.BaseGeneration[newType].Health;
                tile.OnPropertyChanged(nameof(tile.Type));
            });
        }
    }
}
