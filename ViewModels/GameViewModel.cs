using CommunityToolkit.Mvvm.ComponentModel;
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
using TileGame.Services;
namespace TileGame.ViewModels
{
    public class GameViewModel : ObservableObject
    {
        [ObservableProperty]
        private BoardViewModel _board;
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        [ObservableProperty]
        private PlayerViewModel _player;
        public PlayerViewModel PlayerViewModel
        {
            get=> _player;
            set=> SetProperty(ref _player, value);
        }
        public ICommand TileClick { get; }
        public Config Config { get; }
        public BoardViewModel BoardViewModel
        {
            get => _board;
            set => SetProperty(ref _board, value);
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
                if (Config.Tiles.ForegroundGeneration.ContainsKey(tile.Type))
                {
                    foreach (KeyValuePair<ItemType, int> drop in Config.Tiles.ForegroundGeneration[tile.Type].DeathDrops)
                    {
                        PlayerViewModel.Inventory.Items[drop.Key].Count += drop.Value;
                    }
                }
                else if (Config.Tiles.BaseGeneration.ContainsKey(tile.Type))
                {
                    foreach (KeyValuePair<ItemType, int> drop in Config.Tiles.BaseGeneration[tile.Type].DeathDrops)
                    {
                        PlayerViewModel.Inventory.Items[drop.Key].Count += drop.Value;
                    }
                }
                return;
            }
            if (Config.Tiles.ForegroundGeneration.ContainsKey(tile.Type))
            {
                foreach (KeyValuePair<ItemType, int> drop in Config.Tiles.ForegroundGeneration[tile.Type].ClickDrops)
                {
                    PlayerViewModel.Inventory.Items[drop.Key].Count += drop.Value;
                }
            }
            else if (Config.Tiles.BaseGeneration.ContainsKey(tile.Type))
            {
                foreach (KeyValuePair<ItemType, int> drop in Config.Tiles.BaseGeneration[tile.Type].ClickDrops)
                {
                    PlayerViewModel.Inventory.Items[drop.Key].Count += drop.Value;
                }
            }
        }
        private GameViewModel(Config config)
        {
            Config = config;
            TileClick = new RelayCommand<TileViewModel>(BoardTileClicked);
            PlayerViewModel = new PlayerViewModel(new Player());
        }
        public GameViewModel()
        {
            Config = new Config();
            TileClick = new RelayCommand<TileViewModel>(BoardTileClicked);
            PlayerViewModel = new PlayerViewModel(new Player());
        }
        public static async Task<GameViewModel> CreateAsync(GameSave save)
        {
            GameViewModel viewModel = new GameViewModel(save.Board.Config);
            Board board = await Board.CreateAsync(1920, 1080, save.Board.Config);
            viewModel.PlayerViewModel = new PlayerViewModel(save.Player);
            viewModel.BoardViewModel = new BoardViewModel(board);
            viewModel.InitializeTick();
            return viewModel;
        }
        public static async Task<GameViewModel> CreateAsync()
        {
            var config = new Config();
            var gameViewModel = new GameViewModel(config);
            var board = await Board.CreateAsync(1920, 1080, config);
            gameViewModel.BoardViewModel = new BoardViewModel(board);
            gameViewModel.InitializeTick();
            return gameViewModel;
        }
        public static async Task<GameViewModel> CreateAsync(Config config)
        {

            var gameViewModel = new GameViewModel(config);
            var board = await Board.CreateAsync(1920, 1080, config);
            gameViewModel.BoardViewModel = new BoardViewModel(board);
            gameViewModel.InitializeTick();
            return gameViewModel;
        }
        public void InitializeTick()
        {
            _timer.Interval = new TimeSpan(Config.Tick.Interval);
            _timer.Tick += Tick;
            _timer.Start();
        }
        private void Tick(object sender,EventArgs e)
        {
            var playerData = PlayerViewModel.ToPlayer();
            var boardData = BoardViewModel.ToBoard();
            GameSaveService.Save(GameSaveService.Current.Name, new GameSave(GameSaveService.Current.Name, playerData, boardData));
        }
    }
}
