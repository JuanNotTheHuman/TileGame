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
            Timer.Tick += new EventHandler((sender, e) => { Tick(sender, e); });
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
            gameViewModel.BoardViewModel.EnableQueue();
            return gameViewModel;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void Tick(object sender, EventArgs e)
        {
            try
            {
                List<TileViewModel> tiles = new List<TileViewModel>();
                var tilesSnapshot = new ObservableCollection<TileViewModel>(BoardViewModel.Tiles);
                for (int i = 0; i < Config.Tick.MaxTransform; i++)
                {
                    tiles.Clear();
                    foreach (var pair in Config.Tiles.ForegroundGeneration)
                    {
                        TileType type = pair.Key;
                        ForegroundTileConfig config = pair.Value;
                        if (config.SpawnBehavior == SpawnBehavior.Spread)
                        {
                            var tile = tilesSnapshot[Random.Next(tilesSnapshot.Count)];
                            if (!BoardViewModel.CanSpread(BoardViewModel.TilesAt(tile.X, tile.Y).First(r => r.Type == tile.Type), Config)) continue;

                        }
                        else if(config.SpawnBehavior == SpawnBehavior.Random)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
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
