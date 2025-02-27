using CommunityToolkit.Mvvm.ComponentModel;
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
    public class TileConfigViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableDictionary<TileType, BaseTileConfigViewModel> _baseGeneration;
        [ObservableProperty]
        private ObservableDictionary<TileType, ForegroundTileConfigViewModel> _foregroundGeneration;
        public ObservableDictionary<TileType, BaseTileConfigViewModel> BaseGeneration
        {
            get => _baseGeneration;
            set => SetProperty(ref _baseGeneration, value);
        }

        public ObservableDictionary<TileType, ForegroundTileConfigViewModel> ForegroundGeneration
        {
            get => _foregroundGeneration;
            set => SetProperty(ref _foregroundGeneration, value);
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