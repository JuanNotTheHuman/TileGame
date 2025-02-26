using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TradeViewModel : ViewModelBase
    {
        private Trade _trade;
        public KeyValuePair<ItemType, int> TradeOut { get => _trade.TradeOut; }
        public KeyValuePair<ItemType, int> TradeIn { get => _trade.TradeIn; }
        public TradeViewModel(Trade trade)
        {
            _trade = trade;
        }
    }
}
