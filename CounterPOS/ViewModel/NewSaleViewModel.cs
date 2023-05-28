

using System.Threading;
using CounterPOS.Common;
using CounterPOS.Model;
using DevExpress.Xpf.Editors.Helpers;
using DevExpress.XtraReports.UI;

namespace CounterPOS.ViewModel;
public partial class NewSaleViewModel : ObservableObject
{
    private readonly IAdditionalChargesService _additionalChargesService;
    private readonly ICustomerService _customerService;
    private readonly IDashboardService _dashboardService;
    private readonly IEasyPrinting _easyPrinting;
    private readonly IFBRParametersService _fBRParametersService;
    private readonly IDialogMessages _messages;
    private readonly INewSaleService _newSaleService;
    private readonly IPaymentTypeService _paymentTypeService;
    private readonly ISaleReportService _saleReportService;
    private readonly IUserManagementService _userManagementService;
    public NewSaleViewModel(ISaleReportService saleReportService, IPaymentTypeService paymentTypeService, IUserManagementService userManagementService, INewSaleService newSaleService, INewPurchaseService newPurchaseService, IAdditionalChargesService additionalChargesService, ICustomerService customerService, IFBRParametersService fBRParametersService, IDashboardService dashboardService, IDialogMessages messages, IEasyPrinting easyPrinting)
    {
        _saleReportService = saleReportService;
        _paymentTypeService = paymentTypeService;
        _userManagementService = userManagementService;
        _newSaleService = newSaleService;
        _additionalChargesService = additionalChargesService;
        _customerService = customerService;
        _fBRParametersService = fBRParametersService;
        _dashboardService = dashboardService;
        _messages = messages;
        _easyPrinting = easyPrinting;
    }

    #region Global Variables

    public int Incrementer;
    private DispatcherTimer dt = new DispatcherTimer();

    #endregion Global Variables

    #region Commands

    //AddToCartListCommand ShowTransactionsCommand
    public DelegateCommand<AdditionalChargesModel> AddAdditionalChargesCommand => new DelegateCommand<AdditionalChargesModel>(ExecuteAddAdditionalCharges);
    public DelegateCommand AddCustomChargeCommand => new DelegateCommand(ExecuteAddCustomCharge);
    public AsyncCommand AddCustomerCommand => new AsyncCommand(ExecuteAddCustomer, CanExecuteAddCustomer);
    public DelegateCommand<ProductSizes> AddMultiSizeProductToCartListCommand => new DelegateCommand<ProductSizes>(ExecuteAddMultiSizeProductToCartList);
    public DelegateCommand AddNewPaymentTypeCommand => new DelegateCommand(ExecuteAddNewPaymentType);
    public DelegateCommand<SimpleProductModel> AddSimpleProductToCartListCommand => new DelegateCommand<SimpleProductModel>(ExecuteAddSimpleProductToCartList);
    public DelegateCommand AddToCartListCommand => new DelegateCommand(ExecuteAddToCartList);
    public DelegateCommand ClearAdditionalListCommand => new DelegateCommand(ExecuteClearChargesList, CanExecuteClearChargesList);
    public DelegateCommand ClearCartListCommand => new DelegateCommand(ExecuteClearCartList, CanExecuteClearCartList);
    public DelegateCommand ClearSelectedCustomer => new DelegateCommand(ExecuteClearSelectedCustomer);
    
    public DelegateCommand<CartModel> DecrementCartListitemQuantityCommand => new DelegateCommand<CartModel>(ExecuteDecrementCartListitemQuantity);
    public DelegateCommand<CartModel> DecrementDiscountPercentCommand => new DelegateCommand<CartModel>(ExecuteDecrementDiscountPercent);
    public DelegateCommand<CartModel> DecrementTaxPercentCommand => new DelegateCommand<CartModel>(ExecuteDecrementTaxPercent);
    public AsyncCommand DeleteTransactionsPermanentlyCommand => new AsyncCommand(ExecuteDeleteTransactionsPermanently, CanExecuteDeleteTransactionsPermanently);
    public AsyncCommand GetDeletedTransactionsCommand => new AsyncCommand(ExecuteGetDeletedTransactions);
    public AsyncCommand<string> GetTransactionsCommand => new AsyncCommand<string>(ExecuteRefreshTransactions);
    public DelegateCommand HideAdditionalChargesViewCommand => new DelegateCommand(ExecuteHideAdditionalChargesView);

    public DelegateCommand HideTransactionsCommand => new DelegateCommand(ExecuteHideTransactions);
    public DelegateCommand HideViewCommand => new DelegateCommand(ExecuteHideView);
    public DelegateCommand<CartModel> IncrementCartListitemQuantityCommand => new DelegateCommand<CartModel>(ExecuteIncrementCartListitemQuantity);
    public DelegateCommand<CartModel> IncrementDiscountPercentCommand => new DelegateCommand<CartModel>(ExecuteIncrementDiscountPercent);
    public DelegateCommand<CartModel> IncrementTaxPercentCommand => new DelegateCommand<CartModel>(ExecuteIncrementTaxPercent);
    public AsyncCommand<short> JustPrintInvoiceCommand => new AsyncCommand<short>(ExecuteJustPrintInvoice, CanExecuteJustPrintInvoice);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public DelegateCommand<SimpleProductModel> MinusFromCartListCommand => new DelegateCommand<SimpleProductModel>(ExecuteMinusFromCartList);
    public DelegateCommand NewAdditionalChargeCommand => new DelegateCommand(ExecuteNewAdditionalCharge);
    public DelegateCommand OpenAdditionalChargesPopupCommand => new DelegateCommand(ExecuteOpenAdditionalChargesPopup);

    public AsyncCommand<string> ProcessTransactionCommand => new AsyncCommand<string>(ExecuteProcessTransaction, CanExecuteProcessTransaction);
   
    public DelegateCommand<AdditionalChargesModel> RemoveAddedChargeCommand => new DelegateCommand<AdditionalChargesModel>(ExecuteRemoveAddedCharge);
    public DelegateCommand<AdditionalChargesModel> RemoveAdditonalChargeCommand => new DelegateCommand<AdditionalChargesModel>(ExecuteRemoveAdditonalCharge);
    public DelegateCommand<CartModel> RemoveItemFromcartList => new DelegateCommand<CartModel>(ExecuteRemoveItem);
    public AsyncCommand ResetCommand => new AsyncCommand(ExecuteReset);
    public DelegateCommand SaveMenuUISettingsCommand => new DelegateCommand(ExecuteSaveMenuUISettings);
    public DelegateCommand SetDineIn => new DelegateCommand(ExecuteSetDineIn);
    public DelegateCommand SetHomeDelivery => new DelegateCommand(ExecuteSetHomeDelivery);
    public DelegateCommand SetMenuSettingDefaultCommand => new DelegateCommand(ExecuteSetMenuSettingDefault);
    public DelegateCommand SetTakeAway => new DelegateCommand(ExecuteSetTakwAway);
    public DelegateCommand ShowManageAdditionalChargesViewCommand => new DelegateCommand(ExecuteShowManageAdditionalChargesView);
    public DelegateCommand ShowNewCustomerViewCommand => new DelegateCommand(ExecuteShowNewCustomerView);
    public AsyncCommand ShowTransactionsCommand => new AsyncCommand(ExecuteShowTransactions);
    public DelegateCommand SortCartListItemsCommand => new DelegateCommand(ExecuteSortCartListItems, CanExecuteSortCartListItems);
    public AsyncCommand<string> TransactionDeleteCommand => new AsyncCommand<string>(ExecuteDeletetransaction, CanExecuteDeleteTransaction);
    public AsyncCommand<string> TransactionViewCommand => new AsyncCommand<string>(ExecuteViewTransaction, CanExuteViewTransaction);
    public AsyncCommand UpdateAdditionalChargesCommand => new AsyncCommand(ExecuteUpdateAdditionalCharges);
    public AsyncCommand ShowPreviousOrderCommand => new AsyncCommand(ExecuteShowPreviousOrder);

