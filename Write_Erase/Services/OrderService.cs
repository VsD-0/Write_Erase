using System.Threading;

namespace Write_Erase.Services
{
    public class OrderService
    {
        private static SemaphoreSlim _semaphoreSlim = new(1, 1);
        private readonly StoreContext _context;
        private readonly ProductService _productService;

        public OrderService(StoreContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }
        public async Task<List<OrderModel>> GetOrders()
        {
            await _semaphoreSlim.WaitAsync();
            List<OrderModel> orderModels = new();
            try
            {
                var products = await _productService.GetProducts();

                var orders = await _context.Orders
                    .Include(o => o.OrderPickupPoint)
                    .Include(o => o.OrderStatus)
                    .Include(o => o.Orderproducts)
                        .ThenInclude(op => op.ParticleNumberNavigation)
                            .ThenInclude(p => p.Pname)
                    .Include(o => o.Orderproducts)
                        .ThenInclude(op => op.ParticleNumberNavigation)
                            .ThenInclude(p => p.Pmanufacturer)
                    .Include(o => o.Orderproducts)
                        .ThenInclude(op => op.ParticleNumberNavigation)
                            .ThenInclude(p => p.Punit)
                    .ToListAsync();

                orderModels = orders.Select(o => new OrderModel
                {
                    OrderId = o.OrderId,
                    OrderStatusId = o.OrderStatusId,
                    OrderStatus = o.OrderStatus.StatusName,
                    OrderDeliveryDate = o.OrderDeliveryDate,
                    DateOfOrder = o.DateOfOrder,
                    OrderPickupPointId = o.OrderPickupPointId,
                    FullNameUser = o.FullNameUser,
                    ReceiptCode = o.ReceiptCode,
                    OrderAmmount = o.Orderproducts.Sum(op => products.FirstOrDefault(p => p.Article == op.ParticleNumberNavigation.ParticleNumber)?.Price * op.Count ?? 0),
                    OrderDiscountAmmount = o.Orderproducts.Sum(op => (op.ParticleNumberNavigation.Pcost - (op.ParticleNumberNavigation.PmaxDiscount ?? 0) / 100 * op.ParticleNumberNavigation.Pcost) * op.Count),
                    Products = o.Orderproducts.Select(op => new ProductModel
                    {
                        Article = op.ParticleNumberNavigation.ParticleNumber,
                        Image = op.ParticleNumberNavigation.Pphoto == string.Empty ? "picture.png" : op.ParticleNumberNavigation.Pphoto,
                        Title = op.ParticleNumberNavigation.Pname.Name,
                        Description = op.ParticleNumberNavigation.Pdescription,
                        Manufacturer = op.ParticleNumberNavigation.Pmanufacturer.Manufacturer,
                        Price = op.ParticleNumberNavigation.Pcost,
                        Discount = (int)op.ParticleNumberNavigation.PdiscountAmount,
                        Unit = op.ParticleNumberNavigation.Punit.Unit,
                        Count = op.Count
                    }).ToList()
                }).ToList();
                return orderModels;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
             