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

namespace TileGame.ViewModels
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        private Inventory Inventory { get; }
        private ObservableDictionary<ItemType,ItemViewModel> _items;
        public ObservableDictionary<ItemType, ItemViewModel> Items
        {
            get => _items;
            set
            {
                if(_items != value)
                {
                    _items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        public InventoryViewModel(Inventory inventory)
        {
            Inventory = inventory;
            _items = new ObservableDictionary<ItemType, ItemViewModel>(
                inventory.Items.ToDictionary(r => r.Key, r => new ItemViewModel(r.Value))
            );
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
