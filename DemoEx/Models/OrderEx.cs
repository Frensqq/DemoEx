using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DemoEx.Models;

public partial class Order
{
    [NotMapped]
    public DateTimeOffset newDate { get; set; }

    [NotMapped]
    public decimal TotalSum => DetailOrders?.Sum(d => d.Count * d.Product.DiscountPrice) ?? 0;
}
       

