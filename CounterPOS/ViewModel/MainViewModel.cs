namespace CounterPOS.ViewModel;

public partial class MainViewModel : ObservableObject
{
    private readonly ICompanyService _companyService;
    private readonly ILoginService _loginService;
    private readonly IImageHelper _imageHelper;
    private readonly IDialogMessages _messages;

    public MainViewModel(ICompanyService companyService, ILoginService loginService, IImageHelper imageHelper, IDialogMessages messages)
    {
        _companyService = companyService;
        _loginService = loginService;
        _imageHelper = imageHelper;
        _messages = messages;

        var color = Settings.Default.AccentBrush;
        SelectedColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color);
    }

    #region Main

    #region Commands

    public DelegateCommand LoadDataCommand => new DelegateCommand(ExecuteLoadData);

    public DelegateCommand<string> UniversalSearchItemCommand => new DelegateCommand<string>(ExecuteUniversalSearchItem);
    public DelegateCommand ChangeApplicationAscentCommand => new DelegateCommand(ExecuteChangeApplicationAscent);

    public DelegateCommand CloseSelectedTabCommand => new DelegateCommand(ExecuteCloseSelectedTab);

    public DelegateCommand<Window> LogoutCommand => new DelegateCommand<Window>(ExecuteLogout);
    public DelegateCommand NewTabCommand => new DelegateCommand(ExecuteNewTab);
    public DelegateCommand ToggleCheckCommand => new DelegateCommand(ExecuteToggleCheck);

    public DelegateCommand OpenSettingsViewCommand => new DelegateCommand(ExecuteOpenSettingsView);

    public DelegateCommand ShowMoreOptionsPopupCommand => new DelegateCommand(ExecuteShowMoreOptionsPopup);

    #endregion Commands

    #region Properties

    [ObservableProperty]
    private CompanyModel _CompanyDetails = new();

    [ObservableProperty]
    private UserModel _CurrentUser = new();

    [ObservableProperty]
    private bool _IsAccentPopupToggleChecked = false;

    [ObservableProperty]
    private NavigationTab _SelectedTab = new();

    [ObservableProperty]
    private ObservableCollection<NavigationTab> _Tabs = new();

    [ObservableProperty]
    private bool _IsMoreOptionsPopupOpened;

    [ObservableProperty]
    private bool _IsUserLoggedIn = false;

    [ObservableProperty]
    private System.Windows.Media.Color _SelectedColor;

    partial void OnSelectedTabChanged(NavigationTab value)
    {
        if (SelectedTab is not null)
        {
            SelectedTabHeader = SelectedTab.Header;
        }
    }

    #endregion Properties

    #region Command Functions

    private void ExecuteLoadData()
    {
        LoginLoadData();
        if (!IsUserLoggedIn)
        {
            UserIsLoggedOut();
        }
        else
        {
            UserHasLoggedIn();
        }

        //ExecuteNewTab();
    }

    private void UserIsLoggedOut()
    {
        NavigationTab login = new()
        {
            Header = "Login",
            Content = this,
            parent = this
        };
        Tabs.Add(login);
        SelectedTab = login;
    }

    private void UserHasLoggedIn()
    {
        var loginTab = Tabs.Where(i => i.Header == "Login").FirstOrDefault();

        if (loginTab is null)
        {
            return;
        }

        Tabs.Remove(loginTab);
        NavigationTab Home = new()
        {
            Header = "Home",
            Content = "",
            parent = this
        };
        Tabs.Add(Home);
        SelectedTab = Home;
        CompanyDetails = CommonProperties.CompanyDetails;
        CurrentUser = CommonProperties.CurrentUser;
        PopulateUniversalSearchItemList();
    }

    private void ExecuteChangeApplicationAscent()
    {
        System.Windows.Media.Color color = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(SelectedColor.ToString());
        Accent.Apply(color);

        var converter = new System.Windows.Media.BrushConverter();
        var brush = (System.Windows.Media.Brush)converter.ConvertFromString(SelectedColor.ToString());
        ThemeManager.Current.AccentColor = brush;

        Settings.Default.AccentBrush = SelectedColor.ToString();
        Settings.Default.Save();
    }

    private void ExecuteCloseSelectedTab()
    {
        if (SelectedTab is not null)
        {
            Tabs.Remove(SelectedTab);
        }
    }

    private void ExecuteLogout(Window window)
    {
        IsUserLoggedIn = false;
        Tabs.Clear();
        UserIsLoggedOut();
    }

    private void ExecuteToggleCheck()
    {
        IsAccentPopupToggleChecked = true;
    }

    #endregion Command Functions

    #region Private Functions

    private void ExecuteNewTab()
    {
        IsMoreOptionsPopupOpened = true;
    }

    #endregion Private Functions

    #region Universal Search

    [ObservableProperty]
    private object _Content;

    [ObservableProperty]
    private ObservableCollection<UniversalSerachModel> _SearchItemList = new();

    [ObservableProperty]
    private string _Header;

    [ObservableProperty]
    private string _SelectedTabHeader = string.Empty;

    private void PopulateUniversalSearchItemList()
    {
        SearchItemList.Clear();
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Dashboard", Tag = "Dashboard" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Sale", Tag = "NewSale" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Purchase", Tag = "NewPurchase" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Products", Tag = "Products" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Customers", Tag = "Customers" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Suppliers", Tag = "Suppliers" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Expanses", Tag = "Expanses" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Reports", Tag = "Reports" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Product Category", Tag = "AddCategory" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Expanse Category", Tag = "AddExpanseCategory" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "Payment Type", Tag = "AddPaymentType" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "FBR Parameters", Tag = "OpenFBRParameters" });
        SearchItemList.Add(new UniversalSerachModel { SearchItem = "New Tab", Tag = "NewTab" });
    }

    private void NavigateToAsync(string header, object vm)
    {
        if (string.Equals(SelectedTab.Header, "Home"))
        {
            NavigationTab tab1 = new()
            {
                Header = header,
                Content = vm
            };
            Tabs.Remove(SelectedTab);
            Tabs.Add(tab1);
            SelectedTab = tab1;

            return;
        }

        if (Tabs.Any(i => i.Header == header))
        {
            SelectedTab = Tabs.FirstOrDefault(i => i.Header == header);
            return;
        }

        NavigationTab tab = new()
        {
            Header = header,
            Content = vm
        };

        Tabs.Add(tab);
        SelectedTab = tab;
    }

    private void AddNewTab()
    {
        foreach (var item in Tabs)
        {
            if (item.Header == "Home")
            {
                SelectedTab = item;
                return;
            }
        }

        NavigationTab tab = new()
        {
            parent = this,
            Header = "Home",
            Content = ""
        };

        Tabs.Add(tab);
        SelectedTab = tab;
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

    private void ExecuteOpenSettingsView()
    {
        SettingsWindow settingsWindow = new();
        settingsWindow.ShowDialog();
    }

    private void OpenSuppliers()
    {
        var supplierService = App.GetService<ISupplierService>();
        var messages = App.GetService<IDialogMessages>();
        var easyPrinting = App.GetService<IEasyPrinting>();
        NavigateToAsync("Suppliers", new SupplierViewModel(supplierService, messages, easyPrinting));
    }

    private void ExecuteUniversalSearchItem(string str)
    {
        IsMoreOptionsPopupOpened = false;
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

            case "NewTab":
                AddNewTab();
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

    private void ExecuteShowMoreOptionsPopup()
    {
        IsMoreOptionsPopupOpened = true;
    }

    #endregion Universal Search

    #endregion Main

    #region Login

    #region Global Variables

    private readonly string TableSchemaCommand =
            "SELECT name FROM PRAGMA_TABLE_INFO('tblAdditionalCharges')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblCategory')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblCompany')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblCustomers')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblExpanseCategory')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblExpanses')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblFBRParameters')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblFBRResponse')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblPaymentType')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblProductSize')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblProducts')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblPurchaseTransactions')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblPurchaseTransactionsDetails')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblSaleTransactions')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblSaleTransactionsDetails')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblSuppliers')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblUser')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblDeletedSales')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblDeletedSaleDetails')" +
            "UNION ALL " +
            "SELECT name FROM PRAGMA_TABLE_INFO('tblDeletedFBRResponse')"
    ;

    private string CreateTablesCommand =

        "CREATE TABLE tblAdditionalCharges (ChargeID INTEGER NOT NULL UNIQUE, ChargeType TEXT, ChargeAmount NUMERIC DEFAULT 0," +
        "PRIMARY KEY(ChargeID AUTOINCREMENT));" +

        "CREATE TABLE tblCategory(CategoryID INTEGER NOT NULL UNIQUE, Category TEXT, PRIMARY KEY(CategoryID AUTOINCREMENT));" +

        "CREATE TABLE tblCompany(ID INTEGER NOT NULL UNIQUE, Name TEXT, Country TEXT, StartYear TEXT, Address1 TEXT, Address2 TEXT," +
        "Email TEXT, Phone TEXT, Logo BLOB, PrintMessage1 TEXT, PrintMessage2 TEXT, CurrencySymbol TEXT, PRIMARY KEY(ID AUTOINCREMENT));" +

        "CREATE TABLE tblCustomers(ID INTEGER NOT NULL UNIQUE, Name TEXT, Phone TEXT, NTN Text, CNIC Text, Address TEXT, EmailAddress TEXT, " +
        "AddedDate TEXT, Remarks TEXT, BlackList INTEGER DEFAULT 0, PRIMARY KEY(ID AUTOINCREMENT)); " +

        "CREATE TABLE tblExpanseCategory(CategoryID INTEGER NOT NULL UNIQUE, Category TEXT, PRIMARY KEY(CategoryID AUTOINCREMENT));" +

        "CREATE TABLE tblExpanses(ExpansesID INTEGER NOT NULL UNIQUE, ExpansesDescription TEXT, Amount NUMERIC DEFAULT 0, ExpanseDate TEXT," +
        "ExpanseCategory TEXT, Note TEXT, PRIMARY KEY(ExpansesID AUTOINCREMENT));" +

        "CREATE TABLE tblFBRParameters(POSID TEXT);" +

        "CREATE TABLE tblFBRResponse(TransactionID INTEGER, InvoiceNumber TEXT, Code TEXT, Response TEXT, Errors TEXT);" +

        "CREATE TABLE tblPaymentType(PaymentType TEXT NOT NULL UNIQUE, PRIMARY KEY(PaymentType));" +

        "CREATE TABLE tblProductSize(SizeID INTEGER NOT NULL UNIQUE, ProductID INTEGER," +
        "ProductSize TEXT, Discount NUMERIC DEFAULT 0, Tax NUMERIC DEFAULT 0, PurchaseQuantity INTEGER DEFAULT 0, PurchasePrice NUMERIC DEFAULT 0, SalePrice NUMERIC DEFAULT 0, Identifier TEXT," +
        "PCTCode TEXT, Category TEXT, TimesSold INTEGER DEFAULT 0, PRIMARY KEY(SizeID AUTOINCREMENT)); " +

        "CREATE TABLE tblProducts(ID INTEGER NOT NULL UNIQUE, Name TEXT, Description TEXT, Identifier TEXT, PCTCode TEXT," +
        "AddedDate TEXT, PurchasePrice NUMERIC DEFAULT 0, SalePrice NUMERIC DEFAULT 0, Category TEXT, PurchaseQuantity INTEGER DEFAULT 0, Picture BLOB, TimesSold INTEGER DEFAULT 0," +
        "IsVariantProduct INTEGER, IsAvailable INTEGER, Discount NUMERIC DEFAULT 0, Tax NUMERIC DEFAULT 0, PRIMARY KEY(ID AUTOINCREMENT));" +

        "CREATE TABLE tblPurchaseTransactions(PurchaseID INTEGER NOT NULL UNIQUE, PurchaseDate TEXT, SupplierID INTEGER, SupplierName TEXT, " +
        "PurchaseBY TEXT, PaymentType TEXT, DiscountPercent NUMERIC DEFAULT 0, DiscountAmount NUMERIC DEFAULT 0, TaxPercent NUMERIC DEFAULT 0, TaxAmount NUMERIC DEFAULT 0, SubTotal NUMERIC DEFAULT 0," +
        "GrandTotal NUMERIC DEFAULT 0, PurchaseNote TEXT, DiscountOnTotal INTEGER, TransactionType TEXT, PRIMARY KEY(PurchaseID AUTOINCREMENT));" +

        "CREATE TABLE tblPurchaseTransactionsDetails(PurchaseID INTEGER, ProductName TEXT, PurchasePrice NUMERIC DEFAULT 0, SalePrice NUMERIC DEFAULT 0, Qty INTEGER," +
        "Total NUMERIC DEFAULT 0, DiscountPercent NUMERIC DEFAULT 0, DiscountAmount NUMERIC DEFAULT 0, TaxPercent NUMERIC DEFAULT 0, TaxAmount NUMERIC DEFAULT 0, PRIMARY KEY(PurchaseID AUTOINCREMENT));" +

        "CREATE TABLE tblSaleTransactions(TransactionID INTEGER NOT NULL UNIQUE, CustomerID INTEGER, Narration TEXT, InvoiceDate TEXT," +
        "InvoiceTime TEXT, SubTotal NUMERIC DEFAULT 0, DiscountValue NUMERIC DEFAULT 0, DiscountPrice NUMERIC DEFAULT 0, GSTValue NUMERIC DEFAULT 0, GSTPrice NUMERIC DEFAULT 0," +
        " AdditionalCharges NUMERIC DEFAULT 0, GrandTotal NUMERIC DEFAULT 0, Cash NUMERIC DEFAULT 0," +
        "Change NUMERIC, PaymentType TEXT, IsPaid INTEGER DEFAULT 0, Cashier TEXT, IsFBRInvoice INTEGER, DiscountOnTotal INTEGER, TransactionType TEXT," +
        "PRIMARY KEY(TransactionID AUTOINCREMENT));" +

        "CREATE TABLE tblSaleTransactionsDetails(TransactionDetailsID INTEGER NOT NULL UNIQUE, TransactionID INTEGER, Name TEXT, Qty INTEGER," +
        "Price NUMERIC DEFAULT 0, Discount NUMERIC DEFAULT 0, Tax NUMERIC DEFAULT 0, Total NUMERIC DEFAULT 0, IsVariantProduct INTEGER DEFAULT 0, ProductID INTEGER, PRIMARY KEY(TransactionDetailsID AUTOINCREMENT));" +

        "CREATE TABLE tblSuppliers(SupplierID INTEGER NOT NULL UNIQUE, SupplierName TEXT, Email TEXT, Phone TEXT, Location TEXT," +
        "Website TEXT, Note TEXT, AddedDate TEXT, PRIMARY KEY(SupplierID AUTOINCREMENT));" +

        "CREATE TABLE tblUser(UserID INTEGER NOT NULL UNIQUE, Username TEXT, Password TEXT, AddAccount INTEGER DEFAULT 0, EditAccount INTEGER DEFAULT 0," +
        "DeleteAccount INTEGER DEFAULT 0, AddInventory INTEGER DEFAULT 0, EditInventory INTEGER DEFAULT 0, DeleteInventory INTEGER DEFAULT 0, CreateInvoice INTEGER DEFAULT 0," +
        "DeleteInvoice INTEGER DEFAULT 0, AddExpanse INTEGER DEFAULT 0, EditExpanse INTEGER DEFAULT 0, DeleteExpanse INTEGER DEFAULT 0, InteractDashboard INTEGER DEFAULT 0," +
        "ViewReports INTEGER DEFAULT 0, EditSettings INTEGER DEFAULT 0, PRIMARY KEY(UserID AUTOINCREMENT));" +

        "CREATE TABLE tblDeletedSales(TransactionID INTEGER NOT NULL UNIQUE, CustomerID INTEGER, Narration TEXT, InvoiceDate TEXT, InvoiceTime TEXT," +
        "SubTotal NUMERIC DEFAULT 0, DiscountValue NUMERIC DEFAULT 0, DiscountPrice NUMERIC DEFAULT 0, GSTValue NUMERIC DEFAULT 0, GSTPrice NUMERIC DEFAULT 0," +
        "AdditionalCharges NUMERIC DEFAULT 0, GrandTotal NUMERIC DEFAULT 0, Cash NUMERIC DEFAULT 0, Change NUMERIC, PaymentType TEXT, IsPaid INTEGER DEFAULT 0," +
        "Cashier TEXT, IsFBRInvoice INTEGER, DiscountOnTotal INTEGER, TransactionType TEXT, PRIMARY KEY(TransactionID AUTOINCREMENT));" +

        "CREATE TABLE tblDeletedSaleDetails(TransactionDetailsID INTEGER NOT NULL UNIQUE, TransactionID INTEGER, Name TEXT, Qty INTEGER,Price NUMERIC DEFAULT 0," +
        "Discount NUMERIC DEFAULT 0, Tax NUMERIC DEFAULT 0, Total NUMERIC DEFAULT 0, IsVariantProduct INTEGER DEFAULT 0, ProductID INTEGER, PRIMARY KEY(TransactionDetailsID AUTOINCREMENT));" +

        "CREATE TABLE tblDeletedFBRResponse(TransactionID INTEGER, InvoiceNumber TEXT, Code TEXT, Response TEXT, Errors TEXT)";

    private List<string> DatabaseTableSchema;
    private string? newConnectionStng;

    #endregion Global Variables

    #region Commands

    public DelegateCommand BrowseCompanyLogoCommand => new DelegateCommand(ExecuteBrowseCompanyLogo);
    public DelegateCommand<Window> CloseWindowCommand => new DelegateCommand<Window>(ExecuteCloseWindow);
    public DelegateCommand CreateNewDataBaseCommand => new DelegateCommand(ExecuteCreateNewDatabase);
    public DelegateCommand<Window> InserInitialDataCommand => new DelegateCommand<Window>(ExecuteInserInitialData);
    public DelegateCommand<Window> LoginCommand => new DelegateCommand<Window>(ExecuteLogin);
    public DelegateCommand RemoveCompanyLogoCommand => new DelegateCommand(ExecuteRemoveCompanyLogo);
    public DelegateCommand SelectDatabaseCommand => new DelegateCommand(ExecuteSelectDatabase);

    #endregion Commands

    #region Properties

    [ObservableProperty]
    private bool _CanLogin;

    [ObservableProperty]
    private CompanyModel _NewCompanyDetails = new();

    [ObservableProperty]
    private ObservableCollection<string> _CountryList;

    [ObservableProperty]
    private bool _CreateNewDatabase;

    [ObservableProperty]
    private FBRParametersModel _FBRParameters = new();

    [ObservableProperty]
    private string _NewLogo = string.Empty;

    [ObservableProperty]
    private UserModel _NewUser = new();

    [ObservableProperty]
    private string _Password;

    [ObservableProperty]
    private string _PaymentType;

    [ObservableProperty]
    private string _Username = Settings.Default.LastUsedUsername;

    #endregion Properties

    #region Command Functions

    private void ExecuteBrowseCompanyLogo()
    {
        var path = _imageHelper.BrowseImage();
        if (!string.IsNullOrWhiteSpace(path))
        {
            NewLogo = path;
        }
    }

    private void ExecuteCloseWindow(Window obj)
    {
        if (obj is not null)
        {
            obj.Close();
            Application.Current.Shutdown();
        }
    }

    private void ExecuteCreateNewDatabase()
    {
        SaveFileDialog saveFileDialog = new SaveFileDialog();
        try
        {
            saveFileDialog.Title = "Save Database File";
            saveFileDialog.Filter = "Database files (*.db, *.sqlite, *.sqlite3) | *.db; *.sqlite; *.sqlite3";
            if (saveFileDialog.ShowDialog() == true)
            {
                SQLiteConnection.CreateFile(saveFileDialog.FileName);
                newConnectionStng = string.Format(format: @"Data Source={0};Version=3;", saveFileDialog.FileName);

                if (CreateTables(newConnectionStng))
                {
                    UpdateConnectionString(newConnectionStng);
                    CountryList = GetCountryList();
                    FirstTimeSettingUpWindow obj = new()
                    {
                        DataContext = this
                    };
                    obj.ShowDialog();
                }
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    private void ExecuteInserInitialData(Window window)
    {
        if (!string.IsNullOrWhiteSpace(NewCompanyDetails.Name) && !string.IsNullOrWhiteSpace(NewCompanyDetails.Address1) && !string.IsNullOrWhiteSpace(NewCompanyDetails.Phone)
            && !string.IsNullOrWhiteSpace(NewUser.Username) && !string.IsNullOrWhiteSpace(NewUser.Password))
        {
            if (_loginService.InsertInitialData(NewCompanyDetails, NewUser, FBRParameters, NewLogo))
            {
                Username = NewUser.Username;
                Password = NewUser.Password;
                _messages.ShowInfoNotification($"Use these credential to login. \nDefault username : {NewUser.Username} \nDefault " +
                                $"Password : {NewUser.Password} \nYou can change them later in Settings.");

                if (window is not null)
                {
                    window.Close();
                    ShowLogin();
                }
            }
            else
            {
                HideLogin();
                //DeleteDatabaseFile();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private void LoginLoadData()
    {
        DatabaseTableSchema = new List<string>
            {
                "ChargeID",
                "ChargeType",
                "ChargeAmount",
                "CategoryID",
                "Category",
                "ID",
                "Name",
                "Country",
                "StartYear",
                "Address1",
                "Address2",
                "Email",
                "Phone",
                "Logo",
                "PrintMessage1",
                "PrintMessage2",
                "CurrencySymbol",
                "ID",
                "Name",
                "Phone",
                "NTN",
                "CNIC",
                "Address",
                "EmailAddress",
                "AddedDate",
                "Remarks",
                "BlackList",
                "CategoryID",
                "Category",
                "ExpansesID",
                "ExpansesDescription",
                "Amount",
                "ExpanseDate",
                "ExpanseCategory",
                "Note",
                "POSID",
                "TransactionID",
                "InvoiceNumber",
                "Code",
                "Response",
                "Errors",
                "PaymentType",
                "SizeID",
                "ProductID",
                "ProductSize",
                "Discount",
                "Tax",
                "PurchaseQuantity",
                "PurchasePrice",
                "SalePrice",
                "Identifier",
                "PCTCode",
                "Category",
                "TimesSold",
                "ID",
                "Name",
                "Description",
                "Identifier",
                "PCTCode",
                "AddedDate",
                "PurchasePrice",
                "SalePrice",
                "Category",
                "PurchaseQuantity",
                "Picture",
                "TimesSold",
                "IsVariantProduct",
                "IsAvailable",
                "Discount",
                "Tax",
                "PurchaseID",
                "PurchaseDate",
                "SupplierID",
                "SupplierName",
                "PurchaseBY",
                "PaymentType",
                "DiscountPercent",
                "DiscountAmount",
                "TaxPercent",
                "TaxAmount",
                "SubTotal",
                "GrandTotal",
                "PurchaseNote",
                "DiscountOnTotal",
                "TransactionType",
                "PurchaseID",
                "ProductName",
                "PurchasePrice",
                "SalePrice",
                "Qty",
                "Total",
                "DiscountPercent",
                "DiscountAmount",
                "TaxPercent",
                "TaxAmount",
                "TransactionID",
                "CustomerID",
                "Narration",
                "InvoiceDate",
                "InvoiceTime",
                "SubTotal",
                "DiscountValue",
                "DiscountPrice",
                "GSTValue",
                "GSTPrice",
                "AdditionalCharges",
                "GrandTotal",
                "Cash",
                "Change",
                "PaymentType",
                "IsPaid",
                "Cashier",
                "IsFBRInvoice",
                "DiscountOnTotal",
                "TransactionType",
                "TransactionDetailsID",
                "TransactionID",
                "Name",
                "Qty",
                "Price",
                "Discount",
                "Tax",
                "Total",
                "IsVariantProduct",
                "ProductID",
                "SupplierID",
                "SupplierName",
                "Email",
                "Phone",
                "Location",
                "Website",
                "Note",
                "AddedDate",
                "UserID",
                "Username",
                "Password",
                "AddAccount",
                "EditAccount",
                "DeleteAccount",
                "AddInventory",
                "EditInventory",
                "DeleteInventory",
                "CreateInvoice",
                "DeleteInvoice",
                "AddExpanse",
                "EditExpanse",
                "DeleteExpanse",
                "InteractDashboard",
                "ViewReports",
                "EditSettings",
                "TransactionID",
                "CustomerID",
                "Narration",
                "InvoiceDate",
                "InvoiceTime",
                "SubTotal",
                "DiscountValue",
                "DiscountPrice",
                "GSTValue",
                "GSTPrice",
                "AdditionalCharges",
                "GrandTotal",
                "Cash",
                "Change",
                "PaymentType",
                "IsPaid",
                "Cashier",
                "IsFBRInvoice",
                "DiscountOnTotal",
                "TransactionType",
                "TransactionDetailsID",
                "TransactionID",
                "Name",
                "Qty",
                "Price",
                "Discount",
                "Tax",
                "Total",
                "IsVariantProduct",
                "ProductID",
                "TransactionID",
                "InvoiceNumber",
                "Code",
                "Response",
                "Errors",
        };
        string dataBasePath = ConnectionHelper.CnnVal();
        SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder(dataBasePath);
        if (!File.Exists(builder.DataSource))
        {
            HideLogin();
        }
        else
        {
            if (!CheckDatabaseIsValid(dataBasePath))
            {
                HideLogin();
            }
            else
            {
                ShowLogin();
            }
        }
    }

    private void ExecuteLogin(Window window)
    {
        try
        {
            Settings.Default.LastUsedUsername = Username;
            Settings.Default.Save();
            CommonProperties.CompanyDetails = _companyService.GetCompanyInfo();

            MainWindow mw = new();
            var user = _loginService.Login(Username, Password);
            if (user is not null)
            {
                CommonProperties.CurrentUser = user;
                CurrentUser = user;
                IsUserLoggedIn = true;
                UserHasLoggedIn();
                //if (window is not null)
                //{
                //    window.Close();
                //    mw.Show();
                //}
                //else
                //{
                //    _messages.ShowErrorNotification("Something terrible went wrong, restart application");
                //}
            }
            else
            {
                _messages.ShowInfoNotification("Username or Password Incorrect.");
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
        Password = string.Empty;
    }

    private void ExecuteRemoveCompanyLogo()
    {
        NewCompanyDetails.Logo = null;
    }

    private void ExecuteSelectDatabase()
    {
        try
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title = "Select Database File";
            dlg.Filter = "Database files (*.db, *.sqlite, *.sqlite3) | *.db; *.sqlite; *.sqlite3";
            if (dlg.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(dlg.FileName))
                {
                    string newConnectionStng = string.Format(format: "Data Source={0};Version=3;", dlg.FileName);
                    if (CheckDatabaseIsValid(newConnectionStng))
                    {
                        ShowLogin();
                        UpdateConnectionString(newConnectionStng);
                    }
                    else
                    {
                        HideLogin();
                        _messages.ShowErrorNotification("Some of the tables not found, bad database.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            HideLogin();
            _messages.ShowErrorMessage(e.Message);
        }
    }

    #endregion Command Functions

    #region Private Functions

    public static ObservableCollection<string> GetCountryList()
    {
        ObservableCollection<string> cultureList = new ObservableCollection<string>();
        CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
        foreach (CultureInfo culture in cultures)
        {
            RegionInfo region = new RegionInfo(culture.Name);
            if (!(cultureList.Contains(region.EnglishName)))
            {
                cultureList.Add(region.EnglishName);
            }
        }
        return cultureList;
    }

    private bool CheckDatabaseIsValid(string connectionString)
    {
        try
        {
            List<string> TablesHeader = new List<string>();
            string dataBasePath = connectionString;
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder(dataBasePath);

            using (IDbConnection db = new SQLiteConnection(connectionString))
            {
                var collection = db.Query<string>(TableSchemaCommand);
                foreach (var item in collection)
                {
                    TablesHeader.Add(item);
                }
            }

            return Enumerable.SequenceEqual(TablesHeader, DatabaseTableSchema);
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    private bool CreateTables(string connectionString)
    {
        try
        {
            string dataBasePath = connectionString;
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder(dataBasePath);
            using IDbConnection db = new SQLiteConnection(connectionString, true);
            db.Open();
            using var transaction = db.BeginTransaction();
            db.Execute(CreateTablesCommand);
            transaction.Commit();
            db.Close();
            return transaction.ReturnSuccess();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

 

    private void HideLogin()
    {
        CreateNewDatabase = true;
        CanLogin = false;
    }

    private void ShowLogin()
    {
        CreateNewDatabase = false;
        CanLogin = true;
    }

    private void UpdateConnectionString(string newConnectionString)
    {
        try
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection section = (ConnectionStringsSection)config.GetSection("connectionStrings");
            section.ConnectionStrings["Counter"].ConnectionString = newConnectionString;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("connectionStrings");
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    #endregion Private Functions

    #endregion Login
}