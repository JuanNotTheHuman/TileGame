﻿using System;
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
        public bool IsLoading { get; private set; }
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
        public static async Task<GameViewModel> CreateAsync()
        {
            var config = new Config();
            var gameViewModel = new GameViewModel(config);
            var board = await Board.CreateAsync(1920, 1080, config);
            gameViewModel.BoardViewModel = new BoardViewModel(board,config);
            return gameViewModel;
        }
        public static async Task<GameViewModel> CreateAsync(Config config)
        {

            var gameViewModel = new GameViewModel(config);
            var board = await Board.CreateAsync(1920, 1080, config);
            gameViewModel.BoardViewModel = new BoardViewModel(board, config);
            return gameViewModel;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
