using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Erase.MVVM.ViewModels
{
    public class BrowseAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        private readonly OrderService _orderService;

        public List<string> Sorts { get; set; } = new() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "Новый", "Завершен" };
        public List<string> OrderFilters { get; set; } = new() { "Новый", "Завершен" };
        public ObservableCollection<Order> Orders { get; set; }
        public List<OrderModel> Order_m { get; set; }
        public string FullName { get; set; } = Global.CurrentUser == null ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";
        public int? MaxRecords { get; set; } = 0;
        public int? Records { get; set; } = 0;
        public string SelectedSort
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }
        public string SelectedFilter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }
        public string Search
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: UpdateProduct); }
        }
        public BrowseAdminViewModel(PageService pageService, ProductService productService, OrderService orderService)
        {
            _pageService = pageService;
            _productService = productService;
            _orderService = orderService;
        }
        private async void UpdateProduct()
        {
            var currentOrders = _productService.GetOrders();
            MaxRecords = currentOrders.Count;

            if (!string.IsNullOrEmpty(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "Новый":
                        currentOrders = currentOrders.Where(c => c.OrderStatusId == 1).ToList();
                        break;
                    case "Завершен":
                        currentOrders = currentOrders.Where(c => c.OrderStatusId == 2).ToList();
                        break;
                }
            }

            if (!string.IsNullOrEmpty(Search))
                currentOrders = currentOrders.Where(p => p.OrderId.ToString().ToLower().Contains(Search.ToLower())).ToList();

            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По возрастанию":
                        currentOrders = currentOrders.OrderBy(o => o.DateOfOrder).ToList();
                        break;
                    case "По убыванию":
                        currentOrders = currentOrders.OrderByDescending(o => o.DateOfOrder).ToList();
                        break;
                }
            }

            Records = currentOrders.Count;
            Orders = new ObservableCollection<Order>(currentOrders);
            Order_m = await _orderService.GetOrders();
        }
        public DelegateCommand SignOutCommand => new(() =>
        {

            Global.CurrentUser.UserName = string.Empty;
            Global.CurrentUser.UserSurname = string.Empty;
            Global.CurrentUser.UserPatronymic = string.Empty;
            Global.CurrentUser.UserRole = string.Empty;
            _pageService.ChangePage(new SingInPage());
        });


        public OrderModel SelectedOrder { get; set; }

        public bool IsDialogEditOrderOpen { get; set; } = false;
        public DateTime EditDataOrder { get; set; }
        public int EditStatusOrderIndex { get; set; }

        public DelegateCommand EditOrderCommand => new(() =>
        {
            if (SelectedOrder == null)
                return;
            EditDataOrder = SelectedOrder.OrderDeliveryDate;
            EditStatusOrderIndex = (SelectedOrder.OrderStatusId == 2 ? 2 : 1) + 1;
            IsDialogEditOrderOpen = true;
        });

        public DelegateCommand SaveCurrentOrderCommand => new(async () =>
        {
            var item = Orders.First(i => i.OrderId == SelectedOrder.OrderId);
            var index = Orders.IndexOf(item);
            item.OrderDeliveryDate = EditDataOrder;
            item.OrderStatusId = SelectedOrder.OrderStatusId;

            Orders.RemoveAt(index);
            Orders.Insert(index, item);
            await _orderService.SaveChangesAsync();
            IsDialogEditOrderOpen = false;
        });
    }
}
