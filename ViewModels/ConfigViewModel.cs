using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class ConfigViewModel : ObservableObject
    {
        public ICommand RandomizeSeed { get; }
        [ObservableProperty]
        private int _seed;
        [ObservableProperty]
        private TickConfigViewModel _tickConfig;
        [ObservableProperty]
        private TileConfigViewModel _tileConfig;
        public TickConfigViewModel Tick
        {
            get => _tickConfig;
            set=> SetProperty(ref _tickConfig, value);
        }
        public TileConfigViewModel Tiles
        {
            get => _tileConfig;
            set => SetProperty(ref _tileConfig, value);
        }
        public int Seed
        {
            get => _seed;
            set => SetProperty(ref _seed, value);
        }
        public ConfigViewModel(Config config)
        {
            Seed = config.Seed;
            Tick = new TickConfigViewModel(config.Tick);
            Tiles = new TileConfigViewModel(config.Tiles);
            RandomizeSeed = new RelayCommand<object>(GetRandomSeed);
        }
        private void GetRandomSeed(object source=null)
        {
            Seed=(int)DateTime.Now.Ticks & 0x0000FFFF;
            OnPropertyChanged(nameof(Seed));
        }
        public Config ToConfig()
        {
            return new Config(Seed, Tick.ToTickConfig(), Tiles.ToTileConfig());
        }
    }
}
