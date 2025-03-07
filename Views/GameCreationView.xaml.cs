﻿using System;
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
    /// Logika interakcji dla klasy GameCreationView.xaml
    /// </summary>
    public partial class GameCreationView : Page
    {
        public GameCreationView()
        {
            InitializeComponent();
            DataContext = new GameCreationViewModel();
            ((MainWindow)App.Current.MainWindow).ViewModel = DataContext as GameCreationViewModel;
        }
    }
}
