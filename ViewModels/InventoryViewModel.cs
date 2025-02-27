using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TileGame.Helpers;
using TileGame.Enums;
using TileGame.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TileGame.ViewModels
{
    public class InventoryViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableDictionary<ItemType,ItemViewModel> _items;
        public ObservableDictionary<ItemType, ItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }
        public InventoryViewModel(Inventory inventory)
        {
            Items = new ObservableDictionary<ItemType, ItemViewModel>(
                inventory.Items.ToDictionary(r => r.Key, r => new ItemViewModel(r.Value))
            );
        }
        public Inventory ToInventory()
        {
            return new Inventory(Items.ToDictionary(r => r.Key, r => r.Value.ToItem()));
        }
    }
}
