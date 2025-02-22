using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.Helpers
{
    public class DropCommandConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string action && value is Enum itemType)
            {
                return new Tuple<Enum, string>(itemType, action);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
