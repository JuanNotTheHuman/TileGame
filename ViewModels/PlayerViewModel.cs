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
    public class PlayerViewModel : ViewModelBase
    {
        private Player Player { get; }
        private InventoryViewModel _inventory;
        public InventoryViewModel Inventory
        {
            get => _inventory;
            set
            {
                if (_inventory != value)
                {
                    _inventory = value;
                    OnPropertyChanged(nameof(Inventory));
                }
            }
        }
        public PlayerViewModel(Player player)
        {
            Player = player;
            Inventory = new InventoryViewModel(player.Inventory);
        }
        public Player ToPlayer()
        {
            return new Player(Inventory.ToInventory());
        }
    }
}
