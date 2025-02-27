using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TileViewModel : ObservableObject
    {
        [ObservableProperty]
        private double health;
        [ObservableProperty]
        private TileType type;
        [ObservableProperty]
        private double x;
        [ObservableProperty]
        private double y;
        public TileViewModel(Tile tile)
        {
            Health = tile.Health;
            Type = tile.Type;
            X = tile.X;
            Y = tile.Y;
        }
        public double Health
        {
            get => health;
            set=>SetProperty(ref health, value);
        }
        public TileType Type
        {
            get => type;
            set => SetProperty(ref type, value);
        }

        public TileCategory Category
        {
            get => Tile.GetTileCategory(Type);
        }
        public double X
        {
            get => x;
            set => SetProperty(ref x, value);
        }
        public double Y
        {
            get => y;
            set => SetProperty(ref y, value);
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
