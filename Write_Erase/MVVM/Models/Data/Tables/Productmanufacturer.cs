using System;
using System.Collections.Generic;

namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Productmanufacturer
{
    public int ManufacturerId { get; set; }

    public string Manufacturer { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
