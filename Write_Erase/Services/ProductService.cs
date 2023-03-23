

namespace Write_Erase.Services
{
    public class ProductService
    {
        private readonly TradeContext _context;
        public ProductService(TradeContext context)
        {
            _context = context;
        }
        public async Task<List<ProductModel>> GetProducts()
        {
            List<ProductModel> products = new();

            await Task.Run(async () =>
            {
                try
                {
                    List<Product> product = await _context.Products.ToListAsync();

                    List<Pname> pnames = await _context.Pnames.ToListAsync();
                    List<Pmanufacturer> pmanufactures = await _context.Pmanufacturers.ToListAsync();

                    foreach (var item in product)
                    {
                        products.Add(new ProductModel
                        {
                            Article = item.ProductArticleNumber,
                            Image = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                            Title = pnames.SingleOrDefault(pn => pn.PnameId == item.ProductName).ProductName,
                            Description = item.ProductDescription,
                            Manufacturer = pmanufactures.SingleOrDefault(pm => pm.PmanufacturerId == item.ProductManufacturer).ProductManufacturer,
                            Price = item.ProductCost,
                            Discount = (int)item.ProductDiscountAmount
                        });
                    }
                }
                catch (InvalidOperationException ex) { Debug.WriteLine(ex); }
            });
            return products;
        }
        public async Task<int> AddOrder(Orderuser order)
        {
            await _context.Orderusers.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in Global.ProductsBasket)
            {
                await _context.Orderproduct.AddAsync(new Orderproduct
                {
                    OrderId = order.OrderId,
                    ProductArticleNumber = item.Product.Article,
                    ProductCount = item.Count
                });
                await _context.SaveChangesAsync();
            }

            return order.OrderId;
        }
    }
}
