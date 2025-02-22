using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TickConfigViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private TickConfig _tickConfig;
        private int _interval;
        private int _maxtransform;
        public int Interval
        {
            get { return _interval; }
            set
            {
                if (_interval != value)
                {
                    _interval = value;
                    OnPropertyChanged(nameof(Interval));
                }
            }
        }
        public int MaxTransform
        {
            get { return _maxtransform; }
            set
            {
                if( _maxtransform != value)
                {
                    _maxtransform = value;
                    OnPropertyChanged(nameof(MaxTransform));
                }
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TickConfigViewModel(TickConfig tickConfig)
        {
            _tickConfig = tickConfig;
            Interval = _tickConfig.Interval;
            MaxTransform = _tickConfig.MaxTransform;
        }
    }
}
