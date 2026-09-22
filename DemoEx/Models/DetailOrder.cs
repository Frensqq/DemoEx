using System;
using System.Collections.Generic;

namespace DemoEx.Models;

public partial class DetailOrder
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public int Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}
