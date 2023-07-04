using Microsoft.Win32;
using System.Reflection;
using Write_Erase.MVVM.Models.Data.Tables;
using Write_Erase.Services;

namespace Write_Erase.MVVM.ViewModels
{
    internal class BrowseProductAdmViewModel : BindableBase
    {
        #region Fields
        private readonly PageService _pageService;
        private readonly ProductService _productService;
        #endregion Fields

        #region Property
        public bool IsCheckedHideProduct { get; set; }
        public string EditDescription { get; set; }
        public decimal EditCost { get; set; }
        public int EditDiscount { get; set; }
        public int EditInStock { get; set; }
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

        public bool IsDialogAddProductOpen { get; set; } = false;
        public string ProductArticle { get; set; }
        public Productname ProductSelectedName { get; set; }
        public string ProductDescription { get; set; }
        public Productcategory ProductSelectedCategories { get; set; }
        public string ProductImage { get; set; }
        public Productmanufacturer ProductSelectedManufacturer { get; set; }
        public Productprovider ProductSelectedProvider { get; set; }
        public string ProductPrice { get; set; }
        public string ProductDiscount { get; set; }
        public string ProductCountInStock { get; set; }

        public ObservableCollection<Productmanufacturer> Pmanufacturers { get; set; }
        public ObservableCollection<Productcategory> Pcategories { get; set; }
        public ObservableCollection<Productprovider> Pproviders { get; set; }
        public ObservableCollection<Productname> Pnames { get; set; }
        #endregion

        public BrowseProductAdmViewModel(PageService pageService, ProductService productService)
        {
            _pageService = pageService;
            _productService = productService;

            Pmanufacturers = new(_productService.GetPmanufacturers());
            Pcategories = new(_productService.GetPcategories());
            Pproviders = new(_productService.GetProdivers());
            Pnames = new(_productService.GetNames());
        }

        #region Command
        public DelegateCommand AddProductCommand => new(() =>
        {
            IsDialogAddProductOpen = true;
        });
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
            EditDescription = SelectedProduct.Description;
            EditCost = SelectedProduct.Price;
            EditDiscount = SelectedProduct.Discount;
            EditInStock = SelectedProduct.InStock;
            IsDialogEditOrderOpen = true;
        });

        public DelegateCommand SaveAddProductCommand => new(async () =>
        {
            if (!Products.Any(p => p.Article == ProductArticle))
            {
                Products.Insert(0, await _productService.AddProductAsync(new Product
                {
                    ParticleNumber = ProductArticle,
                    Pcost = decimal.Parse(ProductPrice),
                    Pdescription = ProductDescription,
                    Pphoto = ProductImage == null ? "" : ProductImage,
                    PnameId = ProductSelectedName.NameId,
                    PcategoryId = ProductSelectedCategories.CategoryId,
                    PmanufacturerId = ProductSelectedManufacturer.ManufacturerId,
                    PproviderId = ProductSelectedProvider.ProviderId,
                    PdiscountAmount = sbyte.Parse(ProductDiscount),
                    PquantityInStock = int.Parse(ProductCountInStock),
                    Pstatus = 0,
                    PunitId = 1
                }));
            }
            IsDialogAddProductOpen = false;
        }, bool () =>
        {
            return !string.IsNullOrWhiteSpace(ProductArticle)
            && ProductSelectedName != null
            && !string.IsNullOrWhiteSpace(ProductDescription)
            && ProductSelectedCategories != null
            && ProductSelectedManufacturer != null
            && ProductSelectedProvider != null
            && !string.IsNullOrWhiteSpace(ProductPrice)
            && !string.IsNullOrWhiteSpace(ProductDiscount)
            && !string.IsNullOrWhiteSpace(ProductCountInStock);
        });

        public DelegateCommand ChoiceImageCommand => new(() =>
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "Выберите изображение",
                Filter = "Изображения (*.jpg, *jpeg, *.png)|*.jpg;*.jpeg;*.png",
                Multiselect = false
            };

            string targetDirectory = Path.Combine("D:\\Projects\\VS\\repos\\Write_Erase\\Write_Erase\\Resources\\Image");

            if (openFileDialog.ShowDialog() == true)
            {
                ProductImage = ProductArticle + ".png";
                if (!File.Exists(Path.Combine(targetDirectory, ProductArticle)))
                {
                    File.Copy(openFileDialog.FileName, Path.Combine(targetDirectory, ProductArticle + ".png"));
                }
            }
            
        });

        public DelegateCommand SaveProductCommand => new(async () =>
        {
            var item = Products.First(i => i.Article == SelectedProduct.Article);
            var index = Products.IndexOf(item);
            item.Status = IsCheckedHideProduct == true ? 1 : 0;
            item.Description = EditDescription;
            item.Price = EditCost;
            item.Discount = EditDiscount;
            item.InStock = EditInStock;
            Products.RemoveAt(index);
            Products.Insert(index, item);
            await _productService.SaveChangesAsync(item);
            IsDialogEditOrderOpen = false;
        });
        #endregion Command
    }
}
