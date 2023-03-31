namespace Write_Erase.MVVM.Models
{
    public class ProductModel
    {
        public string? Article { get; set; }
        public string? Image { get; set; }
        public string? DisplayedImage
        {
            get { return Path.GetFullPath($@"Resources\Image\{Image}"); }
        }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Manufacturer { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public decimal DisplayedPrice
        {
            get { return this.Price - (this.Price / 100 * this.Discount); }
        }
    }
}
