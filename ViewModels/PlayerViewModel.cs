using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class PlayerViewModel : ObservableObject
    {
        [ObservableProperty]
        private InventoryViewModel _inventory;
        public InventoryViewModel Inventory
        {
            get => _inventory;
            set => SetProperty(ref _inventory, value);
        }
        public PlayerViewModel(Player player)
        {
            Inventory = new InventoryViewModel(player.Inventory);
        }
        public Player ToPlayer()
        {
            return new Player(Inventory.ToInventory());
        }
    }
}
