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
    public class GameSaveViewModel : ViewModelBase
    {
        private GameSave _gameSave;
        private PlayerViewModel _playerViewModel;
        private BoardViewModel _boardViewModel;
        public string Name
        {
            get { return _gameSave.Name; }
            set
            {
                if (_gameSave.Name != value)
                {
                    _gameSave.Name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public PlayerViewModel Player
        {
            get { return _playerViewModel; }
            set
            {
                if (_playerViewModel != value)
                {
                    _playerViewModel = value;
                    OnPropertyChanged(nameof(Player));
                }
            }
        }
        public BoardViewModel Board
        {
            get { return _boardViewModel; }
            set
            {
                if (_boardViewModel != value)
                {
                    _boardViewModel = value;
                    OnPropertyChanged(nameof(Board));
                }
            }
        }
        public GameSaveViewModel(GameSave gameSave)
        {
            _gameSave = gameSave;
            _playerViewModel = new PlayerViewModel(gameSave.Player);
            _boardViewModel = new BoardViewModel(gameSave.Board);
        }
    }
}
