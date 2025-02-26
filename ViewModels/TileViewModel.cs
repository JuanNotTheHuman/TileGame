using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TileViewModel : ViewModelBase
    {
        public Tile Tile { get; }
        public TileViewModel(Tile tile)
        {
            Tile = tile;
        }
        public double Health
        {
            get => Tile.Health;
            set
            {
                if (Tile.Health != value)
                {
                    Tile.Health = value;
                    OnPropertyChanged(nameof(Health));
                }
            }
        }
        public TileType Type
        {
            get => Tile.Type;
            set
            {
                if (Tile.Type != value)
                {
                    Tile.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }

        public TileCategory Category
        {
            get => Tile.Category;
        }
        public double X
        {
            get => Tile.X;
            set
            {
                if (Tile.X != value)
                {
                    Tile.X = value;
                    OnPropertyChanged(nameof(X));
                }
            }
        }
        public double Y
        {
            get => Tile.Y;
            set
            {
                if (Tile.Y != value)
                {
                    Tile.Y = value;
                    OnPropertyChanged(nameof(Y));
                }
            }
        }
        public int Size
        {
            get => Tile.Size;
        }
        public Tile ToTile()
        {
            return new Tile(Type, Health, X, Y);
        }
    }
}
