﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class ItemViewModel : ViewModelBase
    {
        public Item Item { get; }
        public string Description
        {
            get => Item.Description;
        }
        public int Count
        {
            get => Item.Count;
            set
            {
                if(Item.Count != value)
                {
                    Item.Count = value;
                    OnPropertyChanged(nameof(Count));
                }
            }
        }
        public ItemViewModel(Item item)
        {
            Item = item;
            Count = item.Count;
        }
        public Item ToItem()
        {
            return new Item(Item.Description, Item.Count);
        }
    }
}
