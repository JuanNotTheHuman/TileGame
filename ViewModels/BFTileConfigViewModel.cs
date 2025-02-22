using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class BaseTileConfigViewModel : INotifyPropertyChanged
    {
        private BaseTileConfig _config;
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
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public BaseTileConfigViewModel(BaseTileConfig config)
        {
            _config = config;
        }
    }
    public class ForegroundTileConfigViewModel : BaseTileConfigViewModel
    {
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
        public ICommand ToggleSpawnableOn { get; }
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
    }
}
