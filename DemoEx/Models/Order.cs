using System;
using System.Collections.Generic;

namespace DemoEx.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string Login { get; set; } = null!;

    public virtual ICollection<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();

    public virtual User LoginNavigation { get; set; } = null!;



}
