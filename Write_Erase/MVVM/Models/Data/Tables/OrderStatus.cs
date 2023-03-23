namespace Write_Erase;

public partial class OrderStatus
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Orderuser> Orderusers { get; } = new List<Orderuser>();
}
