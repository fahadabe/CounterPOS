namespace CounterPOS.ViewModel;

public partial class ProductViewModel : ObservableObject
{
    public int Incrementer;
    private readonly ICategoryService _categoryService;
    private readonly ICustomerService _customerService;
    private readonly IEasyPrinting _easyPrinting;
    private readonly IImageHelper _imageHelper;
    private readonly IDialogMessages _messages;
    private readonly IProductService _productService;
    private readonly ISaleReportService _saleReportService;
    private bool isImageChanged = false;

    public ProductViewModel(IProductService productService, ICategoryService categoryService, ISaleReportService saleReportService, ICustomerService customerService, IDialogMessages messages, IEasyPrinting easyPrinting, IImageHelper imageHelper)
    {
        _productService = productService;
        _categoryService = categoryService;
        _saleReportService = saleReportService;
        _customerService = customerService;
        _messages = messages;
        _easyPrinting = easyPrinting;
        _imageHelper = imageHelper;
    }
    #region Commands

    public DelegateCommand AddCategoryWindowCommand => new DelegateCommand(ExecuteAddCategoryWindow, CanExecuteAddCategoryWindow);
    public AsyncCommand AddProductCommand => new AsyncCommand(ExecuteAddProduct, CanExecuteAddProduct);
    public AsyncCommand DeleteProductCommand => new AsyncCommand(ExecuteDeleteProduct, CanExecuteDeleteProduct);
    public DelegateCommand EditAddPictureCommand => new DelegateCommand(ExecuteEditAddPictureCommand);
    public AsyncCommand EditProductCommand => new AsyncCommand(ExecuteEditProduct, CanExecuteEditProduct);
    public DelegateCommand EditRemovePictureCommand => new DelegateCommand(ExecuteEditRemovePictureCommand);
    public AsyncCommand<string> GetProductPerformanceCommand => new AsyncCommand<string>(ExecuteGetProductPerformance);
    public AsyncCommand GetSimpleProductSaleDetailsCommand => new AsyncCommand(ExecuteGetSimpleProductSaleDetails);
    public DelegateCommand HideViewCommand => new DelegateCommand(ExecuteHideViewCommand);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public DelegateCommand NewAddPictureCommand => new DelegateCommand(ExecuteNewAddPictureCommand);
    public DelegateCommand NewProductAddSizeCommand => new DelegateCommand(ExecuteNewProductAddSize);
    public DelegateCommand<ProductSizes> NewProductRemoveSizeCommand => new DelegateCommand<ProductSizes>(NewProductRemoveSize);
    public DelegateCommand NewRemovePictureCommand => new DelegateCommand(ExecuteNewRemovePictureCommand);
    public AsyncCommand RefreshCategoriesCommand => new AsyncCommand(ExecuteRefreshCategories);
    public AsyncCommand RefreshProductsCommand => new AsyncCommand(ExecuteRefreshProduct);
    public DelegateCommand SelectedProductAddSizeCommand => new DelegateCommand(ExecuteSelectedProductAddSize);
    public DelegateCommand<ProductSizes> SelectedProductRemoveSizeCommand => new DelegateCommand<ProductSizes>(ExecuteSelectedProductRemoveSize);
    public DelegateCommand ShowEditProductViewCommand => new DelegateCommand(ExecuteShowEditProductView, CanExecuteEditProductWindow);
    public AsyncCommand ShowNewProductViewCommand => new AsyncCommand(ExecuteShowNewProductView, CanExecuteAddProductWindow);
    public AsyncCommand TransactionViewCommand => new AsyncCommand(ExecuteTransactionView, CanExecuteTransactionView);
    #endregion Commands

    #region Properties

    [ObservableProperty]
    private ObservableCollection<CategoryModel> _CategoryList = new();

    [ObservableProperty]
    private CompanyModel _CompanyDetails = new();

    [ObservableProperty]
    private UserModel _CurrentUser = new();

    [ObservableProperty]
    private ObservableCollection<ProductModel> _FilteredProductList;

    [ObservableProperty]
    private ObservableCollection<ProductPerformanceModel> _FilterProductSaleDetailList = new();

    [ObservableProperty]
    private int _InactiveProductCount = 0;

    [ObservableProperty]
    private bool _IsEditProductViewVisible = false;

    [ObservableProperty]
    private bool _IsNewProductViewVisible = false;

    [ObservableProperty]
    private bool _IsProductPerformaceViewVisible = true;

    [ObservableProperty]
    private ObservableCollection<ProductModel> _MasterProductList;

