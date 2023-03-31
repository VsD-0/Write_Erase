using System;
using System.Collections.Generic;

namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Orderstatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
