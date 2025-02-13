using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
        private Timer Timer { get; set; }
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
            Timer = new Timer();
            Timer.Interval = Config.Tick.Interval;
            Timer.Elapsed += async(source, ev) => await Task.Run(() => Tick(source, ev));
            TileClick = new RelayCommand<TileViewModel>(BoardTileClicked);
            PlayerViewModel = new PlayerViewModel(new Player());
        }

        public static async Task<GameViewModel> CreateAsync()
        {
            var config = new Config();
            var gameViewModel = new GameViewModel(config);
            var board = await Board.CreateAsync(1920, 1080, config);
            gameViewModel.BoardViewModel = new BoardViewModel(board);
            gameViewModel.Timer.Enabled = true;
            return gameViewModel;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async Task Tick(object source, ElapsedEventArgs e)
        {
            try
            {
                int transforms = 0;
                List<TileViewModel> tiles = new List<TileViewModel>();
                lock (BoardViewModel.Tiles)
                {
                    var tilesSnapshot = BoardViewModel.Tiles.ToList();
                    if(tilesSnapshot.Count(r => tilesSnapshot.Count(r1 => r != r1 && Tile.Collides(r.Tile, r1.Tile)) >= 3) > 0)
                    {
                        Debug.WriteLine($"Tiles with at least 3 overlapping: {tilesSnapshot.Count(r => tilesSnapshot.Count(r1 => r != r1 && Tile.Collides(r.Tile, r1.Tile)) >= 3)}");
                    }
                }
                while (transforms < Config.Tick.MaxTransform)
                {
                    foreach (var config in Config.Tiles.ForegroundGeneration)
                    {
                        if (transforms >= Config.Tick.MaxTransform) break;
                        int tileCount = 0;
                        lock (BoardViewModel.Tiles)
                        {
                            var tilesSnapshot = new ObservableCollection<TileViewModel>(BoardViewModel.Tiles);
                            tileCount = tilesSnapshot.Count(r => r.Type == config.Key);
                        }
                        if (config.Value.Max > tileCount)
                        {
                            if (config.Value.SpawnChance < Random.NextDouble()) continue;
                            TileViewModel bgTile = BoardViewModel.Tiles.OrderBy(r => Random.Next()).First();
                            if (config.Value.SpawnableOn.Contains(bgTile.Type) && BoardViewModel.Tiles.Count(r => Tile.Collides(r.Tile, bgTile.Tile)) < 2)
                            {
                                tiles.Add(new TileViewModel(new Tile(config.Key, config.Value.Health, bgTile.X, bgTile.Y)));
                                transforms++;
                            }
                        }
                    }
                    foreach (var config in Config.Tiles.BaseGeneration)
                    {
                        if (config.Value is ForegroundTileConfig fconfig)
                        {
                            int cnt;
                            lock (BoardViewModel.Tiles)
                            {
                                var t = new ObservableCollection<TileViewModel>(BoardViewModel.Tiles);
                                cnt = t.Count(r => r.Type == config.Key);
                            }
                            if (fconfig.Max < cnt)
                            {
                                if (config.Value.SpawnChance > Random.NextDouble())
                                {
                                    TileViewModel tl;
                                    lock (BoardViewModel.Tiles)
                                    {
                                        var t = new ObservableCollection<TileViewModel>(BoardViewModel.Tiles);
                                        tl = t.OrderBy(r => Random.Next()).First();
                                    }
                                    if (fconfig.SpawnableOn.Contains(tl.Tile.Type))
                                    {
                                        int c;
                                        lock (BoardViewModel.Tiles)
                                        {
                                            var t = new ObservableCollection<TileViewModel>(BoardViewModel.Tiles);
                                            c = t.Count(r => Tile.Collides(r.Tile, tl.Tile));
                                        }
                                        if (c < 2)
                                        {
                                            if (tl.Type != config.Key)
                                            {
                                                await UpdateTileType(tl, config.Key);
                                                transforms++;
                                            }
                                        }
                                    }
                                }
                            }
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
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var board = BoardViewModel.Tiles;
                var distinctTiles = tiles.Distinct(new TileViewModelPositionEqualityComparer()).ToList();
                foreach (var tile in distinctTiles)
                {
                    board.Add(tile);
                    Debug.WriteLine($"Finalized: Added a {tile.Type} at ({tile.X}, {tile.Y})");
                }
                BoardViewModel.OnPropertyChanged(nameof(BoardViewModel.Tiles));
                Debug.WriteLine($"Total Tiles After Update: {board.Count}");
            });
        }

        private async Task UpdateTileType(TileViewModel tile, TileType newType)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Debug.WriteLine($"Updated {tile.Type} to {newType}");
                tile.Type = newType;
                tile.Health = Config.Tiles.BaseGeneration[newType].Health;
                tile.OnPropertyChanged(nameof(tile.Type));
            });
        }
    }
}
