using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        }
        public TileConfig ToTileConfig()
        {
            var baseGeneration = new Dictionary<TileType, BaseTileConfig>(BaseGeneration.ToDictionary(r => r.Key, r => r.Value.ToBaseTileConfig()));
            var foregroundGeneration = new Dictionary<TileType, ForegroundTileConfig>(ForegroundGeneration.ToDictionary(r => r.Key, r => r.Value.ToForegroundTileConfig()));
            return new TileConfig(baseGeneration, foregroundGeneration);
        }
    }
}