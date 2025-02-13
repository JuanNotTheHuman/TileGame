using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.ViewModels;

namespace TileGame.Helpers
{
    public class TileViewModelPositionEqualityComparer : IEqualityComparer<TileViewModel>
    {
        public bool Equals(TileViewModel a, TileViewModel b)
        {
            if (a == null || b == null) return false;

            return a.X == b.X && a.Y == b.Y;
        }

        public int GetHashCode(TileViewModel obj)
        {
            if (obj == null) return 0;

            return obj.X.GetHashCode() ^ obj.Y.GetHashCode();
        }
    }
}