    public AsyncCommand PreviousTransactionViewCommand => new AsyncCommand(ExecutePreviousTransactionView, CanExecutePreviousTransactionView);
    public AsyncCommand PreviousTransactionDeleteCommand => new AsyncCommand(ExecutePreviousTransactionDelete, CanExecutePreviousTransactionDelete);
    public DelegateCommand CopyCommand => new DelegateCommand(ExecuteCopy);

    public DelegateCommand ShowSaleSettingsCommand => new DelegateCommand(ExecuteShowSaleSettings);

    public DelegateCommand HideSaleSettingsViewCommand => new DelegateCommand(ExecuteHideSaleSettingsView);











    #endregion Commands
    
    #region Properties
    [ObservableProperty]
    private string _CartSectionHeader;
    [ObservableProperty]
    private bool _IsPreviousOrdersViewVisible;
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _PreviousOrdersList = new();
    [ObservableProperty]
    private string _SelectedCustomerName = "";
    [ObservableProperty]
    private TransactionModel _SelectedPreviousOrder;
    [ObservableProperty]
    private ObservableCollection<object> _ItemsToReset = new();
    [ObservableProperty]
    private ObservableCollection<AdditionalChargesModel> _AddedAdditionalCharges = new();
    [ObservableProperty]
    private decimal _AdditionalCharges;
    [ObservableProperty]
    private ObservableCollection<AdditionalChargesModel> _AdditionalChargesList = new();
    [ObservableProperty]
    private ObservableCollection<CartModel> _CartList = new();
    [ObservableProperty]
    private decimal _CartlistCount;
    [ObservableProperty]
    private int _CartListQuantity;
    [ObservableProperty]
    private CompanyModel _CompanyDetails = new();
    [ObservableProperty]
    private bool _CreateUIMenu;
    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private AdditionalChargesModel _CustomCharge = new();
    [ObservableProperty]
    private string _CustomerDetails = "";
    
