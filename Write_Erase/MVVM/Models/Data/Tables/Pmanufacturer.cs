namespace Write_Erase;

public partial class Pmanufacturer
{
    public int PmanufacturerId { get; set; }

    public string ProductManufacturer { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
