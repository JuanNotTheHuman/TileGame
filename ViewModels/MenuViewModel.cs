using System.Windows.Input;
using TileGame.Views;
using TileGame;
using TileGame.Services;
using System.Diagnostics;
namespace TileGame.ViewModels
{
    public class MenuViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand StartNewGameCommand { get; set; }
        public ICommand ContinueGameCommand { get; set; }

        public MenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            StartNewGameCommand = new RelayCommand<object>(CreateGame);
            ContinueGameCommand = new RelayCommand<object>(ContinueGame);
            Debug.WriteLine("Initialized MenuViewModel");
        }
        private void CreateGame(object obj)
        {
            Debug.WriteLine("A");
            _navigationService.NavigateToPage(new GameCreationView());
        }
        private void ContinueGame(object source = null)
        {
            Debug.WriteLine("B");
            _navigationService.NavigateToPage(new GameSavesView());
        }
        public MenuViewModel()
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                _navigationService = new NavigationService(mainWindow.MainFrame);
            }
            StartNewGameCommand = new RelayCommand<object>(CreateGame);
            ContinueGameCommand = new RelayCommand<object>(ContinueGame);
        }

    }
}

