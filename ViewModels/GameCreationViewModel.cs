using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TileGame.Helpers;
using TileGame.Models;
using TileGame.Services;
using TileGame.Views;

namespace TileGame.ViewModels
{
    public class GameCreationViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ConfigViewModel _config;
        private string _worldName;
        public string WorldName
        {
            get { return _worldName; }
            set
            {
                _worldName = value;
                OnPropertyChanged(nameof(WorldName));
            }
        }
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
            CreateWorldCommand = new RelayCommand<object>((source)=>_ = CreateWorld(source));
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
        private async Task CreateWorld(object source = null)
        {
            if (string.IsNullOrWhiteSpace(WorldName))
            {
               MessageBox.Show("World name is not set!");
                return;
            }
            var board = await Board.CreateAsync(1920, 1080, Config.ToConfig());
            var save = GameSaveService.AddSave(WorldName, new GameSave(WorldName, new Player(), board));
            Debug.WriteLine($"Saved...: {save.Board.TileGrid.Count}");
            _navigationService.NavigateToPage(new GameView(save));
        }

    }
}
