using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TileGame.Enums;

namespace TileGame.Helpers
{
    public class EnumToImagePathConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo cultureInfo)
        {
            if(value is Enum tileType)
            {
                return $"../Images/{tileType}.png";
            }
            return null;
        }
        public object ConvertBack(object value, Type target, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
