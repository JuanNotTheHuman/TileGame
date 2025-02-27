using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TickConfigViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _interval;
        [ObservableProperty]
        private int _maxtransform;
        public int Interval
        {
            get => _interval;
            set=> SetProperty(ref _interval, value);
        }
        public int MaxTransform
        {
            get => _maxtransform;
            set => SetProperty(ref _maxtransform, value);
        }
        public TickConfigViewModel(TickConfig tickConfig)
        {
            Interval = tickConfig.Interval;
            MaxTransform = tickConfig.MaxTransform;
        }
        public TickConfig ToTickConfig()
        {
            return new TickConfig(Interval, MaxTransform);
        }
    }
}
