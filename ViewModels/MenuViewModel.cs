using System.Windows.Input;
using TileGame.Views;
using TileGame;
using TileGame.Services;
namespace TileGame.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ICommand StartNewGameCommand { get; set; }
        public ICommand ContinueGameCommand { get; set; }

        public MenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            StartNewGameCommand = new RelayCommand<object>(CreateGame);
            ContinueGameCommand = new RelayCommand<object>(ContinueGame);
        }

        private void CreateGame(object obj)
        {
            _navigationService.NavigateToPage(new GameCreationView());
        }
        private void ContinueGame(object source = null)
        {
            _navigationService.NavigateToPage(new GameSavesView());
        }
        public MenuViewModel()
        {
            if (App.Current.MainWindow is MainWindow mainWindow)
            {
                _navigationService = new NavigationService(mainWindow.MainFrame);
            }
            StartNewGameCommand = new RelayCommand<object>(CreateGame);
        }

    }
}

