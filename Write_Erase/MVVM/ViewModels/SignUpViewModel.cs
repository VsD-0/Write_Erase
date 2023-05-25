using Write_Erase.Services;

namespace Write_Erase.MVVM.ViewModels
{
    public class SignUpViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly UserService _userService;

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Password { get; set; }
        public string Login { get; set; }

        public string ErrorMessage { get; set; }
        private List<string> _userLogin { get; set; } = new();
        public SignUpViewModel(PageService pageService, UserService userService)
        {
            _pageService = pageService;
            _userService = userService;
            Task.Run(async () => _userLogin = await _userService.GetAllLogin());
        }
        public AsyncCommand SignUpCommand => new(async () =>
        {
            await _userService.AddNewUser(Name, Surname, Patronymic, Login, Password);
            _pageService.ChangePage(new SingInPage());
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Login) ||
                string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Surname) ||
                string.IsNullOrWhiteSpace(Patronymic) ||
                string.IsNullOrWhiteSpace(Password))
            return false;
            else return true;
        });
        public DelegateCommand SignInCommand => new(() =>
        {
            _pageService.ChangePage(new SingInPage());
        });
    }
}
