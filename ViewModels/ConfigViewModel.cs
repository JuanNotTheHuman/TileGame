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
    public class ConfigViewModel : ViewModelBase
    {
        public ICommand RandomizeSeed { get; }
        private int _seed;
        private Config _config;
        private TickConfigViewModel _tickConfig;
        private TileConfigViewModel _tileConfig;
        public Config Config
        {
            get
            {
                return _config;
            }
        }
        public TickConfigViewModel Tick
        {
            get => _tickConfig;
            set
            {
                if (_tickConfig != value)
                {
                    _tickConfig = value;
                    OnPropertyChanged(nameof(Tick));
                }
            }
        }
        public TileConfigViewModel Tiles
        {
            get => _tileConfig;
            set
            {
                if (_tileConfig != value)
                {
                    _tileConfig = value;
                    OnPropertyChanged(nameof(Tiles));
                }
            }
        }
        public int Seed
        {
            get => _seed;
            set
            {
                if (_seed != value)
                {
                    _seed = value;
                    OnPropertyChanged(nameof(Seed));
                }
            }
        }
        public ConfigViewModel(Config config)
        {
            _config = config;
            Seed = _config.Seed;
            Tick = new TickConfigViewModel(_config.Tick);
            Tiles = new TileConfigViewModel(_config.Tiles);
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
