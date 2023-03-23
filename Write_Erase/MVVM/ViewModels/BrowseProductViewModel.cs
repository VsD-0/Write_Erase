namespace Write_Erase.MVVM.ViewModels
{
    public class BrowseProductViewModel : BindableBase
    {
        #region Fields
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        #endregion Fields

        #region Property
        public List<string> Sorts { get; set; } = new() { "По умолчанию", "По возрастанию", "По убыванию" };
        public List<string> Filters { get; set; } = new() { "Все диапазоны", "0-5%", "5-9%", "9% и более" };
        public List<ProductModel> Products { get; set; }
        public string FullName { get; set; } = Global.CurrentUser == null ? "Гость" : $"{Global.CurrentUser.UserSurname} {Global.CurrentUser.UserName} {Global.CurrentUser.UserPatronymic}";
        public string SelectedSort
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: ChangeList); }
        }
        public string SelectedFilter
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: ChangeList); }
        }
        public string Search
        {
            get { return GetValue<string>(); }
            set { SetValue(value, changedCallback: ChangeList); }
        }
        public ProductModel SelectedProduct { get; set; }
        public int? MaxRecords { get; set; } = 0;
        public int? FoundRecords { get; set; } = 0;
        #endregion

        public BrowseProductViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
        }

        #region Command
        async void ChangeList()
        {
            var actualProduct = await _productService.GetProducts();
            MaxRecords = actualProduct.Count;

            if (!string.IsNullOrEmpty(Search))
                actualProduct = actualProduct.Where(p => p.Title.ToLower().Contains(Search.ToLower())).ToList();
            if (!string.IsNullOrEmpty(SelectedFilter))
            {
                switch (SelectedFilter)
                {
                    case "Все диапазоны":
                        break;
                    case "0-5%":
                        actualProduct = actualProduct.Where(p => p.Discount >= 0 && p.Discount < 5).ToList();
                        break;
                    case "5-9%":
                        actualProduct = actualProduct.Where(p => p.Discount >= 5 && p.Discount < 9).ToList();
                        break;
                    case "9% и более":
                        actualProduct = actualProduct.Where(p => p.Discount >= 9).ToList();
                        break;
                }
            }
            if (!string.IsNullOrEmpty(SelectedSort))
            {
                switch (SelectedSort)
                {
                    case "По умолчанию":
                        break;
                    case "По возрастанию":
                        actualProduct = actualProduct.OrderBy(p => p.Price).ToList();
                        break;
                    case "По убыванию":
                        actualProduct = actualProduct.OrderByDescending(p => p.Price).ToList();
                        break;
                }
            }

            FoundRecords = actualProduct.Count;
            Products = actualProduct;
        }

        public DelegateCommand BasketCommand => new(() => _pageService.ChangePage(new BasketPage()));
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser = null;
            _pageService.ChangePage(new SingInPage());
        });
        public DelegateCommand AddProduct => new(() =>
        {
            ProductModel p = Products.Where(c => c.Article == SelectedProduct.Article).First();
            if (Global.ProductsBasket.Where(c => c.Product.Article == SelectedProduct.Article).Count() == 0)
            {
                Basket basket = new Basket();
                basket.Product = p;
                basket.Count = 1;
                Global.ProductsBasket.Add(basket);
            }
            else
            {
                Basket bp = Global.ProductsBasket.Where(c => c.Product.Article == SelectedProduct.Article).First();
                bp.Count++;
            }
        });
        #endregion Command
    }
}
