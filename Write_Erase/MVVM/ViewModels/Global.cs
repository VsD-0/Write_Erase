using System.Collections.ObjectModel;

namespace Write_Erase.MVVM.Models
{
    public static class Global
    {
        public static UserModel? CurrentUser { get; set; }
        public static ObservableCollection<Basket> ProductsBasket { get; set; } = new ObservableCollection<Basket>();
    }
}
