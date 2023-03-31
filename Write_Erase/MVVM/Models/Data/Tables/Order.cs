using System;
using System.Collections.Generic;

namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Order
{
    public int OrderId { get; set; }

    public int OrderStatusId { get; set; }

    public DateTime OrderDeliveryDate { get; set; }

    public DateTime DateOfOrder { get; set; }

    public int OrderPickupPointId { get; set; }

    public int ReceiptCode { get; set; }

    public string FullNameUser { get; set; } = null!;

    public virtual Orderpickuppoint OrderPickupPoint { get; set; } = null!;

    public virtual Orderstatus OrderStatus { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; } = new List<Orderproduct>();
}
