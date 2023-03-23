namespace Write_Erase;

public partial class Pname
{
    public int PnameId { get; set; }

    public string ProductName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
