using Write_Erase.Services;

namespace Write_Erase.MVVM.ViewModels
{
    public class BrowseProductAdmViewModel : BindableBase
    {
        #region Fields
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        #endregion Fields

        #region Property
        public bool IsCheckedHideProduct { get; set; }
        public bool IsDialogEditOrderOpen { get; set; } = false;
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

        public BrowseProductAdmViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;
        }

        #region Command
        async void ChangeList()
        {
            List<ProductModel> actualProduct = await _productService.GetProducts();
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

        public DelegateCommand OrderCommand => new(() => _pageService.ChangePage(new BrowseAdminPage()));
        public DelegateCommand SignOutCommand => new(() =>
        {
            Global.CurrentUser = null;
            _pageService.ChangePage(new SingInPage());
        });

        public DelegateCommand EditProduct => new(() =>
        {
            IsCheckedHideProduct = SelectedProduct.Status == 1 ? true : false;
            IsDialogEditOrderOpen = true;
        });

        public DelegateCommand SaveProductCommand => new(async () =>
        {
            var item = Products.First(i => i.Article == SelectedProduct.Article);
            var index = Products.IndexOf(item);
            item.Status = IsCheckedHideProduct == true ? 1 : 0;

            Products.RemoveAt(index);
            Products.Insert(index, item);
            await _productService.SaveChangesAsync();
            IsDialogEditOrderOpen = false;
        });
        #endregion Command
    }
}
