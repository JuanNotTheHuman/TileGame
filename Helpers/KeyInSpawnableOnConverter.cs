using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using TileGame.Enums;
namespace TileGame.Helpers
{
    public class KeyInSpawnableOnConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2 || values[0] == null || values[1] == null)
                return Brushes.Transparent;

            if (values[0] is TileType key && values[1] is IEnumerable<TileType> spawnableOn)
            {
                return spawnableOn.Contains(key) ? Brushes.Green : Brushes.Transparent;
            }

            return Brushes.Transparent;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[2];
        }
    }
}
