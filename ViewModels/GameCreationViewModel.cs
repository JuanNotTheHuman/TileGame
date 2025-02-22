using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class GameCreationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ConfigViewModel _config;
        public ConfigViewModel Config
        {
            get { return _config; }
            set
            {
                _config = value;
                OnPropertyChanged(nameof(Config));
            }
        }
        public GameCreationViewModel(ConfigViewModel config)
        {
            Config = config;
        }
        public GameCreationViewModel()
        {
            Config = new ConfigViewModel(new Config());
        }
    }
}
