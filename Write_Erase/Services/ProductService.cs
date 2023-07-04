using Org.BouncyCastle.Crypto;
using System.Threading;
using AutoMapper;
using Write_Erase.MVVM.Models.Data.Tables;

namespace Write_Erase.Services
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private readonly StoreContext _context;
        static List<Product> _product;

        private static SemaphoreSlim _semaphoreSlim = new(1, 1);

        public ProductService(StoreContext context)
        {
            _context = context;
        }
        public async Task<List<ProductModel>> GetProducts()
        {
            await _semaphoreSlim.WaitAsync();
            List<ProductModel> products = new();
            try
            {
                _product = await _context.Products.ToListAsync();

                List<Productname> pnames = await _context.Productnames.ToListAsync();
                List<Productmanufacturer> pmanufactures = await _context.Productmanufacturers.ToListAsync();
                List<Productunit> productunits = await _context.Productunits.ToListAsync();

                foreach (var item in _product)
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
                        Unit = productunits.SingleOrDefault(pn => pn.UnitId == item.PunitId).Unit,
                        InStock = item.PquantityInStock,
                        Status = item.Pstatus,
                    });
                }
                return products;
            }
            finally { _semaphoreSlim.Release(); }
        }
        public async Task<Order> AddOrder(Order order)
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
                Product edit_pr = _product.First(i => i.ParticleNumber == item.Product.Article);
                edit_pr.PquantityInStock -= item.Count;
                _context.Products.Update(edit_pr);
                await _context.SaveChangesAsync();
            }

            return order;
        }

        public List<Productname> GetNames() => _context.Productnames.ToList();
        public List<Productprovider> GetProdivers() => _context.Productproviders.ToList();
        public List<Productcategory> GetPcategories() => _context.Productcategories.ToList();
        public List<Productmanufacturer> GetPmanufacturers() => _context.Productmanufacturers.ToList();

        public List<Order> GetOrders()
        {
            return _context.Orders.ToList();
        }
        public async Task<ProductModel> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            var dbProduct = new ProductModel
            {
                Article = product.ParticleNumber,
                Image = product.Pphoto == string.Empty ? "picture.png" : product.Pphoto,
                Title = product.Pname.Name,
                Description = product.Pdescription,
                Manufacturer = product.Pmanufacturer.Manufacturer,
                Price = product.Pcost,
                Discount = (int)product.PdiscountAmount,
                Unit = product.Punit.Unit,
                InStock = product.PquantityInStock,
                Status = product.Pstatus,
            };

            return dbProduct;
        }
        public async Task SaveChangesAsync(ProductModel pr)
        {
            Product edit_pr = _product.First(i => i.ParticleNumber == pr.Article);
            edit_pr.Pstatus = pr.Status;
            edit_pr.Pdescription = pr.Description;
            edit_pr.Pcost = pr.Price;
            edit_pr.PmaxDiscount = pr.Discount;
            edit_pr.PquantityInStock = pr.InStock;
            _context.Products.Update(edit_pr);
            await _context.SaveChangesAsync();
        }
    }
}
