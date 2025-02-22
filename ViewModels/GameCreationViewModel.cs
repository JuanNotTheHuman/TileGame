using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TileGame.Models;
using TileGame.Views;

namespace TileGame.ViewModels
{
    public class GameCreationViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
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
            CancelCommand = new RelayCommand<object>(Cancel);
            CreateWorldCommand = new RelayCommand<object>(CreateWorld);
            if(App.Current.MainWindow is MainWindow mainWindow)
            {
                _navigationService = new NavigationService(mainWindow.MainFrame);
            }
        }
        public ICommand CancelCommand { get; }
        public ICommand CreateWorldCommand { get; }
        private void Cancel(object source = null)
        {
            _navigationService.NavigateToPage(new MenuView());
        }
        private void CreateWorld(object source = null)
        {
            _navigationService.NavigateToPage(new GameView(Config.ToConfig()));
        }
    }
}
