﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TileGame.Helpers
{
    public class BooleanToBorderThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool isSelected && isSelected) ? new Thickness(3) : new Thickness(0);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
