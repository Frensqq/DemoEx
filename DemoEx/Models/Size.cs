using System;
using System.Collections.Generic;

namespace DemoEx.Models;

public partial class Size
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();

    public virtual ICollection<Stok> Stoks { get; set; } = new List<Stok>();
}