    [ObservableProperty]
    private int _DeletedFBRInvoicesCount;
    [ObservableProperty]
    private decimal _DeletedTotalAmountSum;
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _DeletedTransactions = new();
    [ObservableProperty]
    private bool _DineInCheck = false;
    [ObservableProperty]
    private bool _DiscountAndTaxOnTotal = true;
    [ObservableProperty]
    private bool _DiscountAndTaxPerItem;
    [ObservableProperty]
    private decimal _DiscountPrice;
    [ObservableProperty]
    private int _FBRInvoicesCount;
    [ObservableProperty]
    private int _FBRInvoicesCountInPreviousOrders;
    [ObservableProperty]
    private FBRParametersModel _FBRPrameterModel = new();
    [ObservableProperty]
    private ObservableCollection<BaseProductModel> _FilteredProductList = new();
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _FilteredTransactionsList = new();
    [ObservableProperty]
    private bool _GiveDiscountAndTax;
    [ObservableProperty]
    private bool _HeaderAutoFill;
    [ObservableProperty]
    private string _HeaderLocation = "";
    [ObservableProperty]
    private string _HeaderOrientation = "";
    [ObservableProperty]
    private bool _HomeDeliveryCheck;
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _InvoiceReportDataSource = new();
    [ObservableProperty]
    private bool _IsCartSectionVisible = true;
    [ObservableProperty]
    private bool _IsLoading = false;
    [ObservableProperty]
    private bool _IsManageAdditionalChargesViewVisible = false;
    [ObservableProperty]
    private bool _IsNewCustomerViewVisible = false;
    [ObservableProperty]
    private bool _CartListIsEmpty = true;
    [ObservableProperty]
    private bool _IsSaleSettingChecked;
    [ObservableProperty]
    private bool _IsUISettingsMenuCheckedd = false;
    [ObservableProperty]
    private bool _IsViewAdditionalChargesChecked = false;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CartSectionHeader))]
    private int _LastRow;
    [ObservableProperty]
    private CustomerModel _SelectedCustomer;
    [ObservableProperty]
    private ObservableCollection<BaseProductModel> _MasterProductList = new();
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _MasterTransactionsList = new();
    [ObservableProperty]
    private CustomerModel _NewCustomer = new();
    [ObservableProperty]
    private int _NewOrder;
    [ObservableProperty]
    private string _OrderNote = "";
    [ObservableProperty]
    private string _OrderType = "";
    [ObservableProperty]
    private ObservableCollection<PaymentTypeModel> _PaymentTypeList = new();
    [ObservableProperty]
    private int _PreviousOrdersCount;
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _PrintInvoiceSource = new();
    [ObservableProperty]
    private string _ProductSearchTerm = "";
    [ObservableProperty]
    private TransactionModel _SelectedDeletedTransaction = new();
    [ObservableProperty]
    private string _SelectedPaymentText = "";
    [ObservableProperty]
    private PaymentTypeModel _SelectedPaymentType = new();
    [ObservableProperty]
    private string _SelectedPaymentTypeText = "";
    [ObservableProperty]
    private BaseProductModel _SelectedProduct = new();
    [ObservableProperty]
    private TransactionModel _SelectedTransaction = new();
    [ObservableProperty]
    private bool _ShowAdditionalChargesList;
    [ObservableProperty]
    private bool _ShowDeletedTransactions = false;
    [ObservableProperty]
    private bool _ShowMainView = true;
    [ObservableProperty]
    private bool _ShowTransactions = false;
    [ObservableProperty]
    private ObservableCollection<Tabs> _Tabs = new();
    [ObservableProperty]
    private bool _TakeAwayCheck = true;
    [ObservableProperty]
    private decimal _TodaysSale;
    [ObservableProperty]
    private int _TodaysTransactionsCount;
    [ObservableProperty]
    private decimal _TotalAmountSum;
    [ObservableProperty]
    private decimal _TotalAmountSumForPreviousOrders;
    [ObservableProperty]
    private int _TotalDeletedTransactionCount;
    [ObservableProperty]
    private int _TotalTransactionCount;
    [ObservableProperty]
    private TransactionModel _Transaction = new();
    [ObservableProperty]
    private ObservableCollection<TransactionDetailsModel> _TransactionDetails = new();
    [ObservableProperty]
    private string _TransactionFilterText = "";
    [ObservableProperty]
    private double _UIMenuButtonHeight;
    [ObservableProperty]
    private double _UIMenuButtonWidth;
    [ObservableProperty]
    private double _UIMenuItemNameFontSize;
    [ObservableProperty]
    private double _UIMenuItemProductImageHeight;
    [ObservableProperty]
    private double _UIMenuItemProductImageWidth;
    [ObservableProperty]
    private ObservableCollection<CustomerModel> _CustomerList = new();
    [ObservableProperty]
    private bool _IsSaleSettingsVisible = false;



    partial void OnIsSaleSettingsVisibleChanged(bool value)
    {
        SetSectionHeader(value);

    }

    private void SetSectionHeader(bool value)
    {
        if (value)
        {
            CartSectionHeader = "Sale Settings";
            return;
        }
        CartSectionHeader = $"Token #{LastRow}";
    }

    partial void OnPreviousOrdersListChanged(ObservableCollection<TransactionModel> value)
    {
        CreatePreviousOrdersSummary();
    }
    partial void OnAddedAdditionalChargesChanged(ObservableCollection<AdditionalChargesModel> value)
    {
        CheckIfChargesListIsEmpty();
    }

    partial void OnAdditionalChargesListChanged(ObservableCollection<AdditionalChargesModel> value)
    {
        CheckIfChargesListIsEmpty();
    }

    partial void OnCartListChanged(ObservableCollection<CartModel> value)
    {
        Calculation();
    }

    partial void OnCustomerDetailsChanged(string value)
    {
        SetNarration();
    }

    partial void OnDeletedTransactionsChanged(ObservableCollection<TransactionModel> value)
    {
        MakeDeletedTransactionsSummary();
    }

    partial void OnDiscountAndTaxOnTotalChanged(bool value)
    {
        if (value)
        {
            DiscountAndTaxPerItem = false;
            foreach (var item in CartList)
            {
                item.Discount = 0;
                item.Tax = 0;
            }
        }
    }

    partial void OnDiscountAndTaxPerItemChanged(bool value)
    {
        if (value)
        {
            DiscountAndTaxOnTotal = false;
            Transaction.DiscountValue = 0;
            Transaction.GSTValue = 0;
            SubTotalFunc();
        }
    }

    partial void OnFilteredTransactionsListChanged(ObservableCollection<TransactionModel> value)
    {
        MakeTransactionsSummary();
    }
    partial void OnSelectedCustomerChanged(CustomerModel value)
    {
        if (value is not null)
        {
            CustomerDetails = $"{FormatSelectedCustomerName(value.Name)}{FormatSelectedCustomerPhone(value.Phone)}{value.Address}";
        }
        else
        {
            CustomerDetails = "";
        }
    }

    partial void OnOrderNoteChanged(string value)
    {
        SetNarration();
    }

    partial void OnProductSearchTermChanged(string value)
    {
        FilterProducts();
    }

    partial void OnSelectedPaymentTypeChanged(PaymentTypeModel value)
    {
        if (value is not null)
        {
             Task.Run(() => GetTodaySale());
        }
    }

    partial void OnTransactionFilterTextChanged(string value)
    {
        FilterTransactions();
    }

    #endregion Properties

    #region Command Functions


    private bool CanExecuteSortCartListItems()
    {
        return CartList.Count > 1;
    }

    private void ExecuteSortCartListItems()
    {
        CartList = new ObservableCollection<CartModel>(CartList.OrderByDescending(i => i.Total));
    }
    private bool CanExecuteAddCustomer()
    {
        return CurrentUser?.AddAccount ?? false && NewCustomer is not null;
    }

    private bool CanExecuteClearCartList()
    {
        return CartList.Any();
    }

    private bool CanExecuteClearChargesList()
    {
        return AddedAdditionalCharges.Count > 0;
    }

    private bool CanExecuteDeleteTransaction(string arg)
    {
        if (string.Equals(arg, "live"))
        {
            return SelectedTransaction is not null && CurrentUser.DeleteInvoice;
        }
        else if (string.Equals(arg, "deleted"))
        {
            return SelectedDeletedTransaction is not null && CurrentUser.DeleteInvoice;
        }

        return false;
    }

    private bool CanExecuteDeleteTransactionsPermanently()
    {
        return DeletedTransactions.Count > 0 && CurrentUser.DeleteInvoice;
    }

    private bool CanExecuteJustPrintInvoice(short arg)
    {
        return CurrentUser.ViewReports;
    }

    private bool CanExecuteProcessTransaction(string obj)
    {
        return CurrentUser.CreateInvoice && CartList.Any();
    }

    
    private bool CanExuteViewTransaction(string arg)
    {
        if (string.Equals(arg, "live"))
        {
            return CurrentUser.ViewReports && SelectedTransaction is not null;
        }
        else if (string.Equals(arg, "deleted"))
        {
            return CurrentUser.ViewReports && SelectedDeletedTransaction is not null;
        }
        else
        {
            return false;
        }
    }

    private void CheckIfChargesListIsEmpty()
    {
        if (AdditionalChargesList is not null)
        {
            if (AdditionalChargesList.Count > 0)
            {
                ShowAdditionalChargesList = true;
            }
            else
            {
                ShowAdditionalChargesList = false;
            }
        }
    }

    private string ConvertToFullWord(string str)
    {
        switch (str)
        {
            case "S":
                return "Small";

            case "M":
                return "Medium";

            case "L":
                return "Large";

            case "X":
                return "Xtra-Large";

            case "R":
                return "Regular";

            default:
                return str;
        }
    }

    private async Task DeleteLiveTransaction()
    {
        if (SelectedTransaction is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete Transaction 'INV-{SelectedTransaction.TransactionID}'?"))
            {
                
                await _newSaleService.DeleteAnSaveTransactionAsync(SelectedTransaction);
                PrintInvoiceSource.Clear();
                MasterTransactionsList.Remove(SelectedTransaction);
                FilteredTransactionsList.Remove(SelectedTransaction);
                await GetTodaySale();
            }
        }
    }

    private async Task DeleteTransactionPermanantly()
    {
        if (SelectedDeletedTransaction is not null)
        {
            if (await _messages.AskUser($"Are you sure to permanently delete Transaction INV-'{SelectedDeletedTransaction.TransactionID}'?"))
            {
                await _saleReportService.DeleteTransactionPermanentlyAsync(SelectedDeletedTransaction);
                PrintInvoiceSource.Clear();
                DeletedTransactions.Remove(SelectedDeletedTransaction);
            }
        }
    }

    private void ExecuteAddAdditionalCharges(AdditionalChargesModel obj)
    {
        if (!AddedAdditionalCharges.Any(i => i.ChargeType == obj.ChargeType))
        {
            AddedAdditionalCharges.Add(new AdditionalChargesModel { ChargeType = obj.ChargeType, ChargeAmount = obj.ChargeAmount, Index = Incrementer });
        }
        SubTotalFunc();
    }

    private void ExecuteAddCustomCharge()
    {
        if (CustomCharge is not null && !string.IsNullOrWhiteSpace(CustomCharge.ChargeType) && CustomCharge.ChargeAmount > 0)
        {
            ExecuteAddAdditionalCharges(CustomCharge);
            CustomCharge = new();
        }
    }

    private async Task ExecuteAddCustomer()
    {
        if (!string.IsNullOrWhiteSpace(NewCustomer.Name))
        {
            if (await _customerService.AddCustomerAsync(NewCustomer))
            {
                var lastID = _customerService.GetLastCustomerID();
                ExecuteHideView();
                NewCustomer.Id = lastID;
                CustomerList.Add(NewCustomer);
                SelectedCustomer = CustomerList.LastOrDefault();
                
                NewCustomer = new();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private void ExecuteAddMultiSizeProductToCartList(ProductSizes obj)
    {
        try
        {
            if (DiscountAndTaxOnTotal)
            {
                AddToCart(obj.ItemName + " " + ConvertToFullWord(obj.ProductSize), 1, obj.SalePrice, obj.Picture, 0, 0, obj.Identifier, obj.PCTCode, obj.IsVariantProduct, obj.Category, obj.ProductID, obj.SizeID);
            }
            else
            {
                AddToCart(obj.ItemName + " " + ConvertToFullWord(obj.ProductSize), 1, obj.SalePrice, obj.Picture, obj.Discount, obj.Tax, obj.Identifier, obj.PCTCode, obj.IsVariantProduct, obj.Category, obj.ProductID, obj.SizeID);
            }

            var item = ItemsToReset.OfType<ProductSizes>().FirstOrDefault(i => $"{i.ItemName} {i.ProductSize}" == $"{obj.ItemName} {obj.ProductSize}");
            if (item is not null)
            {
                obj.AddedToCartQty = CartList.Where(i => i.Name == $"{obj.ItemName} {obj.ProductSize}").Select(i => i.Qty).FirstOrDefault();
            }
            else
            {
                obj.AddedToCartQty = CartList.Where(i => i.Name == $"{obj.ItemName} {obj.ProductSize}").Select(i => i.Qty).FirstOrDefault();
                ItemsToReset.Add(obj);
            }


        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

   

    private void ExecuteAddSimpleProductToCartList(SimpleProductModel obj)
    {
        if (DiscountAndTaxOnTotal)
        {
            AddToCart(obj.Name, 1, obj.SalePrice, obj.Picture, 0, 0, obj.Identifier, obj.PCTCode, obj.IsVariantProduct, obj.Category, obj.ID, 0);
        }
        else
        {
            AddToCart(obj.Name, 1, obj.SalePrice, obj.Picture, obj.Discount, obj.Tax, obj.Identifier, obj.PCTCode, obj.IsVariantProduct, obj.Category, obj.ID,0);
        }

        var item = ItemsToReset.OfType<SimpleProductModel>().FirstOrDefault(i => i.Name == obj.Name);
        if (item is not null)
        {
             
            obj.AddedToCartQty = CartList.Where(i => i.Name == obj.Name).Select(i => i.Qty).FirstOrDefault();
        }
        else
        {
            obj.AddedToCartQty = CartList.Where(i => i.Name == obj.Name).Select(i => i.Qty).FirstOrDefault();
            ItemsToReset.Add(obj);
        }
    }

    private void ExecuteAddToCartList()
    {
        if (SelectedProduct is not null)
        {
            if (DiscountAndTaxOnTotal)
            {
                AddToCart(SelectedProduct.Name, 1, SelectedProduct.SalePrice, SelectedProduct.Picture, 0, 0, SelectedProduct.Identifier, SelectedProduct.PCTCode, SelectedProduct.IsVariantProduct, SelectedProduct.Category, SelectedProduct.ID, SelectedProduct.SizeID);
            }
            else
            {
                AddToCart(SelectedProduct.Name, 1, SelectedProduct.SalePrice, SelectedProduct.Picture, SelectedProduct.Discount, SelectedProduct.Tax, SelectedProduct.Identifier, SelectedProduct.PCTCode, SelectedProduct.IsVariantProduct, SelectedProduct.Category, SelectedProduct.ID, SelectedProduct.SizeID);
            }
        }
    }

    private void ExecuteAddNewPaymentType()
    {
        var paymentTypeViewModel = App.GetService<PaymentTypeViewModel>();
        PaymentTypeWindow paymentTypeWindow = new(paymentTypeViewModel);
        paymentTypeWindow.ShowDialog();
    }

    private void ExecuteClearCartList()
    {
        CartList.Clear();
        AddedAdditionalCharges.Clear();
        AdditionalCharges = 0;
        SubTotalFunc();
        StopDispatcherIfCartListIsEmpty();
        ResetItems();
    }

    private void ExecuteClearChargesList()
    {
        AddedAdditionalCharges.Clear();
        SubTotalFunc();
    }

    private void ExecuteClearSelectedCustomer()
    {
        SelectedCustomer = null;
        CustomerDetails = "";
    }

    

    private void ExecuteDecrementCartListitemQuantity(CartModel obj)
    {
        MinusFromcart(obj.Name);
    }

    private void ExecuteDecrementDiscountPercent(CartModel obj)
    {
        if (obj.Discount > 0)
        {
            obj.Discount--;
        }
    }

    private void ExecuteDecrementTaxPercent(CartModel obj)
    {
        if (obj.Tax > 0)
        {
            obj.Tax--;
        }
    }

    private async Task ExecuteDeletetransaction(string arg)
    {
        switch (arg)
        {
            case "live":
                await DeleteLiveTransaction();
                break;

            case "deleted":
                await DeleteTransactionPermanantly();
                break;

            default:
                break;
        }
    }

    private async Task ExecuteDeleteTransactionsPermanently()
    {
        if (await _messages.AskUser("This will delete transactions permanently.\nAre you sure to continue?"))
        {
            if (await _newSaleService.DeleteTransactionPermanently(DeletedTransactions))
            {
                DeletedTransactions = await _newSaleService.GetDeletedTransactions();
                _messages.ShowSuccessNotification("Transactions Deleted");
            }
        }
    }

    private async Task ExecuteGetDeletedTransactions()
    {
        DeletedTransactions = await _newSaleService.GetDeletedTransactions();
    }

    private void ExecuteHideAdditionalChargesView()
    {
        IsManageAdditionalChargesViewVisible = false;
        IsCartSectionVisible = true;
        AdditionalChargesList.Clear();
        var collection = _additionalChargesService.GetAdditionalChargesAsync();
        foreach (var item in collection)
        {
            AdditionalChargesList.Add(new AdditionalChargesModel { ChargeType = item.ChargeType, ChargeAmount = item.ChargeAmount, Index = Incrementer });
            Incrementer++;
        }
        CheckIfChargesListIsEmpty();
    }

    

    private void ExecuteHideTransactions()
    {
        ShowMainView = true;
        ShowTransactions = false;
        ShowDeletedTransactions = false;
        MasterTransactionsList.Clear();
        FilteredTransactionsList.Clear();
        DeletedTransactions.Clear();
        Clipboard.Clear(); 
    }

    private void ExecuteHideView()
    {
        IsNewCustomerViewVisible = false;
        IsCartSectionVisible = true;
    }

    private void ExecuteIncrementCartListitemQuantity(CartModel obj)
    {
        IncrementQuantity(obj.Name);
    }

    private void ExecuteIncrementDiscountPercent(CartModel obj)
    {
        if (obj.Discount < 99)
        {
            obj.Discount++;
        }
    }

    private void ExecuteIncrementTaxPercent(CartModel obj)
    {
        if (obj.Tax < 99)
        {
            obj.Tax++;
        }
    }

    private async Task ExecuteJustPrintInvoice(short obj)
    {
        if (SelectedTransaction is not null)
        {
            await PrepareDataForPrintingForViewing(SelectedTransaction, "live");
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, false, true, SelectedTransaction.IsPaid, obj);
            PrintInvoiceSource.Clear();
        }
    }

    

    private async Task ExecuteLoadData()
    {
        dt.Tick += dtTicker!;
        dt.Interval = TimeSpan.FromSeconds(0.1);
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();

        CheckingSettings();
        ExecuteSetTakwAway();

        if (CreateUIMenu)
        {
            Tabs.Clear();
            await _newSaleService.CreateProductMenuAsync(Tabs);
        }
        else
        {
            await LoadProductsInCombo();
        }

        LastRow = _newSaleService.GetLasRow() + 1;
        FBRPrameterModel = _fBRParametersService.GetFBRParametersAsync();
        AdditionalChargesList = _additionalChargesService.GetAdditionalChargesAsync();
        await GetTodaySale();

        await GetCustomers();
        CartSectionHeader = $"Token #{LastRow}";
    }

    private void ExecuteMinusFromCartList(SimpleProductModel obj)
    {
        MinusFromcart(obj.Name);
    }

    private void ExecuteNewAdditionalCharge()
    {
        AdditionalChargesList.Add(new AdditionalChargesModel { ChargeType = "", ChargeAmount = 0, Index = Incrementer });
        Incrementer++;
        CheckIfChargesListIsEmpty();
        SubTotalFunc();
        //StopDispatcherIfCartListIsEmpty();
    }

    private void ExecuteOpenAdditionalChargesPopup()
    {
        IsViewAdditionalChargesChecked = true;
    }

  


    private async Task ExecuteRefreshTransactions(string obj)
    {
        TransactionFilterText = String.Empty;
        await GetTodayTransactions();
    }

    private void ExecuteRemoveAddedCharge(AdditionalChargesModel obj)
    {
        _ = AddedAdditionalCharges.Remove(AddedAdditionalCharges.FirstOrDefault(i => i == obj));
        SubTotalFunc();

        //StopDispatcherIfCartListIsEmpty();
    }

    private void ExecuteRemoveAdditonalCharge(AdditionalChargesModel obj)
    {
        _ = AdditionalChargesList.Remove(AdditionalChargesList.FirstOrDefault(i => i.Index! == obj.Index));
        CheckIfChargesListIsEmpty();
    }

    private void ExecuteRemoveItem(CartModel obj)
    {
        if (obj is not null)
        {
            _ = CartList.Remove(CartList.FirstOrDefault(i => i.Name == obj.Name));
            Transaction.SubTotal = CartList.Sum(a => a.Total);
            if (CartList.Count == 0)
            {
                Transaction.Cash = 0;
                Transaction.DiscountValue = 0;
                Transaction.GSTValue = 0;
            }
            SubTotalFunc();

            var itemsToResetCopy = ItemsToReset.ToList();

            foreach (var item in itemsToResetCopy)
            {
                if (item is ProductSizes size && string.Equals($"{size.ItemName} {size.ProductSize}", obj.Name))
                {
                    size.AddedToCartQty = 0;
                    ItemsToReset.Remove(size);
                }
                else if (item is SimpleProductModel simple  && string.Equals(simple.Name, obj.Name))
                {
                    simple.AddedToCartQty = 0;
                    ItemsToReset.Remove(simple);
                }
            }
        }



    }

    private async Task ExecuteReset()
    {
        await Reset();
    }

    private void ExecuteSaveMenuUISettings()
    {
        Settings.Default.UIMenuButtonWidth = UIMenuButtonWidth;
        Settings.Default.UIMenuButtonHeight = UIMenuButtonHeight;
        Settings.Default.UIMenuItemNameFontSize = UIMenuItemNameFontSize;
        Settings.Default.UIMenuItemProductImageWidth = UIMenuItemProductImageWidth;
        Settings.Default.UIMenuItemProductImageHeight = UIMenuItemProductImageHeight;
        Settings.Default.HeaderAutoFill = HeaderAutoFill;
        Settings.Default.HeaderLocation = HeaderLocation;
        Settings.Default.HeaderOrientation = HeaderOrientation;
        Settings.Default.Save();
    }

    private void ExecuteSetDineIn()
    {
        if (DineInCheck)
        {
            TakeAwayCheck = false;
            HomeDeliveryCheck = false;
            OrderType = OrderTypes.Dining;
        }
        else
        {
            OrderType = "";
        }
        SetNarration();
    }

    private void ExecuteSetHomeDelivery()
    {
        if (HomeDeliveryCheck)
        {
            DineInCheck = false;
            TakeAwayCheck = false;
            OrderType = OrderTypes.HomeDelivery;
        }
        else
        {
            OrderType = "";
        }
        SetNarration();
    }

    private void ExecuteSetMenuSettingDefault()
    {
        UIMenuButtonWidth = 50;
        UIMenuButtonHeight = 50;
        UIMenuItemNameFontSize = 16;
        UIMenuItemProductImageWidth = 90;
        UIMenuItemProductImageHeight = 90;
        HeaderAutoFill = true;
        HeaderLocation = "Left";
        HeaderOrientation = "Horizontal";
        ExecuteSaveMenuUISettings();
    }

    private void ExecuteSetTakwAway()
    {
        if (TakeAwayCheck)
        {
            DineInCheck = false;
            HomeDeliveryCheck = false;
            OrderType = OrderTypes.TakeAway;
        }
        else
        {
            OrderType = "";
        }
        SetNarration();
    }

    private void ExecuteShowManageAdditionalChargesView()
    {
        IsManageAdditionalChargesViewVisible = true;
        IsNewCustomerViewVisible = false;
        IsCartSectionVisible = false;
        AdditionalChargesList.Clear();
        var collection = _additionalChargesService.GetAdditionalChargesAsync();
        foreach (var item in collection)
        {
            AdditionalChargesList.Add(new AdditionalChargesModel { ChargeType = item.ChargeType, ChargeAmount = item.ChargeAmount, Index = Incrementer });
            Incrementer++;
        }
    }

    private void ExecuteShowNewCustomerView()
    {
        IsNewCustomerViewVisible = true;
        IsSaleSettingsVisible = false;
        IsManageAdditionalChargesViewVisible = false;
        IsCartSectionVisible = false;
    }
    private async Task ExecuteShowTransactions()
    {
        ShowTransactions = true;
        
        
            
            TransactionFilterText = string.Empty;
            ShowMainView = false;
            ShowDeletedTransactions = false;
        await Task.Delay(100);
        await GetTodayTransactions();
        
    }

    private bool CanExecutePreviousTransactionView()
    {
        return CurrentUser.ViewReports && SelectedPreviousOrder is not null;
    }

    private async Task ExecutePreviousTransactionView()
    {
        await PrepareDataForPrintingForViewing(SelectedPreviousOrder, "live");
        _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, true, false, SelectedPreviousOrder.IsPaid);
        PrintInvoiceSource.Clear();
    }


    private bool CanExecutePreviousTransactionDelete()
    {
        return SelectedPreviousOrder is not null && CurrentUser.DeleteInvoice;
    }

    private async Task ExecutePreviousTransactionDelete()
    {
        if (await _messages.AskUser($"Are you sure to delete Transaction 'INV-{SelectedPreviousOrder.TransactionID}'?"))
        {
            if (await _newSaleService.DeleteAnSaveTransactionAsync(SelectedPreviousOrder))
            {
                PreviousOrdersList.Remove(SelectedPreviousOrder);
            }
        }
    }
    private void ExecuteCopy()
    {
        Clipboard.SetText("showdeleted");
    }


    private void ExecuteShowSaleSettings()
    {
        IsSaleSettingsVisible = true;
        IsNewCustomerViewVisible = false;
    }

    private void ExecuteHideSaleSettingsView()
    {
        IsSaleSettingsVisible = false;
    }

    private async Task ExecuteShowPreviousOrder()
    {
        IsPreviousOrdersViewVisible = !IsPreviousOrdersViewVisible;
        if (IsPreviousOrdersViewVisible && SelectedCustomer is not null)
        {
            SelectedCustomerName = SelectedCustomer.Name;
            await Task.Delay(100);
            PreviousOrdersList = await _customerService.GetTransactionsByCustomerAsync(SelectedCustomer.Id);
            

        }
        else
        {
            PreviousOrdersList.Clear();
            SelectedCustomerName = "no customer selected";
        }
        CreatePreviousOrdersSummary();
    }
    private async Task ExecuteUpdateAdditionalCharges()
    {
        if (AdditionalChargesList!.Any(a => a.ChargeType == ""))
        {
            _messages.ShowWarningNotification("Please define additional charges correctly.");
        }
        else
        {
            if (await _additionalChargesService.UpdateAdditionalCharges(AdditionalChargesList))
            {
                AdditionalChargesList.Clear();
                var collection = _additionalChargesService.GetAdditionalChargesAsync();
                foreach (var item in collection)
                {
                    AdditionalChargesList.Add(new AdditionalChargesModel { ChargeType = item.ChargeType, ChargeAmount = item.ChargeAmount, Index = Incrementer });
                    Incrementer++;
                }
                IsManageAdditionalChargesViewVisible = false;
                IsCartSectionVisible = true;
            }

        }
    }

    private async Task ExecuteViewTransaction(string arg)
    {
        switch (arg)
        {
            case "live":
                await ProcessLiveTransaction();
                break;

            case "deleted":
                await ProcessDeletedTransaction();
                break;

            default:
                break;
        }
    }

    private async Task ProcessDeletedTransaction()
    {
        if (SelectedDeletedTransaction is not null)
        {
            await PrepareDataForPrintingForViewing(SelectedDeletedTransaction, "deleted");
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, true, false, SelectedDeletedTransaction.IsPaid);
            PrintInvoiceSource.Clear();
        }
    }

    private async Task ProcessLiveTransaction()
    {
        if (SelectedTransaction is not null)
        {
            await PrepareDataForPrintingForViewing(SelectedTransaction, "live");
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, true, false, SelectedTransaction.IsPaid);
            PrintInvoiceSource.Clear();
        }
    }
    #endregion Command Functions

    #region Private Functions
    
    public async Task LoadProductsInCombo()
    {
        MasterProductList = await _newSaleService.GetAllProducts();
        FilteredProductList = MasterProductList;
    }

    public async Task GetCustomers()
    {
        CustomerList = await _customerService.GetCustomersAsync();
    }

    public void SubTotalFunc()
    {
        Transaction.SubTotal = CartList!.Sum(a => a.Total);
        Calculation();
        CalculateCartlistSummary();
    }

    

    private void AddToCart(string itemName, int itemQty, decimal itemRate, byte[] picture, decimal discount,
       decimal tax, string identifier, string pctCode, bool isVariantProduct, string category, int productID, int sizeID)
    {
        try
        {
            dt.Start();
            
            var item = CartList.FirstOrDefault(i => i.Name == itemName);
            if (item != null)
            {
                item.Qty++;
            }
            else
            {
                CartList.Add(new CartModel
                {
                    Name = itemName,
                    Qty = itemQty,
                    SalePrice = itemRate,
                    Picture = picture,
                    Discount = discount,
                    Tax = tax,
                    Identifier = identifier,
                    PCTCode = pctCode,
                    IsVariantProduct = isVariantProduct,
                    Category = category,
                    ProductID = productID,
                    SizeID = sizeID
                });
            }
            SubTotalFunc();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    private void CalculateCartlistSummary()
    {
        CartlistCount = CartList.Count ;
        CartListQuantity = CartList.Sum(x => x.Qty);
        if (CartList.Any())
        {
            CartListIsEmpty = false;
        }
        else
        {
            CartListIsEmpty = true;
        }
    }

   
    private void Calculation()
    {
        decimal subTotal = Transaction.SubTotal;
        decimal selectedDiscount = Transaction.DiscountValue;
        decimal gstValue = Transaction.GSTValue;
        decimal discount = subTotal - ((subTotal * selectedDiscount) / 100);
        decimal priceAfterDiscount = subTotal - (subTotal * selectedDiscount / 100);
        Transaction.DiscountPrice = Transaction.SubTotal - priceAfterDiscount;
        decimal priceAfterGST = (priceAfterDiscount + (priceAfterDiscount * gstValue / 100));
        Transaction.GSTPrice = priceAfterGST - priceAfterDiscount;
        AdditionalCharges = AddedAdditionalCharges.Sum(i => i.ChargeAmount);
        Transaction.GrandTotal = priceAfterGST + AdditionalCharges;

        if (Transaction.Cash is 0 && Transaction.Cash < Transaction.GrandTotal)
        {
            Transaction.Change = 0;
        }
        else
        {
            Transaction.Change = Transaction.Cash - Transaction.GrandTotal;
        }
    }

    private bool CheckIfSelectedCustomerIsBlackListed()
    {
        return SelectedCustomer?.Blacklist ?? false;
    }

    private void CheckingSettings()
    {
        CreateUIMenu = Settings.Default.CreateUIMenuAtStartup;
        Transaction.IsFBRInvoice = Settings.Default.IsDefaultFBRInvoice;
        UIMenuButtonWidth = Settings.Default.UIMenuButtonWidth;
        UIMenuButtonHeight = Settings.Default.UIMenuButtonHeight;
        UIMenuItemNameFontSize = Settings.Default.UIMenuItemNameFontSize;
        UIMenuItemProductImageWidth = Settings.Default.UIMenuItemProductImageWidth;
        UIMenuItemProductImageHeight = Settings.Default.UIMenuItemProductImageHeight;
        HeaderAutoFill = Settings.Default.HeaderAutoFill;
        HeaderLocation = Settings.Default.HeaderLocation;
        HeaderOrientation = Settings.Default.HeaderOrientation;
        SelectedPaymentType = PaymentTypeList.FirstOrDefault(i => i.PaymentType == Settings.Default.DefaultPaymentType);

        GiveDiscountAndTax = CurrentUser.CreateInvoice;

        if (GiveDiscountAndTax)
        {
            Transaction.DiscountValue = Settings.Default.DefaultDiscountValue;
            Transaction.GSTValue = Settings.Default.DefaultGSTValue;
        }
        else
        {
            Transaction.DiscountValue = 0;
            Transaction.DiscountPrice = 0;
            Transaction.GSTValue = 0;
            Transaction.GSTPrice = 0;
        }
        if (Settings.Default.DiscountAndTaxOnTotal == true)
        {
            DiscountAndTaxOnTotal = true;
        }
        else
        {
            DiscountAndTaxPerItem = true;
        }
    }

    private void dtTicker(object sender, EventArgs e)
    {
        SubTotalFunc();
    }


    private FBRResponseModel? GetFBRResponseFromFBRService()
    {
        try
        {
            if (FBRPrameterModel is not null && !string.IsNullOrWhiteSpace(FBRPrameterModel.POSID.ToString()))
            {
                //HttpClientHandler handler = new();
                //handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                //HttpClient client = new(handler);
                FBRResponseModel fbrResponse = new();
                HttpClient client = new();

                client.DefaultRequestHeaders.Authorization = new("Bearer", "1298b5eb-b252-3d97-8622-a4a69d5bf818");
                var serializedObject = JsonConvert.SerializeObject(SetValueToFBRModel());
                var content = new StringContent(serializedObject, Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("https://esp.fbr.gov.pk:8244/FBR/v1/api/Live/PostData", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = response.Content.ReadAsStringAsync().Result;
                    fbrResponse = JsonConvert.DeserializeObject<FBRResponseModel>(jsonString);

                    if (fbrResponse is not null)
                    {
                        if (string.IsNullOrWhiteSpace(fbrResponse.Errors) && fbrResponse.Response.ToLower().Contains("successfully".ToLower()))
                        {
                            return fbrResponse;
                        }
                        else
                        {
                            _messages.ShowErrorNotification($"An error occurred while generating the Fiscal Invoice. Please check for any issues and try again.");
                            Transaction.IsFBRInvoice = false;
                            return null;
                        }
                    }
                }
            }
            _messages.ShowErrorNotification("There seems to be an issue with either the FBR Parameters or the FBR Fiscal Service. Please check the parameters or service to resolve the issue.");
            return null;
        }
        catch (Exception e)
        {
            Transaction.IsFBRInvoice = false;
            _messages.ShowErrorNotification(e.Message);
            return null;
        }
    }

    private async Task ExecuteProcessTransaction(string obj)
    {
        if (Transaction.IsFBRInvoice)
        {

            if (CartList.Any(item => string.IsNullOrEmpty(item.PCTCode)))
            {
                _messages.ShowWarningNotification("Some items in your cart are missing a required PCTCode for FBR Invoices. Please add the PCTCode to proceed.");
                return;
            }
            var fBRResponse = GetFBRResponseFromFBRService();
            if (fBRResponse is not null)
            {
                await AdditionalCheckingBeforeProcessing(obj, fBRResponse);
            }
        }
        else
        {
            await AdditionalCheckingBeforeProcessing(obj, null);
        }
    }
    
    private async Task AdditionalCheckingBeforeProcessing(string obj, FBRResponseModel fBRResponse)
    {
        try
        {
            SetTransactionValues();
            if (CheckIfSelectedCustomerIsBlackListed())
            {
                if (await _messages.AskUser($"Selected Customer '{SelectedCustomer.Name}' is Blacklisted.\n{SelectedCustomer.Remarks}\nContinue anyway?,"))
                {
                    await ProcessTransaction(obj, fBRResponse);
                }
            }
            else
            {
                await ProcessTransaction(obj, fBRResponse);
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            await Reset();
        }
    }
   
    private async Task ProcessTransaction(string obj, FBRResponseModel fBRResponseModel)
    {
        if (await _newSaleService.InsertTransactionAsync(Transaction, CartList, fBRResponseModel))
        {
            NewOrder = _newSaleService.ReturnLatestRow();
            var showInvoice = Settings.Default.ShowInvoiceOnCreating;
            var isPaid = Transaction.IsPaid;
            
            if (string.Equals(obj,"print"))
            {
                 PrepareDataForPrinting(fBRResponseModel);
                 _easyPrinting.PrintEasily(InvoiceReportDataSource, InvoiceType.SaleInvoice, showInvoice, true, isPaid);
            }
            else if (string.Equals(obj, "save"))
            {
                _easyPrinting.PrintEasily(InvoiceReportDataSource, InvoiceType.SaleInvoice, showInvoice, false, isPaid);
            }
           
            await Reset();
        }
    }


    private void FilterProducts()
    {
        FilteredProductList = MasterProductList.Where(i => i.ProductInfo.ToLower().Contains(ProductSearchTerm.ToLower())).ToObservableCollection();
    }

    private void FilterTransactions()
    {
        if (string.Equals(TransactionFilterText.ToLower(), "showdeleted".ToLower()))
        {
            ShowDeletedTransactionView();
        }
        else
        {
            FilteredTransactionsList = (from r in MasterTransactionsList!.AsEnumerable()
                                        where r.FIlterAble!.ToLower().Contains(TransactionFilterText.ToLower())
                                        select r).ToObservableCollection();
        }
    }

   

    private string FormatOrderNote()
    {
        if (!string.IsNullOrWhiteSpace(OrderNote))
        {
            return $"{OrderNote}\n\n";
        }
        else
        {
            return string.Empty;
        }
    }

    private string FormatSelectedCustomerName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }
        return $"{name}\n";
    }

    private string FormatSelectedCustomerPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return string.Empty;
        }
        return $"{phone}\n";
    }

    private ObservableCollection<FBRInvoiceItemsModel> GetFBRInvoiceItems()
    {
        ObservableCollection<FBRInvoiceItemsModel> items = new ObservableCollection<FBRInvoiceItemsModel>();
        foreach (var item in CartList!)
        {
            items.Add(new FBRInvoiceItemsModel
            {
                ItemCode = item.Identifier,
                ItemName = item.Name,
                PCTCode = item.PCTCode,
                Quantity = Convert.ToDouble(item.Qty),
                TaxRate = Convert.ToDouble(item.Tax),
                SaleValue = Convert.ToDouble(item.Total),
                Discount = Convert.ToDouble(item.Discount),
                FurtherTax = 0,
                TaxCharged = Convert.ToDouble(item.TaxCharged),
                TotalAmount = Convert.ToDouble(item.Total),
                InvoiceType = 1
            });
        }
        return items;
    }

    

    

    private async Task GetTodaySale()
    {
        TodaysSale = await _dashboardService.GetTodaySale(SelectedPaymentType.PaymentType);
        TodaysTransactionsCount = await _dashboardService.GetTodayTransactionsCount(SelectedPaymentType.PaymentType);
    }

    private async Task GetTodayTransactions()
    {
        if (CurrentUser?.ViewReports ?? false)
        {
            MasterTransactionsList = await _saleReportService.GetTodayTransactionsAsync();
            FilteredTransactionsList = MasterTransactionsList;
            FilterTransactions();
        }
    }

    private void IncrementQuantity(string itemName)
    {
        try
        {
            dt.Start();
            var item = CartList.FirstOrDefault(i => i.Name == itemName);
            if (item is not null)
            {
                item.Qty++;
                Transaction.SubTotal = CartList.Sum(a => a.Total);
            }

            foreach (var resetItem in ItemsToReset)
            {
                if (resetItem.GetType() == typeof(SimpleProductModel))
                {
                    SimpleProductModel simple = (SimpleProductModel)resetItem;

                    if (Equals(itemName, simple.Name))
                    {
                        simple.AddedToCartQty = item.Qty;
                    }

                }

                if (resetItem.GetType() == typeof(ProductSizes))
                {
                    ProductSizes size = (ProductSizes)resetItem;

                    if (Equals(itemName, $"{size.ItemName} {size.ProductSize}"))
                    {
                        size.AddedToCartQty = item.Qty;
                    }
                }
            }


            SubTotalFunc();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    private void MakeDeletedTransactionsSummary()
    {
        if (DeletedTransactions is not null)
        {
            TotalDeletedTransactionCount = DeletedTransactions.Count();
            DeletedFBRInvoicesCount = DeletedTransactions.Count(x => x.IsFBRInvoice);
            DeletedTotalAmountSum = DeletedTransactions.Sum(x => x.GrandTotal);
        }
    }

    
    private void MakeTransactionsSummary()
    {
        if (FilteredTransactionsList is not null)
        {
            TotalTransactionCount = FilteredTransactionsList.Count();
            FBRInvoicesCount = FilteredTransactionsList.Count(x => x.IsFBRInvoice);
            TotalAmountSum = FilteredTransactionsList.Sum(x => x.GrandTotal);
        }
    }

    private void MinusFromcart(string itemName)
    {
        try
        {
            
             var item = CartList.FirstOrDefault(i => i.Name == itemName);
             if (item is not null)
             {
                 if (item.Qty == 1)
                 {
                     _ = CartList.Remove(item);
                     item.Qty = 0;
                 }
                 else
                 {
                     item.Qty--;
                 }

                foreach (var resetItem in ItemsToReset)
                {
                    if (resetItem.GetType() == typeof(SimpleProductModel))
                    {
                        SimpleProductModel simple = (SimpleProductModel)resetItem;

                        if (Equals(itemName, simple.Name))
                        {
                            simple.AddedToCartQty = item.Qty;
                        }
                    }

                    if (resetItem.GetType() == typeof(ProductSizes))
                    {
                        ProductSizes size = (ProductSizes)resetItem;

                        if (Equals(itemName, $"{size.ItemName} {size.ProductSize}"))
                        {
                            size.AddedToCartQty = item.Qty;
                        }
                    }
                }
            }
            
            SubTotalFunc();
            StopDispatcherIfCartListIsEmpty();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    private void PrepareDataForPrinting(FBRResponseModel response)
    {
     
        
            InvoiceReportDataSource.Add(new TransactionModel
                        {
                            Narration = Transaction.Narration,
                            InvoiceDate = Transaction.InvoiceDate,
                            InvoiceTime = Transaction.InvoiceTime,
                            SubTotal = Transaction.SubTotal,
                            DiscountValue = Transaction.DiscountValue,
                            DiscountPrice = Transaction.DiscountPrice,
                            GSTValue = Transaction.GSTValue,
                            GSTPrice = Transaction.GSTPrice,
                            Cash = Transaction.Cash,
                            Change = Transaction.Change,
                            AdditionalCharges = Transaction.AdditionalCharges,
                            GrandTotal = Transaction.GrandTotal,
                            IsPaid = Transaction.IsPaid,
                            Cashier = Transaction.Cashier,
                            TransactionDetail = SetTransactionDetailValuesForPrinting(),
                            CompanyDetails = SetCompanyDetails(),
                            FBRResponse = response,
                            FBRImageSource = @"Resources\FbrPosImage.png",
                            IsFBRInvoice = Transaction.IsFBRInvoice,
                            DiscountOnTotal = DiscountAndTaxOnTotal,
                            TransactionType = TransactionType.Sale
                        });
    }
    private ObservableCollection<TransactionDetailsModel> SetTransactionDetailValuesForPrinting()
    {
        ObservableCollection<TransactionDetailsModel> items = new();
        foreach (var item in CartList)
        {
            items.Add(new TransactionDetailsModel
            {
                TransactionID = NewOrder,
                Name = item.Name,
                Qty = item.Qty,
                Price = item.SalePrice,
                Discount = item.Discount,
                Tax = item.Tax,
                Total = item.Total,
                IsVariantProduct = item.IsVariantProduct,
                ProductID = item.ProductID,
                SizeID = item.SizeID
            });
        }
        return items;
    }

    private async Task PrepareDataForPrintingForViewing(TransactionModel obj, string type)
    {
        var transactionDetails = await SetTransactionDetailValuesForViewing(obj.TransactionID, type);
        var fbrResponse = await SetFBRResponse(obj, type);

        PrintInvoiceSource.Add
                    (new TransactionModel
                    {
                        TransactionID = obj.TransactionID,
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
                        TransactionDetail = transactionDetails,
                        CompanyDetails = SetCompanyDetails(),
                        FBRResponse = fbrResponse,
                        FBRImageSource = @"Resources\FbrPosImage.png",
                        IsFBRInvoice = obj.IsFBRInvoice,
                        DiscountOnTotal = obj.DiscountOnTotal,
                        TransactionType = TransactionType.Sale,
                    });
    }

    private async Task<ObservableCollection<TransactionDetailsModel>> SetTransactionDetailValuesForViewing(int id, string type)
    {

        if (string.Equals(type, "live"))
        {
            TransactionDetails = await _saleReportService.GetTransactionsDetailsAsync(id);
        }
        else if (string.Equals(type, "deleted"))
        {
            TransactionDetails = await _saleReportService.GetDeletedTransactionsDetailsAsync(id);
        }

        ObservableCollection<TransactionDetailsModel> items = new();

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
                IsVariantProduct = item.IsVariantProduct,
                ProductID = item.ProductID,
                SizeID = item.SizeID


            });
        }

        return items;
    }

   
    



    private async Task Reset()
    {
        DineInCheck = false;
        TakeAwayCheck = true;
        HomeDeliveryCheck = false;
        
        CartList.Clear();
        
        SelectedCustomer = null;
        CustomerDetails = "";
       
        InvoiceReportDataSource.Clear();
        
        Transaction = new();
        LastRow = _newSaleService.GetLasRow() + 1;
        AddedAdditionalCharges.Clear();
        AdditionalCharges = 0;
        OrderNote = "";
        IsPreviousOrdersViewVisible = false;
        SubTotalFunc();
        ExecuteSetTakwAway();
        CheckingSettings();
        StopDispatcherIfCartListIsEmpty();
        await GetTodaySale();
        ResetItems();
        SetSectionHeader(IsSaleSettingsVisible);
    }

    private void ResetItems()
    {
        foreach (var item in ItemsToReset)
        {
            if (item.GetType() == typeof(SimpleProductModel))
            {
                SimpleProductModel simple = (SimpleProductModel)item;
                simple.AddedToCartQty = 0;
            }
            else if (item.GetType() == typeof(ProductSizes))
            {
                ProductSizes size = (ProductSizes)item;
                size.AddedToCartQty = 0;
            }
        }
    }


    private ObservableCollection<CompanyModel> SetCompanyDetails()
    {
        ObservableCollection<CompanyModel> companyModel = new();
        companyModel.Add(new CompanyModel
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
        });
        return companyModel;
    }

    private async Task<FBRResponseModel>? SetFBRResponse(TransactionModel obj, string type)
    {
        if (obj.IsFBRInvoice)
        {

            if (string.Equals(type, "live"))
            {
                return await _saleReportService.GetFBRResponseAsync(obj.TransactionID);
            }
            else if (string.Equals(type, "deleted"))
            {
                return await _saleReportService.GetDeletedFBRResponseAsync(obj.TransactionID);
            }
            return null;
        }
        else
        {
            return null;
        }
    }

    private void SetTransactionValues()
    {
        Transaction.Cashier = CurrentUser.Username;
        Transaction.PaymentType = SelectedPaymentType.PaymentType;
        Transaction.InvoiceTime = DateTime.Now.ToString("h:mm:ss tt");
        Transaction.DiscountOnTotal = DiscountAndTaxOnTotal;
        Transaction.AdditionalCharges = AdditionalCharges;

        if (SelectedCustomer is not null)
        {
            Transaction.CustomerID = SelectedCustomer.Id;
        }
        else
        {
            Transaction.CustomerID = 0;
        }

        SetNarration();
    }

    private void SetNarration()
    {
        Transaction.Narration = $"{OrderType}: {Environment.NewLine}{Environment.NewLine}{FormatCustomerDetails()}{OrderNote}";
    }
    private string FormatCustomerDetails()
    {
        if (!string.IsNullOrWhiteSpace(OrderNote))
        {
            return $"{CustomerDetails}\n\n";
        }
        else
        {
            return $"{CustomerDetails}";
        }
    }


    private FBRInvoiceModel SetValueToFBRModel()
    {
        FBRInvoiceModel fBRInvoiceModel = new FBRInvoiceModel();
        fBRInvoiceModel.InvoiceNumber = string.Empty;
        fBRInvoiceModel.POSID = FBRPrameterModel.POSID;
        fBRInvoiceModel.USIN = (LastRow + 1).ToString();
        fBRInvoiceModel.RefUSIN = string.Empty;
        fBRInvoiceModel.DateTime = DateTime.Now;
        fBRInvoiceModel.BuyerName = SelectedCustomer?.Name ?? "";
        fBRInvoiceModel.BuyerNTN = SelectedCustomer?.Ntn ?? "";
        fBRInvoiceModel.BuyerCNIC = SelectedCustomer?.Cnic ?? "";
        fBRInvoiceModel.BuyerPhoneNumber = SelectedCustomer?.Phone ?? "";
        fBRInvoiceModel.TotalQuantity = Convert.ToDouble(CartList.Count); ;
        fBRInvoiceModel.TotalSaleValue = Convert.ToDouble(Transaction.SubTotal);
        fBRInvoiceModel.TotalTaxCharged = Convert.ToDouble(CartList.Sum(a => a.DiscountPrice));
        fBRInvoiceModel.Discount = Convert.ToDouble(Transaction.DiscountValue);
        fBRInvoiceModel.FurtherTax = 0;
        fBRInvoiceModel.TotalBillAmount = Convert.ToDouble(Transaction.GrandTotal);
        fBRInvoiceModel.PaymentMode = 1;
        fBRInvoiceModel.InvoiceType = 1;
        fBRInvoiceModel.Items = GetFBRInvoiceItems();
        return fBRInvoiceModel;
    }

    private void ShowDeletedTransactionView()
    {
        ShowTransactions = false;
        ShowMainView = false;
        ShowDeletedTransactions = true;
    }
    private void StopDispatcherIfCartListIsEmpty()
    {
        if (CartList.Count == 0)
        {
            dt.Stop();
            CartListIsEmpty = true;
        }
    }

    private void CreatePreviousOrdersSummary()
    {
        PreviousOrdersCount = PreviousOrdersList.Count();
        FBRInvoicesCountInPreviousOrders = PreviousOrdersList.Count(x => x.IsFBRInvoice);
        TotalAmountSumForPreviousOrders = PreviousOrdersList.Sum(x => x.GrandTotal);

    }

    #endregion Private Functions
}