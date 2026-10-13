using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoEx.Models
{
    public partial class Product
    {
        public int Count => Stoks.Sum(x => x.Count);
        public string CountText => Count < 5?"мало":"много";

        public bool Discount => DetailOrders.All(x => x.Order.Date <= DateOnly.FromDateTime( DateTime.Now.AddMonths(-1)));

        public decimal DiscountPrice => Discount ? Price * (decimal)0.75 : Price;

        public IBrush CountColor
        {
            get
            {
                if (Count <= 3)
                    return new SolidColorBrush(Colors.OrangeRed);
                else
                    return Brushes.Transparent;
            }
        }

        public Bitmap ImageBitmap
        {
            get
            {
                try
                {
                    var uri = new Uri($"avares://DemoEx/Assets/Images/{Image}");
                    using var s = AssetLoader.Open(uri);
                    return new Bitmap(s);
                }
                catch
                {
                    using var s = AssetLoader.Open(new Uri("avares://DemoEx/Assets/picture.png"));
                    return new Bitmap(s);
                }
            }
        }
    }
}
