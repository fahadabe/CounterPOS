

namespace CounterPOS.Model;

public partial class NavigationTab : ObservableObject
{
   

    #region Properties

    [ObservableProperty]
    private object _Content;

    [ObservableProperty]
    private string _Header = "";

    [ObservableProperty]
    private ObservableCollection<UniversalSerachModel> _MasterSearchItemList = new();
    [ObservableProperty]
    private string _UniversalSearchTerm = "";
    
    public MainViewModel parent { set; get; }



    #endregion Properties

    #region Command Functions

    [RelayCommand]
    private void LoadData()
    {
        PopulateUniversalSearchItemList();
       
    }
    [RelayCommand]
    private void UniversalSearchItem(string str)
    {
        switch (str)
        {
            case "Dashboard":
                OpenDashboard();
                break;

            case "NewSale":
                OpenNewSale();
                break;

            case "NewPurchase":
                OpenNewPurchase();
                break;

            case "Products":
                OpenProducts();
                break;

            case "Customers":
                OpenCustomers();
                break;

            case "Suppliers":
                OpenSuppliers();
                break;

            case "Expanses":
                OpenExpanses();
                break;

            case "Reports":
                OpenReports();
                break;

            case "AddCategory":
                OpenAddCategory();
                break;

            case "AddExpanseCategory":
                OpenAddExpanseCategory();
                break;

            case "AddPaymentType":
                OpenAddPaymentType();
                break;

            case "OpenFBRParameters":
                OpenFBRParametersSetting();
                break;

            default:
                break;
        }
    }

    #endregion Command Functions

    #region Private Functions

   

    private void NavigateToAsync(string header, object vm)
    {
        foreach (var item in parent.Tabs)
        {
            if (item.Header == header)
            {
                parent.SelectedTab = item;
                return;
            }
        }

        NavigationTab tab = new()
        {
            Header = header,
            Content = vm
        };

        parent.Tabs.Remove(parent.SelectedTab);
        parent.Tabs.Add(tab);
        parent.SelectedTab = tab;
    }

    private void OpenAddCategory()
    {
        var categoryViewModel = App.GetService<CategoryViewModel>();
        Category objC = new Category(categoryViewModel);
        objC.ShowDialog();
    }

  

    private void OpenAddExpanseCategory()
    {
        var expanseCategoryViewModel = App.GetService<ExpanseCategoryViewModel>();
        ExpanseCategory obj = new(expanseCategoryViewModel);
        obj.ShowDialog();
    }

    private void OpenAddPaymentType()
    {
        var paymentTypeViewModel = App.GetService<PaymentTypeViewModel>();
        PaymentTypeWindow obj = new(paymentTypeViewModel);
        obj.ShowDialog();
    }

    private void OpenCustomers()
    {
        var customerService = App.GetService<ICustomerService>();
        var newSaleService = App.GetService<INewSaleService>();
        var saleReportService = App.GetService<ISaleReportService>();
        var messages = App.GetService<IDialogMessages>();
        var easyPrinting = App.GetService<IEasyPrinting>();

        NavigateToAsync("Customers", new CustomerViewModel(customerService, newSaleService, saleReportService, messages, easyPrinting));
    }

    private void OpenDashboard()
    {
        var paymetTypeService = App.GetService<IPaymentTypeService>();
        var dashboardService = App.GetService<IDashboardService>();
        NavigateToAsync("Dashboard", new DashboardViewModel(paymetTypeService, dashboardService));
    }

    private void OpenExpanses()
    {
        var expanseService = App.GetService<IExpanseService>();
        var messages = App.GetService<IDialogMessages>();
        NavigateToAsync("Expanses", new ExpanseViewModel(expanseService, messages));
    }

    private void OpenFBRParametersSetting()
    {
        var fBRParametersViewModel = App.GetService<FBRParametersViewModel>();
        FBRParametersWindow obj = new(fBRParametersViewModel);
        obj.ShowDialog();
    }

    private void OpenNewPurchase()
    {
        var purchaseReportService = App.GetService<IPurchaseReportService>();
        var paymentTypeService = App.GetService<IPaymentTypeService>();
        var usermangementService = App.GetService<IUserManagementService>();
        var newSaleService = App.GetService<INewSaleService>();
        var newPurchaseService = App.GetService<INewPurchaseService>();
        var supplierService = App.GetService<ISupplierService>();

        var messages = App.GetService<IDialogMessages>();
        var easyPrinting = App.GetService<IEasyPrinting>();

        NavigateToAsync("Purchase", new NewPurchaseViewModel(purchaseReportService, paymentTypeService, usermangementService, newSaleService, newPurchaseService, supplierService, messages, easyPrinting));
    }

    private void OpenNewSale()
    {
        var saleReportService = App.GetService<ISaleReportService>();
        var paymentTypeService = App.GetService<IPaymentTypeService>();
        var usermangementService = App.GetService<IUserManagementService>();
        var newSaleService = App.GetService<INewSaleService>();
        var newPurchaseService = App.GetService<INewPurchaseService>();
        var additionalChargesService = App.GetService<IAdditionalChargesService>();
        var customerService = App.GetService<ICustomerService>();
        var fbrparametersService = App.GetService<IFBRParametersService>();
        var dashboardService = App.GetService<IDashboardService>();

        var messages = App.GetService<IDialogMessages>();
        var easyPrinting = App.GetService<IEasyPrinting>();

        NavigateToAsync("Sale", new NewSaleViewModel(saleReportService, paymentTypeService, usermangementService, newSaleService, newPurchaseService, additionalChargesService, customerService, fbrparametersService, dashboardService, messages, easyPrinting));
    }

    private void OpenProducts()
    {
        var productService = App.GetService<IProductService>();
        var categoryService = App.GetService<ICategoryService>();
        var saleReportService = App.GetService<ISaleReportService>();
        var customerService = App.GetService<ICustomerService>();
        var messages = App.GetService<IDialogMessages>();
        var easyPrinting = App.GetService<IEasyPrinting>();
        var imageHelper = App.GetService<IImageHelper>();

        NavigateToAsync("Products", new ProductViewModel(productService, categoryService, saleReportService, customerService, messages, easyPrinting, imageHelper));
    }

    private void OpenReports()
    {
        NavigateToAsync("Reports", new ReportViewModel());
    }

    private void OpenSuppliers()
    {
        var supplierService = App.GetService<ISupplierService>();
        var messages = App.GetService<IDialogMessages>();
        var easyPrinting = App.GetService<IEasyPrinting>();
        NavigateToAsync("Suppliers", new SupplierViewModel(supplierService, messages, easyPrinting));
    }

    private void PopulateUniversalSearchItemList()
    {
        MasterSearchItemList.Clear();
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Dashboard", Tag = "Dashboard" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Sale", Tag = "NewSale" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Purchase", Tag = "NewPurchase" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Products", Tag = "Products" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Customers", Tag = "Customers" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Suppliers", Tag = "Suppliers" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Expanses", Tag = "Expanses" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Reports", Tag = "Reports" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Product Category", Tag = "AddCategory" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Expanse Category", Tag = "AddExpanseCategory" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "Payment Type", Tag = "AddPaymentType" });
        MasterSearchItemList.Add(new UniversalSerachModel { SearchItem = "FBR Parameters", Tag = "OpenFBRParameters" });

        
    }

    #endregion Private Functions
}