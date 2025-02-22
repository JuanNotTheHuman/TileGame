using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TileGame.ViewModels;

namespace TileGame.Views
{
    /// <summary>
    /// Logika interakcji dla klasy GameView.xaml
    /// </summary>
    public partial class GameView : Page
    {
        public GameView()
        {
            InitializeComponent();
            InitializeGameViewMode();
        }
        private async void InitializeGameViewMode()
        {
            LoadingScreen.Visibility = Visibility.Visible;
            var gameViewModelTask = GameViewModel.CreateAsync();
            while (!gameViewModelTask.IsCompleted)
            {
                text.Content += ".";
                await Task.Delay(50);
            }
            var gameViewModel = gameViewModelTask.Result;
            DataContext = gameViewModel;
            LoadingScreen.Visibility = Visibility.Collapsed;
        }

    }
}
