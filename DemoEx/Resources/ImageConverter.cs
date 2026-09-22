using Avalonia.Data.Converters;
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

public class ImageConverter : IValueConverter
{
    public object? Convert(object? v, Type t, object? p, CultureInfo c)
    {
        try
        {
            var uri = new Uri($"avares://DemoEx/Assets/Images/{v}");
            using var s = AssetLoader.Open(uri);
            return new Bitmap(s);
        }
        catch
        {
            using var s = AssetLoader.Open(new Uri("avares://DemoEx/Assets/picture.png"));
            return new Bitmap(s);
        }
    }

    public object? ConvertBack(object? v, Type t, object? p, CultureInfo c) => throw new NotSupportedException();
}