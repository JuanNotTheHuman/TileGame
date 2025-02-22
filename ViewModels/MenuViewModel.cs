using System.Windows.Input;
using TileGame.Views;
using TileGame;
namespace TileGame.ViewModels
{
    public class MenuViewModel
    {
        private readonly INavigationService _navigationService;

        public ICommand StartNewGameCommand { get; set; }

        public MenuViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            StartNewGameCommand = new RelayCommand<object>(CreateGame);
        }

        private void CreateGame(object obj)
        {
            _navigationService.NavigateToPage(new GameView());
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

