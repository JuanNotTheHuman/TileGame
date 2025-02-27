using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Helpers;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public partial class BaseTileConfigViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableDictionary<ItemType, int> _clickDrops;
        [ObservableProperty]
        private ObservableDictionary<ItemType, int> _deathDrops;
        [ObservableProperty]
        private double _spawnChance;
        [ObservableProperty]
        private double _health;
        public ObservableDictionary<ItemType, int> ClickDrops
        {
            get => _clickDrops;
            set => SetProperty(ref _clickDrops, value);
        }
        public ObservableDictionary<ItemType,int> DeathDrops
        {
            get => _deathDrops;
            set => SetProperty(ref _deathDrops, value);
        }
        public double SpawnChance
        {
            get => _spawnChance;
            set => SetProperty(ref _spawnChance, value);
        }
        public double Health
        {
            get => _health;
            set => SetProperty(ref _health, value);
        }

        public BaseTileConfigViewModel(BaseTileConfig config)
        {
            SpawnChance = config.SpawnChance;
            Health = config.Health;
            if (config.ClickDrops == null)
            {
                config.ClickDrops = new Dictionary<ItemType, int>();
            }
            if (config.DeathDrops == null)
            {
                config.DeathDrops = new Dictionary<ItemType, int>();
            }
            ClickDrops = new ObservableDictionary<ItemType, int>(config.ClickDrops);
            DeathDrops = new ObservableDictionary<ItemType, int>(config.DeathDrops);
        }

        public BaseTileConfig ToBaseTileConfig()
        {
            return new BaseTileConfig(SpawnChance, Health, ClickDrops, DeathDrops);
        }
    }

    public partial class ForegroundTileConfigViewModel : BaseTileConfigViewModel
    {
        public ICommand ToggleSpawnableOn { get; }
        public ICommand IncrementClickDropCommand { get; }
        public ICommand DecrementClickDropCommand { get; }
        public ICommand IncrementDeathDropCommand { get; }
        public ICommand DecrementDeathDropCommand { get; }
        [ObservableProperty]
        private ObservableCollection<TileType> _spawnableOn;
        [ObservableProperty]
        private int _max;
        [ObservableProperty]
        private SpawnBehavior _spawnBehavior;
        public int Max
        {
            get => _max;
            set => SetProperty(ref _max, value);
        }

        public SpawnBehavior SpawnBehavior
        {
            get => _spawnBehavior;
            set => SetProperty(ref _spawnBehavior, value);
        }
        public ObservableCollection<TileType> SpawnableOn
        {
            get => _spawnableOn;
            set => SetProperty(ref _spawnableOn, value);
        }
        public ForegroundTileConfigViewModel(ForegroundTileConfig config) : base(config)
        {
            SpawnableOn = new ObservableCollection<TileType>(config.SpawnableOn);
            SpawnBehavior = config.SpawnBehavior;
            ToggleSpawnableOn = new RelayCommand<TileType>(ToggleSpawnableOnExecute);
            IncrementClickDropCommand = new RelayCommand<ItemType>(IncrementClickDrop);
            DecrementClickDropCommand = new RelayCommand<ItemType>(DecrementClickDrop);
            IncrementDeathDropCommand = new RelayCommand<ItemType>(IncrementDeathDrop);
            DecrementDeathDropCommand = new RelayCommand<ItemType>(DecrementDeathDrop);
        }

        private void ToggleSpawnableOnExecute(TileType tileType)
        {
            if (SpawnableOn.Contains(tileType))
            {
                SpawnableOn.Remove(tileType);
            }
            else
            {
                SpawnableOn.Add(tileType);
            }
            OnPropertyChanged(nameof(SpawnableOn));
        }

        private void IncrementClickDrop(ItemType itemType)
        {
            ClickDrops.TryUpdate(itemType, ClickDrops[itemType] + 1);
        }

        private void DecrementClickDrop(ItemType itemType)
        {
            if (ClickDrops[itemType] > 0)
            {
                ClickDrops.TryUpdate(itemType, ClickDrops[itemType] - 1);
            }
        }

        private void IncrementDeathDrop(ItemType itemType)
        {
            DeathDrops.TryUpdate(itemType, DeathDrops[itemType] + 1);
        }

        private void DecrementDeathDrop(ItemType itemType)
        {
            if (DeathDrops[itemType] > 0)
            {
                DeathDrops.TryUpdate(itemType, DeathDrops[itemType] - 1);
            }
        }

        public ForegroundTileConfig ToForegroundTileConfig()
        {
            return new ForegroundTileConfig(SpawnableOn, SpawnBehavior, SpawnChance, Health, Max, ClickDrops, DeathDrops);
        }
    }
}
