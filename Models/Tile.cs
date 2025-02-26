using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TileGame.Enums;

namespace TileGame.Models
{
    [Serializable]
    public class Tile
    {
        public TileType Type { get; set; }
        public TileCategory Category => GetTileCategory(Type);
        public double Health { get; set; }
        public static int Size { get; } = 24;
        public double X { get; set; }
        public double Y { get; set; }
        public static TileCategory GetTileCategory(TileType type)
        {
            switch (type)
            {
                case TileType.GrassA:
                case TileType.GrassB:
                case TileType.GrassC:
                    return TileCategory.Grass;
                case TileType.Bush:
                    return TileCategory.Bush;
                case TileType.Mushrooms:
                    return TileCategory.Other;
                case TileType.TreeA:
                case TileType.TreeB:
                    return TileCategory.Tree;
                default:
                    throw new NotImplementedException();
            }
        }
        public Tile()
        {
            Type = TileType.GrassA;
            Health = double.PositiveInfinity;
            X = 0;
            Y = 0;
        }
        public Tile(TileType type, double health, double x, double y)
        {
            if (health <= 0) throw new ArgumentException("Tile health cannot be lower or equal to 0.");
            Type = type;
            Health = health;
            X = x;
            Y = y;
        }
        public bool Collides(Tile tile)
        {
            return X==tile.X && Y==tile.Y;
        }
        public static bool Collides(Tile a, Tile b)
        {
            return ((a.X==b.X) && (a.Y==b.Y));
        }
        public override string ToString()
        {
            return $"Type:{Type}, Health:{Health}, X:{X}, Y:{Y}";
        }
    }
}
