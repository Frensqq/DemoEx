using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DemoEx.Resources;

public class ColorConverter : IValueConverter
{
    public object? Convert(object? v, Type t, object? p, CultureInfo c)
    {
        if(v!=null && v is int value && value <=3)
        {
            return new SolidColorBrush(Colors.OrangeRed) ;
        }
        else { 
            return  Brushes.Transparent ; 
        }
    }

    public object? ConvertBack(object? v, Type t, object? p, CultureInfo c) => throw new NotSupportedException();
}