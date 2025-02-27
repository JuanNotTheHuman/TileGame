using CommunityToolkit.Mvvm.ComponentModel;
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
    [ObservableObject]
    public class TradeViewViewModel : ObservableObject
    {
        public ObservableCollection<TradeViewModel> Trades { get; set; } = new ObservableCollection<TradeViewModel>();
        [ObservableProperty]
        private InventoryViewModel _playerInventory;
        public InventoryViewModel PlayerInventory
        {
            get => _playerInventory;
            set => SetProperty(ref _playerInventory, value);
        }
        public RelayCommand<TradeViewModel> TradeCommand { get; }
        public TradeViewViewModel()
        {
            _playerInventory = new InventoryViewModel(new Inventory());
            TradeCommand = new RelayCommand<TradeViewModel>(Trade,CanTrade);
            GenerateTrades();
        }
        public TradeViewViewModel(PlayerViewModel player)
        {
            _playerInventory = player.Inventory;
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

            var items = PlayerInventory.Items;

            if (items.TryGetValue(trade.TradeOut.Key, out var tradeOutItem))
            {
                tradeOutItem.Count -= trade.TradeOut.Value;
                OnPropertyChanged(nameof(trade.TradeOut.Key));
            }

            if (items.TryGetValue(trade.TradeIn.Key, out var tradeInItem))
            {
                tradeInItem.Count += trade.TradeIn.Value;
            }
            Debug.WriteLine($"{trade.TradeOut.Key}: {tradeOutItem?.Count}");
            Debug.WriteLine($"{trade.TradeIn.Key}: {tradeInItem?.Count}");

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
