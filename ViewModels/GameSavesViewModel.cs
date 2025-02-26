using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TileGame.Helpers;
using TileGame.Services;
using TileGame.Views;

namespace TileGame.ViewModels
{
    public class GameSavesViewModel : ViewModelBase
    {
        private NavigationService _navigationService { get; }
        private ObservableCollection<KeyValuePair<string, GameSaveViewModel>> _games;
        public ObservableCollection<KeyValuePair<string, GameSaveViewModel>> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged(nameof(Games));
            }
        }
        public ICommand LoadGameCommand { get; }
        public ICommand DeleteGameCommand { get; }
        public ICommand CancelCommand { get; }
        public GameSavesViewModel()
        {
            _games = new ObservableCollection<KeyValuePair<string, GameSaveViewModel>>(GameSaveService.LoadAllSaves().ToDictionary(pair => pair.Key, pair => new GameSaveViewModel(pair.Value)).ToList());
            _navigationService = new NavigationService(((MainWindow)App.Current.MainWindow).MainFrame);
            LoadGameCommand = new RelayCommand<GameSaveViewModel>(LoadGame);
            DeleteGameCommand = new RelayCommand<GameSaveViewModel>(DeleteGame);
            CancelCommand = new RelayCommand<object>(Cancel);
        }
        public void LoadGame(GameSaveViewModel game)
        {
            var save = GameSaveService.LoadSave(game.Name);
            _navigationService.NavigateToPage(new GameView(save));
        }
        public void DeleteGame(GameSaveViewModel game)
        {
            GameSaveService.DeleteSave(game.Name);
            _games.Remove(_games.First(pair => pair.Key == game.Name));
            OnPropertyChanged(nameof(Games));
        }
        public void Cancel(object source = null)
        {
            _navigationService.NavigateToPage(new MenuView());
        }
    }
}
