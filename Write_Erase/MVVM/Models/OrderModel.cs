namespace Write_Erase.MVVM.Models
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime OrderDeliveryDate { get; set; }
        public DateTime DateOfOrder { get; set; }
        public int OrderPickupPointId { get; set; }
        public int ReceiptCode { get; set; }
        public string FullNameUser { get; set; } = null!;
        public decimal OrderAmmount { get; set; }
        public decimal OrderDiscountAmmount { get; set; }
        public List<int> Counts { get; set; }
        public List<ProductModel> Products { get; set; }
    }
}
