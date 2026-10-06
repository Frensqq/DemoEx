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
    }
}
