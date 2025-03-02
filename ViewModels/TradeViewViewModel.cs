using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using TileGame.Enums;
using TileGame.Helpers;
using TileGame.Models;
using TileGame.Services;
namespace TileGame.ViewModels
{
    public class TradeViewViewModel : ObservableObject
    {
        [ObservableProperty]
        private PlayerViewModel _player;
        private ObservableCollection<TradeViewModel> _trades;
        public PlayerViewModel PlayerViewModel
        {
            get => _player;
            set
            {
                Debug.WriteLine("Setting...");
                SetProperty(ref _player, value);
            }
        }
        public ObservableCollection<TradeViewModel> Trades
        {
            get => _trades;
            set => SetProperty(ref _trades, value);
        }
        public ICommand TradeCommand { get; }
        public TradeViewViewModel(Player player)
        {
            PlayerViewModel = new PlayerViewModel(player);
            Trades = new ObservableCollection<TradeViewModel>();
            TradeCommand = new RelayCommand<TradeViewModel>(Trade);
            Debug.WriteLine("Generating trades...");
            GenerateTrades();
        }
        public TradeViewViewModel(PlayerViewModel player)
        {
            PlayerViewModel = player;
            Trades = new ObservableCollection<TradeViewModel>();
            TradeCommand = new RelayCommand<TradeViewModel>(Trade);
            GenerateTrades();
        }
        public TradeViewViewModel()
        {
            PlayerViewModel = new PlayerViewModel(new Player());
            Trades = new ObservableCollection<TradeViewModel>();
            TradeCommand = new RelayCommand<TradeViewModel>(Trade);
            GenerateTrades();
        }
        public void Trade(TradeViewModel trade)
        {
            var tradeOutItem = PlayerViewModel.Inventory.Items[trade.TradeOut.Key];
            var tradeInItem = PlayerViewModel.Inventory.Items[trade.TradeIn.Key];

            tradeOutItem.Count -= trade.TradeOut.Value;
            tradeInItem.Count += trade.TradeIn.Value;
            PlayerViewModel.Inventory.Items.TryUpdate(trade.TradeOut.Key, tradeOutItem);
            PlayerViewModel.Inventory.Items.TryUpdate(trade.TradeIn.Key, tradeInItem);
            Debug.WriteLine($"Trade: {trade.TradeOut.Key} {trade.TradeOut.Value} for {trade.TradeIn.Key} {trade.TradeIn.Value}");
        }

        private void GenerateTrades()
        {
            Trades = new ObservableCollection<TradeViewModel>();
            var random = new Random();
            for (int i = 0; i < 5; i++)
            {
                var tradeInKey = (ItemType)random.Next(Enum.GetValues(typeof(ItemType)).Length);
                var tradeIn = new KeyValuePair<ItemType, int>(tradeInKey,random.Next(1,6));
                var x = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().Where(r => r != tradeInKey);
                var tradeOutKey = x.ElementAt(random.Next(x.Count()));
                var tradeOut = new KeyValuePair<ItemType, int>(tradeOutKey, random.Next(1, 6));
                Trades.Add(new TradeViewModel(new Trade(tradeOut, tradeIn)));
            }
        }
    }
}