    [ObservableProperty]
    private ObservableCollection<ProductPerformanceModel> _MasterProductSaleDetailList = new();

    [ObservableProperty]
    private ProductModel _NewProduct = new();

    [ObservableProperty]
    private string _NewProductPicture = string.Empty;

    [ObservableProperty]
    private bool _NewProductSizeLimitReached = false;

    [ObservableProperty]
    private ObservableCollection<ProductSizes> _NewProductSizeList = new();

    [ObservableProperty]
    private ObservableCollection<TransactionModel> _PrintInvoiceSource = new();

    [ObservableProperty]
    private int _ProductCount = 0;

    [ObservableProperty]
    private string _ProductFilterText = string.Empty;

    [ObservableProperty]
    private string _ProductSaleSearchTerm = string.Empty;

    [ObservableProperty]
    private int _Quantity;

    [ObservableProperty]
    private CategoryModel _SelectedCategory = new();

    [ObservableProperty]
    private ProductModel _SelectedProduct = new();

    [ObservableProperty]
    private object _SelectedProductPicture;

    [ObservableProperty]
    private ProductPerformanceModel? _SelectedProductSale = new();

    [ObservableProperty]
    private bool _SelectedProductSizeLimitReached;

    [ObservableProperty]
    private ObservableCollection<ProductSizes> _SelectedProductSizeList = new();

    [ObservableProperty]
    private TransactionModel _SelectedTransaction;

    [ObservableProperty]
    private ObservableCollection<string> _SizeCollection = new();

    [ObservableProperty]
    private decimal _TotalAmount;

    [ObservableProperty]
    private ObservableCollection<TransactionDetailsModel> _TransactionDetails = new();

     partial void OnFilteredProductListChanged(ObservableCollection<ProductModel> value)
    {
        MakeProductSummary();
    }

     partial void OnFilterProductSaleDetailListChanged(ObservableCollection<ProductPerformanceModel> value)
    {
        MakeItemPerformanceSummary();
    }

     partial void OnProductFilterTextChanged(string value)
    {
        FilterPorducts();
    }

     partial void OnProductSaleSearchTermChanged(string value)
    {
        FilterPorductSale();
    }

     partial void OnSelectedProductChanged(ProductModel value)
    {
        GetSizesAndProductPerformance();
    }

    #endregion Properties

    #region Command Functions

    private bool CanExecuteAddCategoryWindow()
    {
        return CurrentUser?.AddInventory ?? false;
    }

    private bool CanExecuteAddProduct()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecuteAddProductWindow()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecuteDeleteProduct()
    {
        return CurrentUser?.DeleteInventory ?? false && SelectedProduct is not null;
    }

    private bool CanExecuteEditProduct()
    {
        return CurrentUser?.EditInventory ?? false && SelectedProduct is not null;
    }

    private bool CanExecuteEditProductWindow()
    {
        return CurrentUser?.EditInventory ?? false && SelectedProduct is not null;
    }

    private bool CanExecuteTransactionView()
    {
        return CurrentUser?.ViewReports ?? false && SelectedTransaction is not null;
    }

    private void ExecuteAddCategoryWindow()
    {
        var categoryViewModel = App.GetService<CategoryViewModel>();
        Category objC = new Category(categoryViewModel);
        objC.ShowDialog();
    }

