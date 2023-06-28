namespace Write_Erase.Services
{
    internal class PageService
    {
        public event Action<Page>? onPageChanged;
        public void ChangePage(Page page) => onPageChanged?.Invoke(page);
    }
}
