using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TileGame.Enums;
using TileGame.Helpers;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TileConfigViewModel : INotifyPropertyChanged
    {
        private ObservableDictionary<TileType, BaseTileConfigViewModel> _baseGeneration;
        private ObservableDictionary<TileType, ForegroundTileConfigViewModel> _foregroundGeneration;
        private ObservableDictionary<TileType, ObservableCollection<TileDrop>> _clickDrops;
        private ObservableDictionary<TileType, ObservableCollection<TileDrop>> _deathDrops;
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableDictionary<TileType, BaseTileConfigViewModel> BaseGeneration
        {
            get { return _baseGeneration; }
            set
            {
                if (_baseGeneration != value)
                {
                    _baseGeneration = value;
                    OnPropertyChanged(nameof(BaseGeneration));
                }
            }
        }

        public ObservableDictionary<TileType, ForegroundTileConfigViewModel> ForegroundGeneration
        {
            get { return _foregroundGeneration; }
            set
            {
                if (_foregroundGeneration != value)
                {
                    _foregroundGeneration = value;
                    OnPropertyChanged(nameof(ForegroundGeneration));
                }
            }
        }

        public ObservableDictionary<TileType, ObservableCollection<TileDrop>> ClickDrops
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

        public ObservableDictionary<TileType, ObservableCollection<TileDrop>> DeathDrops
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

        public ICommand ModifyDropCommand { get; }

        public TileConfigViewModel(TileConfig config)
        {
            BaseGeneration = new ObservableDictionary<TileType, BaseTileConfigViewModel>(
                config.BaseGeneration.ToDictionary(
                    r => r.Key,
                    r => new BaseTileConfigViewModel(r.Value)
                )
            );

            ForegroundGeneration = new ObservableDictionary<TileType, ForegroundTileConfigViewModel>(
                config.ForegroundGeneration.ToDictionary(
                    r => r.Key,
                    r => new ForegroundTileConfigViewModel(r.Value)
                )
            );
            ClickDrops = new ObservableDictionary<TileType, ObservableCollection<TileDrop>>(
                config.ClickDrops.ToDictionary(
                    r => r.Key,
                    r => new ObservableCollection<TileDrop>(r.Value)
                )
            );
            DeathDrops = new ObservableDictionary<TileType, ObservableCollection<TileDrop>>(
                config.DeathDrops.ToDictionary(
                    r => r.Key,
                    r => new ObservableCollection<TileDrop>(r.Value)
                )
            );

            ModifyDropCommand = new RelayCommand<object>(ModifyDrop);
        }

        private void ModifyDrop(object parameter)
        {
            if (!(parameter is Tuple<TileType, TileDrop, bool, bool> args))
                return;

            var (tileType, drop, isIncrement, isClickDrop) = args;
            var targetCollection = isClickDrop ? ClickDrops : DeathDrops;

            if (!targetCollection.ContainsKey(tileType))
            {
                targetCollection[tileType] = new ObservableCollection<TileDrop>();
            }
            var drops = targetCollection[tileType];
            var existingDrop = drops.FirstOrDefault(d => d.Type == drop.Type);

            if (existingDrop == null)
            {
                existingDrop = new TileDrop(drop.Type);
                drops.Add(existingDrop);
            }

            if (isIncrement)
            {
                existingDrop.Count++;
            }
            else if (existingDrop.Count > 0)
            {
                existingDrop.Count--;
                if (existingDrop.Count == 0)
                {
                    drops.Remove(existingDrop);
                }
            }
            OnPropertyChanged(isClickDrop ? nameof(ClickDrops) : nameof(DeathDrops));
        }
    }
}