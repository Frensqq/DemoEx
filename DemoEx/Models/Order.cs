using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DemoEx.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string Login { get; set; } = null!;

    public virtual ObservableCollection<DetailOrder> DetailOrders { get; set; } = new();

    public virtual User LoginNavigation { get; set; } = null!;



}
