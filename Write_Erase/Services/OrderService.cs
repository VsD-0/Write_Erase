using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Write_Erase.MVVM.Models;
using Write_Erase.MVVM.Models.Data.Tables;

namespace Write_Erase.Services
{
    public class OrderService
    {
        private readonly StoreContext _context;
        public OrderService(StoreContext context)
        {
            _context = context;
        }
        public async Task<List<OrderModel>> GetOrders()
        {
            List<OrderModel> orderModels = new();
            try
            {
                var orders = await _context.Orders
                .Include(o => o.OrderPickupPoint)
                .Include(o => o.OrderStatus)
                .Select(o => new
                {
                    Order = o,
                    Orderproducts = o.Orderproducts,
                    Products = o.Orderproducts.Select(op => op.ParticleNumberNavigation)
                })
                .ToListAsync();

                orderModels = orders.Select(o => new OrderModel
                {
                    OrderId = o.Order.OrderId,
                    OrderStatusId = o.Order.OrderStatusId,
                    OrderStatus = o.Order.OrderStatus.StatusName,
                    OrderDeliveryDate = o.Order.OrderDeliveryDate,
                    DateOfOrder = o.Order.DateOfOrder,
                    OrderPickupPointId = o.Order.OrderPickupPointId,
                    FullNameUser = o.Order.FullNameUser,
                    ReceiptCode = o.Order.ReceiptCode,
                    OrderAmmount = o.Products.Sum(p => p.Pcost * o.Orderproducts.FirstOrDefault(op => op.ParticleNumber == p.ParticleNumber)?.Count ?? 0),
                    OrderDiscountAmmount = o.Products.Sum(p => (decimal)p.PmaxDiscount),

                    Products = o.Products.ToList()
                }).ToList();
            }
            // Обработка исключения
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex);
                orderModels.Add(new OrderModel { });
            }
            return orderModels;
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
