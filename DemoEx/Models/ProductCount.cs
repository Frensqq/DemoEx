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

        public IBrush CountColor => Count <= 3
        ? new SolidColorBrush(Colors.OrangeRed)
        : Brushes.Transparent;

        public Bitmap ImageBitmap => LoadImage(Image);

        private static Bitmap LoadImage(string name)
        {
            try
            {
                using var s = AssetLoader.Open(new Uri($"avares://DemoEx/Assets/Images/{name}"));
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
