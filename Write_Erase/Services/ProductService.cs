namespace Write_Erase.Services
{
    public class ProductService
    {
        private readonly StoreContext _context;
        public ProductService(StoreContext context)
        {
            _context = context;
        }
        public async Task<List<ProductModel>> GetProducts()
        {
            List<ProductModel> products = new();
            try
            {
                List<Product> product = await _context.Products.ToListAsync();

                List<Productname> pnames = await _context.Productnames.ToListAsync();
                List<Productmanufacturer> pmanufactures = await _context.Productmanufacturers.ToListAsync();
                List<Productunit> productunits = await _context.Productunits.ToListAsync();

                foreach (var item in product)
                {
                    products.Add(new ProductModel
                    {
                        Article = item.ParticleNumber,
                        Image = item.Pphoto == string.Empty ? "picture.png" : item.Pphoto,
                        Title = pnames.SingleOrDefault(pn => pn.NameId == item.PnameId).Name,
                        Description = item.Pdescription,
                        Manufacturer = pmanufactures.SingleOrDefault(pm => pm.ManufacturerId == item.PmanufacturerId).Manufacturer,
                        Price = item.Pcost,
                        Discount = (int)item.PdiscountAmount,
                        Unit = productunits.SingleOrDefault(pn => pn.UnitId == item.PunitId).Unit
                    });
                }
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
                products.Add(new ProductModel
                {
                    Article = "#Article#",
                    Image = "picture.png",
                    Title = "#Имя?#",
                    Description = "#Описание?#",
                    Manufacturer = "#Производитель?#",
                    Price = 100,
                    Discount = 5
                });
            }
            return products;
        }
        public async Task<int> AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in Global.ProductsBasket)
            {
                await _context.Orderproducts.AddAsync(new Orderproduct
                {
                    OrderId = order.OrderId,
                    ParticleNumber = item.Product.Article,
                    Count = item.Count
                });
                await _context.SaveChangesAsync();
            }

            return order.OrderId;
        }
    }
}
