using System;
using System.Collections.Generic;

namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Productunit
{
    public int UnitId { get; set; }

    public string Unit { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
