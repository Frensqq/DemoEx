using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoEx.Models;

public partial class Product
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public int SubcategoryId { get; set; }

    public string Image { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Structure { get; set; } = null!;

    public decimal Price { get; set; }

    public int ManufactureId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<DetailOrder> DetailOrders { get; set; } = new List<DetailOrder>();

    public virtual Manufacturer Manufacture { get; set; } = null!;

    public virtual ICollection<Stok> Stoks { get; set; } = new List<Stok>();

    public virtual SubCategory Subcategory { get; set; } = null!;


}
