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
using System.Windows.Shapes;
using TileGame.Services;
using TileGame.ViewModels;

namespace TileGame.Views
{
    /// <summary>
    /// Logika interakcji dla klasy MenuView.xaml
    /// </summary>
    public sealed partial class MenuView : Page
    {
        public MenuView()
        {
            InitializeComponent();
            var navigationService = new NavigationService(((MainWindow)App.Current.MainWindow).MainFrame);
            DataContext = new MenuViewModel(navigationService);
            ((MainWindow)App.Current.MainWindow).ViewModel = DataContext as MenuViewModel;
        }
    }
}
