using System.Windows;

namespace Write_Erase.MVVM.ViewModels
{
    public class SignInViewModel : BindableBase
    {
        private readonly UserService _userService;
        private readonly PageService _pageService;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorMessageButton { get; set; }
        public SignInViewModel(UserService userService, PageService pageService)
        {
            _userService = userService;
            _pageService = pageService;
        }
        public AsyncCommand SignInCommand => new(async () =>
        {
            await Task.Run(async () =>
            {
                if (await _userService.AuthorizationAsync(Username, Password))
                {
                    ErrorMessageButton = string.Empty;
                    await Application.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        if (Global.CurrentUser.UserRole == "Клиент")
                            _pageService.ChangePage(new BrowseProductPage());
                        else
                            _pageService.ChangePage(new BrowseAdminPage());
                    });
                }
                else
                    ErrorMessageButton = "Неверный логин или пароль";
            });
        }, bool () =>
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Пустые поля";
                ErrorMessageButton = string.Empty;
            }
            else
                ErrorMessage = string.Empty;

            return ErrorMessage == string.Empty;
        });
        public DelegateCommand SignUpCommand => new(() => _pageService.ChangePage(new SignUpPage()));
        public DelegateCommand SignInLaterCommand => new(() => _pageService.ChangePage(new BrowseProductPage()));
        // TODO: Удалить кнопку быстрого входа для админинистрации
        public DelegateCommand SignInAdminCommand => new(() => _pageService.ChangePage(new BrowseAdminPage()));
    }
}
