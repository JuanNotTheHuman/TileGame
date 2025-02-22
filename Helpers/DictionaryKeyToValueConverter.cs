using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using TileGame.Enums;
using TileGame.Models;
using TileGame.ViewModels;

namespace TileGame.Helpers
{
    public class DictionaryKeyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2)
                return null;

            var dictionary = values[0] as IDictionary<TileType, ObservableCollection<TileDrop>>;
            var key = values[1] as TileType?;

            if (dictionary == null || key == null)
                return null;

            return dictionary.TryGetValue(key.Value, out var drops)
                ? new ObservableCollection<TileDropViewModel>(drops.Select(d => new TileDropViewModel(d)))
                : new ObservableCollection<TileDropViewModel>();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
