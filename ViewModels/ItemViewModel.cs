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
    public class ItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _description;
        [ObservableProperty]
        private int _count;
        public string Description
        {
            get => _description;
            private set => SetProperty(ref _description, value);
        }
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }
        public ItemViewModel(Item item)
        {
            Description = item.Description;
            Count = item.Count;
        }
        public Item ToItem()
        {
            return new Item(Description, Count);
        }
    }
}
