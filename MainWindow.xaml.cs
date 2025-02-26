using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TileGame.Models;
using TileGame.Services;
using TileGame.ViewModels;
using TileGame.Views;

namespace TileGame
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private ViewModelBase _viewmodel;
        private NavigationService _navigationService;
        public ViewModelBase ViewModel
        {
            get { return _viewmodel; }
            set
            {
                _viewmodel = value;
                OnPropertyChanged(nameof(ViewModel));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            _navigationService = new NavigationService(MainFrame);
            Closing += MainWindow_Closing;
            _navigationService.NavigateToPage(new TradeView(new PlayerViewModel(new Player())));
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                var gameSaveService = GameSaveService.Current;
                var viewModel = ViewModel as GameViewModel;
                if(viewModel == null)
                {
                    return;
                }
                var playerData = viewModel.PlayerViewModel.ToPlayer();
                var boardData = viewModel.BoardViewModel.ToBoard();
                GameSaveService.Save(gameSaveService.Name, new GameSave(gameSaveService.Name, playerData, boardData));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in MainWindow_Closing: {ex}");
            }
        }
    }
}
