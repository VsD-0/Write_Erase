using System;
using System.Collections.Generic;

namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Orderpickuppoint
{
    public int PickupPointId { get; set; }

    public string PickupPoint { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string House { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
