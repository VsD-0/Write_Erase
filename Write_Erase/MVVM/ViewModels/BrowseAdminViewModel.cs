using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Erase.MVVM.ViewModels
{
    internal class BrowseAdminViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly OrderService _orderService;

        public List<string> Sorts { get; set; } = new() { "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "Новый", "Завершен" };
        public List<string> OrderFilters { get; set; } = new() { "Новый", "Завершен" };
        public ObservableCollection<OrderModel> Orders { get; set; }
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
        public BrowseAdminViewModel(PageService pageService, OrderService orderService)
        {
            _pageService = pageService;
            _orderService = orderService;
        }
        private async void UpdateProduct()
        {
            var currentOrders = await _orderService.GetOrders();
            MaxRecords = currentOrders.Count;

            #region FilterSortSearch
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
            #endregion FilterSortSearch

            Records = currentOrders.Count;
            Orders = new ObservableCollection<OrderModel>(currentOrders);
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

        public DelegateCommand ProductCommand => new(() => _pageService.ChangePage(new BrowseProductAdmPage()));
    }
}
