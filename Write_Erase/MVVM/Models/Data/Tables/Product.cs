namespace Write_Erase;

public partial class Product
{
    public string ProductArticleNumber { get; set; } = null!;

    public int ProductName { get; set; }

    public string ProductDescription { get; set; } = null!;

    public int ProductCategory { get; set; }

    public string ProductPhoto { get; set; } = null!;

    public int ProductManufacturer { get; set; }

    public float ProductCost { get; set; }

    public int? ProductMaxDiscount { get; set; }

    public int ProductProvider { get; set; }

    public sbyte? ProductDiscountAmount { get; set; }

    public int ProductQuantityInStock { get; set; }

    public int? Unit { get; set; }

    public string? ProductStatus { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; } = new List<Orderproduct>();

    public virtual ProductCategory ProductCategoryNavigation { get; set; } = null!;

    public virtual Pmanufacturer ProductManufacturerNavigation { get; set; } = null!;

    public virtual Pname ProductNameNavigation { get; set; } = null!;

    public virtual Provider ProductProviderNavigation { get; set; } = null!;

    public virtual Unit? UnitNavigation { get; set; }
}
