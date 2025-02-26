using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Enums;

namespace TileGame.Models
{
    public class Trade
    {
        public KeyValuePair<ItemType,int> TradeOut { get; }
        public KeyValuePair<ItemType, int> TradeIn { get; }
        public Trade()
        {
        }
        public Trade(KeyValuePair<ItemType, int> tradeOut, KeyValuePair<ItemType, int> tradeIn)
        {
            TradeOut = tradeOut;
            TradeIn = tradeIn;
        }
    }
}
