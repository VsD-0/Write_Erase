namespace Write_Erase.MVVM.Models.Data.Tables;

public partial class Orderproduct
{
    public int OrderId { get; set; }

    public string ParticleNumber { get; set; } = null!;

    public int Count { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ParticleNumberNavigation { get; set; } = null!;
}
