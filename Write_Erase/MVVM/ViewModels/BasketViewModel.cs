using System.Collections.ObjectModel;

namespace Write_Erase.MVVM.ViewModels
{
    public class BasketViewModel : BindableBase
    {
        #region Fields
        PageService _pageService;
        PointOfIssuesService _pointOfIssuesService;
        ProductService _productService;
        private readonly static Random rnd = new();
        #endregion Fields

        #region Property
        public string FullName { get; set; } = Global.CurrentUser == null ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";

        public ObservableCollection<Basket> ProductsBasket { get; set; } = Global.ProductsBasket;
        public Basket SelectedProductBasket { get; set; }
        public decimal TotalCost { get; set; }
        public List<Orderpickuppoint> CheckoutPoint { get; set; }
        public Orderpickuppoint CheckoutPointSelected { get; set; }
        public decimal PriceNoDiscount { get; set; }
        public decimal Saved { get; set; }
        public int Count { get; set; }
        #endregion Property

        public BasketViewModel(PageService pageService, PointOfIssuesService pointOfIssuesService, ProductService productService)
        {
            _pageService = pageService;
            _pointOfIssuesService = pointOfIssuesService;
            _productService = productService;
            ChangePrice();
            Task.Run(async () => CheckoutPoint = await _pointOfIssuesService.GetPoints());
        }

        #region Commands
        public DelegateCommand BrowseProductCommand => new(() =>
        {
            _pageService.ChangePage(new BrowseProductPage());
        });
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser = null;
            _pageService.ChangePage(new SingInPage());
        });

        private void ChangePrice()
        {
            TotalCost = PriceNoDiscount = Saved = Count = 0;
            foreach (var product in ProductsBasket)
            {
                Count += product.Count;
                TotalCost += (product.Product.Price - (product.Product.Price / 100 * product.Product.Discount)) * product.Count;
                PriceNoDiscount += product.Product.Price * product.Count;
                Saved += (product.Product.Price / 100 * product.Product.Discount) * product.Count;
                Debug.WriteLine(product.Product.Unit);
            }
        }

        //Change Count
        public DelegateCommand DecreaseCount => new(() =>
        {
            if (SelectedProductBasket.Count > 1)
            {
                var item = ProductsBasket.First(f => f.Product.Article.Equals(SelectedProductBasket.Product.Article));
                item.Count--;

                int index = ProductsBasket.IndexOf(item);
                ProductsBasket.RemoveAt(index);
                ProductsBasket.Insert(index, item);
            }
            else
                ProductsBasket.Remove(SelectedProductBasket);

            ChangePrice();
        });
        public DelegateCommand IncreaseCount => new(() =>
        {
            if (SelectedProductBasket.Count < 99)
            {
                var item = ProductsBasket.First(f => f.Product.Article.Equals(SelectedProductBasket.Product.Article));
                item.Count++;

                int index = ProductsBasket.IndexOf(item);
                ProductsBasket.RemoveAt(index);
                ProductsBasket.Insert(index, item);
            }

            TotalCost = 0;
            ChangePrice();
        });
        public AsyncCommand OrderCommand => new(async () =>
        {
            int code = rnd.Next(100, 999);
            Debug.WriteLine("Заказ оформлен");
            await DocumentService.Create(TotalCost, 0, CheckoutPointSelected, await _productService.AddOrder(new Order
            {
                OrderStatusId = 1,
                OrderDeliveryDate = DateTime.Now,
                DateOfOrder = DateTime.Now.AddDays(5),
                OrderPickupPointId = CheckoutPointSelected.PickupPointId,
                FullNameUser = FullName,
                ReceiptCode = code,
            }), code);
        }, bool () => { return CheckoutPointSelected != null && FullName != "Гость"; });
        #endregion Commands
    }
}
