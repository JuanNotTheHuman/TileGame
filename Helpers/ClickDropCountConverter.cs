using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TileGame.Enums;
using TileGame.Models;

namespace TileGame.Helpers
{
    public class ClickDropCountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2)
                return "0 invalid";

            var itemType = values[0];
            var clickDrops = values[1];
            Debug.WriteLine(values[0]);
            Debug.WriteLine(values[1]);
            // Debug output to check values
            System.Diagnostics.Debug.WriteLine($"ItemType: {itemType}, ClickDrops: {clickDrops}");

            if (clickDrops is ObservableDictionary<TileType, ObservableCollection<TileDrop>> dropsDict)
            {
                // Log all the keys in the dictionary for comparison
                foreach (var key in dropsDict.Keys)
                {
                    System.Diagnostics.Debug.WriteLine($"Key in dropsDict: {key}");
                }

                // Ensure the itemType matches the TileType enum exactly
                if (itemType is TileType tileType)
                {
                    System.Diagnostics.Debug.WriteLine($"ItemType as TileType: {tileType}");

                    // Check if the dictionary contains the key
                    if (dropsDict.ContainsKey(tileType))
                    {
                        var drops = dropsDict[tileType];
                        System.Diagnostics.Debug.WriteLine($"Drop Count for {tileType}: {drops.Count}");  // Debug to see count
                        return drops.Count.ToString();  // Return the count as a string
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"Key {tileType} not found in dropsDict");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"itemType is not of type TileType: {itemType}");
                }
            }

            return "0";  // Return "0" if no drops are found for the key.
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
