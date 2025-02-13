using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class BoardViewModel : INotifyPropertyChanged
    {
        public Board Board { get; private set; }
        private ObservableCollection <TileViewModel> _tiles;
        public BoardViewModel(Board board)
        {
            Board = board;
            _tiles = new ObservableCollection<TileViewModel>(
                board.TileGrid.Select(tile => new TileViewModel(tile))
            );
        }
        public ObservableCollection<TileViewModel> Tiles
        {   
            get => _tiles;
            set
            {
                if(_tiles != value)
                {
                    _tiles = value;
                    OnPropertyChanged(nameof(Tiles));
                }
            }
        }
        public int Score
        {
            get => Score;
            set
            {
                if(Score != value)
                {
                    Score = value;
                    OnPropertyChanged(nameof(Score));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]  string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
