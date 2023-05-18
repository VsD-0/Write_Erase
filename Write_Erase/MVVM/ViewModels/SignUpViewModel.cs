namespace Write_Erase.MVVM.ViewModels
{
    public class SignUpViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public User NewUser { get; set; } = new User();
        public SignUpViewModel(PageService pageService)
        {
            _pageService = pageService;
        }
        public AsyncCommand SignUpCommand => new(async () =>
        {
            //NewUser.UserRole = 1;
            //Global.CurrentUser = new UserModel
            //{
            //    Id = NewUser.UserId,
            //    UserName = NewUser.UserName,
            //    UserSurname = NewUser.UserSurname,
            //    UserPatronymic = NewUser.UserPatronymic,
            //    UserRole = NewUser.UserRole
            //};
            _pageService.ChangePage(new BrowseProductPage());
        });
        public DelegateCommand SignInCommand => new(() =>
        {
            _pageService.ChangePage(new SingInPage());
        });
    }
}
