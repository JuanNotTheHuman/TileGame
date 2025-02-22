using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileGame.Enums
{
    public enum TileType
    {
        GrassA, 
        GrassB, 
        GrassC,
        Bush,
        Mushrooms,
        TreeA,
        TreeB,
    }
    public enum TileCategory
    {
        Grass,
        Bush,
        Tree,
        Other,
    }
    public enum ItemType
    {
        Coin,
        Wood,
        Mushroom,
        Flower,
        Honey
    }
    public enum SpawnBehavior
    {
        Random,
        Spread,
    }
}
