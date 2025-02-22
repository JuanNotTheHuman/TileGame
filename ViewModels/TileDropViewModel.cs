using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.ViewModels
{
    public class TileDropViewModel : INotifyPropertyChanged
    {
        private TileDrop _tileDrop;
        public event PropertyChangedEventHandler PropertyChanged;
        public ItemType Type
        {
            get { return _tileDrop.Type; }
            set
            {
                if (_tileDrop.Type != value)
                {
                    _tileDrop.Type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }
        }
        public int Count
        {
            get { return _tileDrop.Count; }
            set
            {
                if (_tileDrop.Count != value)
                {
                    _tileDrop.Count = value;
                    OnPropertyChanged(nameof(Count));
                }
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public TileDropViewModel(TileDrop tileDrop)
        {
            _tileDrop = tileDrop;
        }
    }
}
