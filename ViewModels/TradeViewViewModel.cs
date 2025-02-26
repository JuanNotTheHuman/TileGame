using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TradeViewViewModel : ViewModelBase
    {
        private PlayerViewModel _player;
        public ObservableCollection<TradeViewModel> Trades { get; set; } = new ObservableCollection<TradeViewModel>();
        public PlayerViewModel Player
        {
            get => _player;
            set
            {
                if (_player != value)
                {
                    _player = value;
                    Debug.WriteLine("setting...");
                    OnPropertyChanged(nameof(Player));
                }
            }
        }
        public RelayCommand<TradeViewModel> TradeCommand { get; }
        public TradeViewViewModel()
        {
            _player = new PlayerViewModel(new Player());
            TradeCommand = new RelayCommand<TradeViewModel>(Trade,CanTrade);
            GenerateTrades();
        }
        public TradeViewViewModel(PlayerViewModel player)
        {
            _player = player;
            TradeCommand = new RelayCommand<TradeViewModel>(Trade,CanTrade);
            GenerateTrades();
        }
        private bool CanTrade(TradeViewModel tvm)
        {
            return true; // for testing
            //return tvm != null
            //    && Player.Inventory.Items.ContainsKey(tvm.TradeOut.Key)
            //    && Player.Inventory.Items[tvm.TradeOut.Key].Count >= tvm.TradeOut.Value;
        }
        private void Trade(TradeViewModel trade)
        {
            if (trade == null) return;
            if (Player.Inventory.Items.ContainsKey(trade.TradeOut.Key))
                Player.Inventory.Items[trade.TradeOut.Key].Count = Player.Inventory.Items[trade.TradeOut.Key].Count - trade.TradeOut.Value;
            if (Player.Inventory.Items.ContainsKey(trade.TradeIn.Key))
                Player.Inventory.Items[trade.TradeIn.Key].Count = Player.Inventory.Items[trade.TradeIn.Key].Count - trade.TradeIn.Value;
            Debug.WriteLine($"{trade.TradeOut.Key}: {Player.Inventory.Items[trade.TradeOut.Key].Count}");
            Debug.WriteLine($"{trade.TradeIn.Key}: {Player.Inventory.Items[trade.TradeIn.Key].Count}");
            TradeCommand.RaiseCanExecuteChanged();
        }
        private void GenerateTrades()
        {
            var random = new Random();
            for (int i = 0; i < 7; i++)
            {
                var tradeIn = new KeyValuePair<ItemType, int>(
                    (ItemType)random.Next(Enum.GetNames(typeof(ItemType)).Length),
                    random.Next(1, 5)
                );

                var validTradeOutItems = ((int[])Enum.GetValues(typeof(ItemType))).Where(r => r != (int)tradeIn.Key).ToList();
                if (!validTradeOutItems.Any()) continue;

                var tradeOut = new KeyValuePair<ItemType, int>(
                    (ItemType)validTradeOutItems[random.Next(validTradeOutItems.Count)],
                    random.Next(1, 5)
                );

                var trade = new TradeViewModel(new Trade(tradeOut, tradeIn));
                Trades.Add(trade);
            }
        }
    }
}
