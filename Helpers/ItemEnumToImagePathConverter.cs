using System;
using System.Globalization;
using System.Windows.Data;

namespace TileGame.Helpers
{
    public class ItemEnumToImagePathConverter : IValueConverter
    {
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {
            if(value is Enum vl)
            {
                return $"../Images/{vl}.png";
            }
            return null;
        }
        public object ConvertBack(object value, Type target, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
