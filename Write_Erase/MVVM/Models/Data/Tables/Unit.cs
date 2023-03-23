namespace Write_Erase;

public partial class Unit
{
    public int Id { get; set; }

    public string UnitName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
