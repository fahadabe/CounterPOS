

namespace CounterPOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly string appName = "CounterPOS";
        private Forms.NotifyIcon? NotifyIcon;

        private static  IServiceProvider ServiceProvider;
        private IHost? AppHost;
        public App()
        {
            NotifyIcon = new Forms.NotifyIcon();
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IDialogMessages, DialogMessages>();
                    services.AddTransient<IImageHelper, ImageHelper>();
                    services.AddSingleton<IEasyPrinting, EasyPrinting>();

                    services.AddTransient<ICompanyService, CompanyService>();
                    services.AddTransient<ILoginService, LoginService>();
                    services.AddTransient<IUserManagementService, UserManagementService>();
                    services.AddTransient<ICustomerService, CustomerService>();
                    services.AddTransient<IDashboardService, DashboardService>();
                    services.AddTransient<IExpanseService, ExpanseService>();
                    services.AddTransient<INewPurchaseService, NewPurchaseService>();
                    services.AddTransient<IAdditionalChargesService, AdditionalChargesService>();
                    services.AddTransient<IFBRParametersService, FBRParametersService>();
                    services.AddTransient<INewSaleService, NewSaleService>();
                    services.AddTransient<IPaymentTypeService, PaymentTypeService>();
                    services.AddTransient<ICategoryService, CategoryService>();
                    services.AddTransient<IProductService, ProductService>();
                    services.AddTransient<IItemReportService, ItemReportService>();
                    services.AddTransient<IPurchaseReportService, PurchaseReportService>();
                    services.AddTransient<ISaleReportService, SaleReportService>();
                    services.AddTransient<ISupplierService, SupplierService>();


                    services.AddTransient<LoginView>();

                    services.AddTransient<Customers>();
                    services.AddTransient<NewCustomerView>();
                    services.AddTransient<CustomerDetailsView>();
                    services.AddTransient<CustomerViewModel>();

                    services.AddTransient<Dashboard>();
                    services.AddTransient<SaleCardControl>();
                    services.AddTransient<DashboardViewModel>();

                    services.AddTransient<Expanse>();
                    services.AddTransient<NewExpanseView>();
                    services.AddTransient<ExpanseDetailsView>();
                    services.AddTransient<ExpanseCategory>();
                    services.AddTransient<ExpanseCategoryViewModel>();
                    services.AddTransient<ExpanseCategoryViewModel>();

                    services.AddTransient<HomeView>();
                    services.AddTransient<NavigationTab>();

                    services.AddTransient<Products>();
                    services.AddTransient<NewProductView>();
                    services.AddTransient<ProductDetailsView>();
                    services.AddTransient<Category>();
                    services.AddTransient<CategoryViewModel>();
                    services.AddTransient<ProductViewModel>();

                    services.AddTransient<Purchase>();
                    services.AddTransient<PurchaseCartListItemUC>();
                    services.AddTransient<NewPurchaseViewModel>();

                    services.AddTransient<SaleReportView>();
                    services.AddTransient<PurchaseReportView>();
                    services.AddTransient<ItemReportView>();
                    services.AddTransient<SaleReportViewModel>();
                    services.AddTransient<PurchaseReportViewModel>();
                    services.AddTransient<ItemReportViewModel>();

                    services.AddTransient<NewSale>();
                    services.AddTransient<ManageAdditionalChargesView>();
                    services.AddTransient<SimpleProductUC>();
                    services.AddTransient<SaleCartListItemUC>();
                    services.AddTransient<MultiSizeProductUC>();
                    services.AddTransient<CartSectionUC>();
                    services.AddTransient<AdditionalChargesUC>();
                    services.AddTransient<NewSaleViewModel>();

                    services.AddTransient<BusinessInfoView>();
                    services.AddTransient<MiscellaneousView>();
                    services.AddTransient<UserManagementView>();
                    services.AddTransient<UpdateUserWindow>();
                    services.AddTransient<BusinessManagementViewModel>();
                    services.AddTransient<MiscellaneousViewModel>();
                    services.AddTransient<UserManagementViewModel>();
                    services.AddTransient<SettingsView>();

                    services.AddTransient<Suppliers>();
                    services.AddTransient<NewSupplierView>();
                    services.AddTransient<SupplierDetailsView>();
                    services.AddTransient<SupplierViewModel>();

                    services.AddTransient<InvoiceViewer>();
                    services.AddTransient<MainWindow>();
                    services.AddSingleton<MainViewModel>();

                    services.AddTransient<FBRParametersWindow>();
                    services.AddTransient<FBRParametersViewModel>();
                    services.AddTransient<FirstTimeSettingUpWindow>();
                    services.AddTransient<PaymentTypeWindow>();
                    services.AddTransient<PaymentTypeViewModel>();



                }).Build();
            ServiceProvider = AppHost.Services;
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                System.Windows.Forms.MessageBox.Show($"{appName} is already running.", "Info", Forms.MessageBoxButtons.OK, Forms.MessageBoxIcon.Information);
                Current.Shutdown();
                return;
            }

            NotifyIcon.Icon = new Icon("Resources/counterpos.ico");
            NotifyIcon.Text = appName;
            NotifyIcon.Visible = true;
            NotifyIcon.Click += nIcon_Click;

            await AppHost!.StartAsync();

            var startupWindow = GetService<MainWindow>();
            startupWindow.Show();

            base.OnStartup(e);  
        }

        private void nIcon_Click(object sender, EventArgs e)
        {
            MainWindow.Visibility = Visibility.Visible;
            MainWindow.WindowState = WindowState.Normal;
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            NotifyIcon.Dispose();
            await AppHost.StopAsync();
            AppHost.Dispose();
            base.OnExit(e);
        }
        
    }
}