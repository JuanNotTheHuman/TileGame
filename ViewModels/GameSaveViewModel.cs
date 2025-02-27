using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TileGame.Models;
using TileGame.Services;

namespace TileGame.ViewModels
{
    public class GameSaveViewModel : ObservableObject
    {
        [ObservableProperty]
        private PlayerViewModel _playerViewModel;
        [ObservableProperty]
        private BoardViewModel _boardViewModel;
        [ObservableProperty]
        private string _name;
        public string Name
        {
            get => _name;
            set=>SetProperty(ref _name, value);
        }
        public PlayerViewModel Player
        {
            get=> _playerViewModel;
            set=>SetProperty(ref _playerViewModel, value);
        }
        public BoardViewModel Board
        {
            get => _boardViewModel;
            set => SetProperty(ref _boardViewModel, value);
        }
        public GameSaveViewModel(GameSave gameSave)
        {
            Name = gameSave.Name;
            Player = new PlayerViewModel(gameSave.Player);
            Board = new BoardViewModel(gameSave.Board);
        }
    }
}
