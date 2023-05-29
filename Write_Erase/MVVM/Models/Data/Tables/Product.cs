using System;
using System.Collections.Generic;

namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Product
{
    public string ParticleNumber { get; set; } = null!;

    public int PnameId { get; set; }

    public string Pdescription { get; set; } = null!;

    public int PcategoryId { get; set; }

    public string Pphoto { get; set; } = null!;

    public int PmanufacturerId { get; set; }

    public decimal Pcost { get; set; }

    public int? PmaxDiscount { get; set; }

    public decimal? PdiscountAmount { get; set; }

    public int PquantityInStock { get; set; }

    public int Pstatus { get; set; }

    public int PproviderId { get; set; }

    public int PunitId { get; set; }

    public virtual ICollection<Orderproduct> Orderproducts { get; } = new List<Orderproduct>();

    public virtual Productcategory Pcategory { get; set; } = null!;

    public virtual Productmanufacturer Pmanufacturer { get; set; } = null!;

    public virtual Productname Pname { get; set; } = null!;

    public virtual Productprovider Pprovider { get; set; } = null!;

    public virtual Productunit Punit { get; set; } = null!;
}
