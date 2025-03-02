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
using TileGame.Models;
using TileGame.ViewModels;

namespace TileGame.Views
{
    /// <summary>
    /// Logika interakcji dla klasy TradeView.xaml
    /// </summary>
    public partial class TradeView : Page
    {
        public TradeView()
        {
            InitializeComponent();
            DataContext = new TradeViewViewModel();
            (App.Current.MainWindow as MainWindow).ViewModel = DataContext as TradeViewViewModel;
        }
        public TradeView(PlayerViewModel player)
        {
            InitializeComponent();
            DataContext = new TradeViewViewModel(player);
            (App.Current.MainWindow as MainWindow).ViewModel = DataContext as TradeViewViewModel;
        }
    }
}