    private async Task ExecuteAddProduct()
    {
        int maxID = _productService.GetLastProductID();
        if (NewProduct.IsVariantProduct)
        {
            if (!string.IsNullOrWhiteSpace(NewProduct.Name) && !string.IsNullOrWhiteSpace(NewProduct.Category))
            {
                if (NewProductSizeList.Count > 0)
                {
                    if (NewProductSizeList.Any(a => a.ProductSize == string.Empty) || NewProductSizeList.Any(a => a.Identifier == string.Empty))
                    {
                        _messages.ShowWarningNotification("Please define sizes correctly.");
                    }
                    else
                    {
                        if (await _productService.AddSizedProductAsync(NewProduct, NewProductPicture, NewProductSizeList))
                        {
                            NewProduct.ID = maxID;
                            MasterProductList.Add(NewProduct);
                            FilteredProductList.Add(NewProduct);
                            NewProductPicture = string.Empty;
                            NewProductSizeLimitReached = false;
                            NewProductSizeList.Clear();
                            NewProduct = new();
                        }
                    }
                }
                else
                {
                    _messages.ShowWarningNotification("Please define atleast one size.");
                }
            }
            else
            {
                _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
            }
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(NewProduct.Name) && !string.IsNullOrWhiteSpace(NewProduct.Identifier) && !string.IsNullOrWhiteSpace(NewProduct.Category))
            {
                if (await _productService.AddProductAsync(NewProduct, NewProductPicture))
                {
                    NewProduct.ID = maxID;
                    MasterProductList.Add(NewProduct);
                    FilteredProductList.Add(NewProduct);
                    NewProduct = new();
                    NewProductPicture = string.Empty;
                }
            }
            else
            {
                _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
            }
        }
    }

    private async Task ExecuteDeleteProduct()
    {
        if (SelectedProduct is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete '{SelectedProduct.Name}'?"))
            {
                if (await _productService.DeleteProductAsync(SelectedProduct))
                {
                    MasterProductList.Remove(SelectedProduct);
                    FilteredProductList.Remove(SelectedProduct);
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private void ExecuteEditAddPictureCommand()
    {
        var path = _imageHelper.BrowseImage();
        if (!string.IsNullOrWhiteSpace(path))
        {
            SelectedProductPicture = path;
            isImageChanged = true;
        }
        else
        {
            isImageChanged = false;
        }
    }

    private async Task ExecuteEditProduct()
    {
        if (SelectedProduct is not null)
        {
            if (SelectedProduct.IsVariantProduct)
            {
                if (!string.IsNullOrWhiteSpace(SelectedProduct.Name) && !string.IsNullOrWhiteSpace(SelectedProduct.Category))
                {
                    if (SelectedProductSizeList!.Count > 0)
                    {
                        if (SelectedProductSizeList.Any(a => a.ProductSize == string.Empty) || SelectedProductSizeList.Any(a => a.Identifier == string.Empty))
                        {
                            _messages.ShowWarningNotification("Please define sizes correctly.");
                        }
                        else
                        {
                            await _productService.UpdateSizedProductAsync(SelectedProduct, SelectedProductSizeList, SelectedProductPicture, isImageChanged, SelectedProductPicture);
                        }
                    }
                    else
                    {
                        _messages.ShowWarningNotification("Please define atleast one size.");
                    }
                }
                else
                {
                    _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(SelectedProduct.Name) && !string.IsNullOrWhiteSpace(SelectedProduct.Identifier) && !string.IsNullOrWhiteSpace(SelectedProduct.Category))
                {
                    await _productService.UpdateProductAsync(SelectedProduct, SelectedProductPicture, isImageChanged, SelectedProductPicture);
                }
                else
                {
                    _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
                }
            }
            isImageChanged = false;
        }
    }

    private void ExecuteEditRemovePictureCommand()
    {
        SelectedProductPicture = "";
        isImageChanged = false;
    }

    private void ExecuteHideViewCommand()
    {
        ShowTransactionsView();
    }

    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        Incrementer = 0;
        await GetProducts();
        await GetCategories();
    }

    private void ExecuteNewAddPictureCommand()
    {
        var path = _imageHelper.BrowseImage();
        if (!string.IsNullOrWhiteSpace(path))
        {
            NewProductPicture = path;
        }
    }

    private void ExecuteNewProductAddSize()
    {
        if (NewProductSizeList?.Count == 4)
        {
            NewProductSizeLimitReached = true;
        }
        else
        {
            NewProductSizeLimitReached = false;
        }
        NewProductSizeList?.Add(new ProductSizes { ProductSize = "", PurchasePrice = 0, SalePrice = 0, Index = Incrementer });
        Incrementer++;
    }

    private void ExecuteNewRemovePictureCommand()
    {
        NewProductPicture = null;
    }

    private async Task ExecuteRefreshCategories()
    {
        await GetCategories();
    }

    private async Task ExecuteRefreshProduct()
    {
        await GetProducts();
    }

    private void ExecuteSelectedProductAddSize()
    {
        if (SelectedProductSizeList.Count == 4)
        {
            SelectedProductSizeLimitReached = true;
        }
        else
        {
            SelectedProductSizeLimitReached = false;
        }

        SelectedProductSizeList.Add(new ProductSizes { ProductSize = "", PurchasePrice = 0, SalePrice = 0, Index = Incrementer });
        Incrementer++;
    }

    private void ExecuteSelectedProductRemoveSize(ProductSizes obj)
    {
        if (SelectedProductSizeList.Count < 6)
        {
            SelectedProductSizeLimitReached = false;
        }
        SelectedProductSizeList.Remove(SelectedProductSizeList.Where(i => i.Index == obj.Index).Single());
        if (SelectedProductSizeList.Count == 0)
        {
            SelectedProduct.IsVariantProduct = false;
        }
    }

    private void ExecuteShowEditProductView()
    {
        if (SelectedProduct is not null)
        {
            ShowEditProductView();
            PopulateSizeCollection();
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteShowNewProductView()
    {
        NewProduct = new();

        ShowNewProductView();
        PopulateSizeCollection();
        await GetCategories();
    }

    private async Task ExecuteTransactionView()
    {
        if (SelectedProductSale is not null)
        {
            SelectedTransaction = await _productService.GetTransactionAsync(SelectedProductSale.TransactionID);
            if (SelectedTransaction is not null)
            {
                await PrepareDataForPrinting(SelectedTransaction);
                _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, true, false, SelectedTransaction.IsPaid);
                PrintInvoiceSource.Clear();
            }
            else
            {
                _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
            }
        }
    }

    private void ShowEditProductView()
    {
        IsEditProductViewVisible = true;
        IsNewProductViewVisible = false;
        IsProductPerformaceViewVisible = false;
    }

    private void ShowNewProductView()
    {
        IsEditProductViewVisible = false;
        IsNewProductViewVisible = true;
        IsProductPerformaceViewVisible = false;
    }

    private void ShowTransactionsView()
    {
        IsEditProductViewVisible = false;
        IsNewProductViewVisible = false;
        IsProductPerformaceViewVisible = true;
    }

    #endregion Command Functions

    #region Private Functions

    private void AddSizesToSelectedProductSizeList()
    {
        SelectedProductSizeList.Clear();

        foreach (var item in _productService.GetSizesForSelectedProduct(SelectedProduct.ID))
        {
            SelectedProductSizeList.Add(new ProductSizes
            {
                ProductID = item.ProductID,
                Picture = item.Picture,
                ProductSize = item.ProductSize,
                Discount = item.Discount,
                Tax = item.Tax,
                PurchasePrice = item.PurchasePrice,
                SalePrice = item.SalePrice,
                Identifier = item.Identifier,
                PCTCode = item.PCTCode,
                IsVariantProduct = item.IsVariantProduct,
                Category = item.Category,
                TimesSold = item.TimesSold,
                Index = Incrementer,
                PurchaseQuantity = item.PurchaseQuantity
            });
            Incrementer++;
        }
        if (SelectedProductSizeList.Count == 5)
        {
            SelectedProductSizeLimitReached = true;
        }
    }

    private async Task ExecuteGetProductPerformance(string obj)
    {
        if (string.IsNullOrWhiteSpace(obj) || string.IsNullOrWhiteSpace(SelectedProduct.Name))
        {
            return;
        }

        string productName = $"{SelectedProduct.Name} {obj}";
        await GetproductSaleDetails(productName);
    }

    private async Task ExecuteGetSimpleProductSaleDetails()
    {
        if (SelectedProduct is not null)
        {
            if (string.IsNullOrWhiteSpace(SelectedProduct.Name))
            {
                return;
            }

            SelectedProductSizeList.Clear();
            await GetproductSaleDetails(SelectedProduct.Name);
        }
    }

    private void FilterPorducts()
    {
        FilteredProductList = MasterProductList.Where(i => i.ProductInfo.ToLower().Contains(ProductFilterText.ToLower())).ToObservableCollection();
    }

    private void FilterPorductSale()
    {
        FilterProductSaleDetailList = MasterProductSaleDetailList.Where(i => i.Filterable.ToLower().Contains(ProductSaleSearchTerm.ToLower())).ToObservableCollection();
    }

    private async Task GetCategories()
    {
        CategoryList = await _categoryService.GetAllCategoriesAsync();
    }

    private async Task GetProducts()
    {
        MasterProductList = await _productService.GetAllProductsAsync();
        FilteredProductList = MasterProductList;
        FilterPorducts();
    }

    private async Task GetproductSaleDetails(string productName)
    {
        MasterProductSaleDetailList.Clear();
        MasterProductSaleDetailList = await _productService.GetProductPerformance(productName);
        ProductSaleSearchTerm = string.Empty;
        FilterPorductSale();
    }

    private void GetSizesAndProductPerformance()
    {
        if (SelectedProduct is not null)
        {
            SelectedProductPicture = SelectedProduct.Picture;
            MasterProductSaleDetailList.Clear();
            if (SelectedProduct.IsVariantProduct)
            {
                AddSizesToSelectedProductSizeList();
            }
            ProductSaleSearchTerm = string.Empty;
            FilterPorductSale();
            if (SelectedProductSizeList.Count < 5)
            {
                SelectedProductSizeLimitReached = false;
            }
            else if (SelectedProductSizeList.Count == 0)
            {
                SelectedProduct.IsVariantProduct = false;
            }
        }
    }
    private void MakeItemPerformanceSummary()
    {
        if (FilterProductSaleDetailList is not null)
        {
            Quantity = FilterProductSaleDetailList.Count();
            TotalAmount = FilterProductSaleDetailList.Sum(i => i.Total);
        }
    }

    private void MakeProductSummary()
    {
        if (FilteredProductList is not null)
        {
            ProductCount = FilteredProductList.Count();
            InactiveProductCount = FilteredProductList.Count(x => x.IsAvailable == false);
        }
    }

    private void NewProductRemoveSize(ProductSizes obj)
    {
        if (NewProductSizeList?.Count < 6)
        {
            NewProductSizeLimitReached = false;
        }
        NewProductSizeList.Remove(NewProductSizeList.Where(i => i.Index == obj.Index).FirstOrDefault());
        if (NewProductSizeList?.Count == 0)
        {
            NewProduct.IsVariantProduct = false;
        }
    }

    private void PopulateSizeCollection()
    {
        SizeCollection.Clear();
        SizeCollection.Add("Small");
        SizeCollection.Add("Regular");
        SizeCollection.Add("Medium");
        SizeCollection.Add("Large");
        SizeCollection.Add("X-Large");
    }

    private async Task PrepareDataForPrinting(TransactionModel? obj)
    {
        if (SelectedTransaction is not null)
        {
            PrintInvoiceSource.Add
                   (new TransactionModel
                   {
                       TransactionID = obj.TransactionID,
                       //CustomerDetails = obj.CustomerDetails,
                       Narration = obj.Narration,
                       InvoiceDate = obj.InvoiceDate,
                       InvoiceTime = obj.InvoiceTime,
                       SubTotal = obj.SubTotal,
                       DiscountValue = obj.DiscountValue,
                       DiscountPrice = obj.DiscountPrice,
                       GSTValue = obj.GSTValue,
                       GSTPrice = obj.GSTPrice,
                       Cash = obj.Cash,
                       Change = obj.Change,
                       AdditionalCharges = obj.AdditionalCharges,
                       GrandTotal = obj.GrandTotal,
                       IsPaid = obj.IsPaid,
                       Cashier = obj.Cashier,
                       TransactionDetail = await SetTransactionDetailValuesForPrinting(obj),
                       CompanyDetails = SetCompanyDetails(),
                       //OrderNote = obj.OrderNote,
                       FBRResponse = await SetFBRResponse(obj),
                       FBRImageSource = @"Resources\FbrPosImage.png",
                       IsFBRInvoice = obj.IsFBRInvoice,
                       DiscountOnTotal = obj.DiscountOnTotal
                   });
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private ObservableCollection<CompanyModel> SetCompanyDetails()
    {
        ObservableCollection<CompanyModel> companyModel = new ObservableCollection<CompanyModel>
            {
                new CompanyModel
                {
                    Name = CompanyDetails.Name,
                    StartYear = CompanyDetails.StartYear,
                    Address1 = CompanyDetails.Address1,
                    Address2 = CompanyDetails.Address2,
                    Email = CompanyDetails.Email,
                    Phone = CompanyDetails.Phone,
                    Logo = CompanyDetails.Logo,
                    PrintMessage1 = CompanyDetails.PrintMessage1,
                    PrintMessage2 = CompanyDetails.PrintMessage2
                }
            };

        return companyModel;
    }

    private async Task<FBRResponseModel?> SetFBRResponse(TransactionModel obj)
    {
        if (obj.IsFBRInvoice)
        {
            return await _saleReportService.GetFBRResponseAsync(obj.TransactionID) ?? new FBRResponseModel();
        }
        else
        {
            FBRResponseModel fBRResponseModel = new FBRResponseModel();
            return fBRResponseModel;
        }
    }

    private async Task<ObservableCollection<TransactionDetailsModel>?> SetTransactionDetailValuesForPrinting(TransactionModel obj)
    {
        ObservableCollection<TransactionDetailsModel> items = new();
        if (obj is not null)
        {
            TransactionDetails = await _saleReportService.GetTransactionsDetailsAsync(obj.TransactionID);
            foreach (var item in TransactionDetails)
            {
                items.Add(new TransactionDetailsModel
                {
                    TransactionID = item.TransactionID,
                    Name = item.Name,
                    Qty = item.Qty,
                    Price = item.Price,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    Total = item.Total,
                });
            }
        }

        return items;
    }

    #endregion Private Functions
}