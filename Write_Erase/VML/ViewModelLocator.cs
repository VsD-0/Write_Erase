namespace Write_Erase
{
    internal class ViewModelLocator
    {
        private static ServiceProvider _provider;
        public static IConfiguration Configuration { get; private set; }
        public static void Init()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            var services = new ServiceCollection();

            #region ViewModel

            services.AddTransient<ShellViewModel>();
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<BrowseProductViewModel>();
            services.AddTransient<BasketViewModel>();
            services.AddTransient<BrowseAdminViewModel>();
            services.AddTransient<BrowseProductAdmViewModel>();

            #endregion

            #region Connection

            services.AddDbContext<StoreContext>(options =>
            {
                var conn = Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(conn, ServerVersion.AutoDetect(conn));
            }, ServiceLifetime.Transient);

            #endregion

            #region Services

            services.AddSingleton<PageService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<PointOfIssuesService>();
            services.AddSingleton<OrderService>();

            #endregion

            _provider = services.BuildServiceProvider();
            foreach (var service in services)
            {
                _provider.GetRequiredService(service.ServiceType);
            }
        }
        public ShellViewModel ShellViewModel => _provider.GetRequiredService<ShellViewModel>();
        public SignInViewModel SignInViewModel => _provider.GetRequiredService<SignInViewModel>();
        public SignUpViewModel SignUpViewModel => _provider.GetRequiredService<SignUpViewModel>();
        public BrowseProductViewModel BrowseProductViewModel => _provider.GetRequiredService<BrowseProductViewModel>();
        public BasketViewModel BasketViewModel => _provider.GetRequiredService<BasketViewModel>();
        public BrowseAdminViewModel BrowseAdminViewModel => _provider.GetRequiredService<BrowseAdminViewModel> ();
        public BrowseProductAdmViewModel BrowseProductAdmViewModel => _provider.GetRequiredService<BrowseProductAdmViewModel>();
    }
}
