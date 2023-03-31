using System;
using System.Collections.Generic;

namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Productprovider
{
    public int ProviderId { get; set; }

    public string Provider { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
