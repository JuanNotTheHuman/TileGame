using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Helpers;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class BaseTileConfigViewModel : ViewModelBase
    {
        private BaseTileConfig _config;
        private ObservableDictionary<ItemType, int> _clickDrops;
        private ObservableDictionary<ItemType, int> _deathDrops;
        public double SpawnChance
        {
            get { return Math.Round(_config.SpawnChance,3); }
            set
            {
                if (_config.SpawnChance != value)
                {
                    _config.SpawnChance = Math.Round(value,3);
                    OnPropertyChanged(nameof(SpawnChance));
                }
            }
        }
        public double Health
        {
            get { return _config.Health; }
            set
            {
                if (_config.Health != value)
                {
                    _config.Health = value;
                    OnPropertyChanged(nameof(Health));
                }
            }
        }
        public ObservableDictionary<ItemType, int> ClickDrops
        {
            get { return _clickDrops; }
            set
            {
                if (_clickDrops != value)
                {
                    _clickDrops = value;
                    OnPropertyChanged(nameof(ClickDrops));
                }
            }
        }
        public ObservableDictionary<ItemType, int> DeathDrops
        {
            get { return _deathDrops; }
            set
            {
                if (_deathDrops != value)
                {
                    _deathDrops = value;
                    OnPropertyChanged(nameof(DeathDrops));
                }
            }
        }
        public BaseTileConfigViewModel(BaseTileConfig config)
        {
            _config = config;
            if(config.ClickDrops == null)
            {
                config.ClickDrops = new Dictionary<ItemType, int>();
            }
            if(config.DeathDrops == null)
            {
                config.DeathDrops = new Dictionary<ItemType, int>();
            }
            _clickDrops = new ObservableDictionary<ItemType, int>(_config.ClickDrops);
            _deathDrops = new ObservableDictionary<ItemType, int>(_config.DeathDrops);
        }
        public BaseTileConfig ToBaseTileConfig()
        {
            return new BaseTileConfig(SpawnChance, Health, ClickDrops, DeathDrops);
        }
    }
    public class ForegroundTileConfigViewModel : BaseTileConfigViewModel
    {
        public ICommand ToggleSpawnableOn { get; }
        public ICommand IncrementClickDropCommand { get; }
        public ICommand DecrementClickDropCommand { get; }
        public ICommand IncrementDeathDropCommand { get; }
        public ICommand DecrementDeathDropCommand { get; }
        private ForegroundTileConfig _config;
        private ObservableCollection<TileType> _spawnableOn;
        public int Max
        {
            get { return _config.Max; }
            set
            {
                if (_config.Max != value)
                {
                    _config.Max = value;
                    OnPropertyChanged(nameof(Max));
                }
            }
        }
        public ObservableCollection<TileType> SpawnableOn
        {
            get { return _spawnableOn; }
            set
            {
                if (_spawnableOn != value)
                {
                    _spawnableOn = value;
                    OnPropertyChanged(nameof(SpawnableOn));
                }
            }
        }
        public SpawnBehavior SpawnBehavior
        {
            get { return _config.SpawnBehavior; }
            set
            {
                if (_config.SpawnBehavior != value)
                {
                    _config.SpawnBehavior = value;
                    OnPropertyChanged(nameof(SpawnBehavior));
                }
            }
        }
        public ForegroundTileConfigViewModel(ForegroundTileConfig config) : base(config)
        {
            _config = config;
            _spawnableOn = new ObservableCollection<TileType>(_config.SpawnableOn);
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
            OnPropertyChanged(nameof(ClickDrops));
        }
        private void DecrementClickDrop(ItemType itemType)
        {
            if (ClickDrops[itemType] > 0)
            {
                ClickDrops.TryUpdate(itemType, ClickDrops[itemType] - 1);
            }
            OnPropertyChanged(nameof(ClickDrops));
        }
        private void IncrementDeathDrop(ItemType itemType)
        {
            DeathDrops.TryUpdate(itemType, DeathDrops[itemType] + 1);
            OnPropertyChanged(nameof(DeathDrops));
        }
        private void DecrementDeathDrop(ItemType itemType)
        {
            if (DeathDrops[itemType] > 0)
            {
                DeathDrops.TryUpdate(itemType, DeathDrops[itemType] - 1);
            }
            OnPropertyChanged(nameof(DeathDrops));
        }
        public ForegroundTileConfig ToForegroundTileConfig()
        {
            return new ForegroundTileConfig(SpawnableOn, SpawnBehavior, SpawnChance, Health, Max, ClickDrops, DeathDrops);
        }
    }
}
