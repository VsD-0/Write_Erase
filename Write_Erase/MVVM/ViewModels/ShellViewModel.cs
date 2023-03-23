namespace Write_Erase.MVVM.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly PageService _pageService;
        public Page? PageSource { get; set; }
        public ShellViewModel(PageService pageService)
        {
            _pageService = pageService;

            _pageService.onPageChanged += (page) => PageSource = page;

            _pageService.ChangePage(new SingInPage());
        }
    }
}
