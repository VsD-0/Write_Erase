namespace Write_Erase;

public partial class Orderuser
{
    public int OrderId { get; set; }

    public int OrderStatus { get; set; }

    public DateTime OrderDeliveryDate { get; set; }

    public DateTime OrderPickupPoint { get; set; }

    public int PointOfIssue { get; set; }

    public string? FullNameClient { get; set; }

    public int ReceiptCode { get; set; }

    public virtual OrderStatus OrderStatusNavigation { get; set; } = null!;

    public virtual ICollection<Orderproduct> Orderproducts { get; } = new List<Orderproduct>();
}
