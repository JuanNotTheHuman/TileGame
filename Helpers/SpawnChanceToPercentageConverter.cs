using System;
using System.Globalization;
using System.Windows.Data;

namespace TileGame.Helpers
{
    public class SpawnChanceToPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double spawnChance)
            {
                return spawnChance.ToString("P1", culture); // "P1" keeps one decimal place (e.g., "0.5%")
            }
            return "0.0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string strValue)
            {
                strValue = strValue.Replace("%", "").Trim(); // Remove percentage symbol and spaces

                if (double.TryParse(strValue, NumberStyles.Any, culture, out double result))
                {
                    return result / 100.0; // Convert "0.5" back to 0.005
                }
            }
            return 0.0;
        }
    }
}
