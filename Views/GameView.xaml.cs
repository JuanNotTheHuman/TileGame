using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TileGame.Models;
using TileGame.Services;
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
            _ = InitializeGameViewModel();
        }
        public GameView(Config config)
        {
            InitializeComponent();
            _ = InitializeGameViewModel(config);
        }

        public GameView(GameSave save)
        {
            InitializeComponent();
            _ = InitializeGameViewModel(save?.Board?.Config, save);
        }

        private async Task InitializeGameViewModel(Config config = null, GameSave save = null)
        {
            try
            {
                LoadingScreen.Visibility = Visibility.Visible;
                var gameViewModel = await GameViewModel.CreateAsync(config);
                if (save != null)
                {
                    gameViewModel.PlayerViewModel = new PlayerViewModel(save.Player);
                    gameViewModel.BoardViewModel = new BoardViewModel(save.Board);
                }
                if (App.Current.MainWindow is MainWindow mainWindow)
                {
                    mainWindow.ViewModel = gameViewModel;
                }
                DataContext = gameViewModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing GameView: {ex}");
            }
            finally
            {
                LoadingScreen.Visibility = Visibility.Collapsed;
                DayTimeDisplay.Visibility = Visibility.Visible;
            }
        }
    }
}