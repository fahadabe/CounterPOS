namespace CounterPOS.ViewModel;

public partial class MiscellaneousViewModel : ObservableObject
{
    private readonly IPaymentTypeService _paymentTypeService;
    public MiscellaneousViewModel(IPaymentTypeService paymentTypeService)
    {
        _paymentTypeService = paymentTypeService;
    }

    #region Commands

    public DelegateCommand SaveUserSettings => new DelegateCommand(ExecuteSaveUserSettings, CanExecuteSaveUserSettings);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public DelegateCommand SelectNewFontCommand => new DelegateCommand(ExecuteSelectNewFont);
    #endregion


    #region Properties


    [ObservableProperty]
    private bool _CreateUIMenu;
    [ObservableProperty]
    private UserModel? _CurrentUser = new();
    [ObservableProperty]
    private decimal _DefaultDiscountValue;
    [ObservableProperty]
    private decimal _DefaultGSTValue;
    [ObservableProperty]
    private bool _DiscountAndTaxOnTotal = true;
    [ObservableProperty]
    private bool _DiscountAndTaxPerItem;
    [ObservableProperty]
    private Font? _InvoiceWatermarkFont;
    [ObservableProperty]
    private int _InvoiceWatermarkImageTransparency;
    [ObservableProperty]
    private string? _InvoiceWatermarkText;
    [ObservableProperty]
    private int _InvoiceWatermarkTextTransparency;
    [ObservableProperty]
    private bool _IsFBRDefaultInvoice;
    [ObservableProperty]
    private ObservableCollection<PaymentTypeModel>? _PaymentTypeList;
    [ObservableProperty]
    private short _PrintCopies;
    [ObservableProperty]
    private double _RowFontSize;
    [ObservableProperty]
    private double _RowHeight;
    [ObservableProperty]
    private PaymentTypeModel? _SelectedPaymentType = new();
    [ObservableProperty]
    private bool _ShowCurrentSale;
    [ObservableProperty]
    private bool _ShowInvoice;
    [ObservableProperty]
    private bool _WatermarkPicture;
    [ObservableProperty]
    private bool _WatermarkText;


    partial void OnDiscountAndTaxOnTotalChanged(bool value)
    {
        if (DiscountAndTaxOnTotal)
        {
            DiscountAndTaxPerItem = false;
        }
    }
    
    partial void OnDiscountAndTaxPerItemChanged(bool value)
    {
        if (DiscountAndTaxPerItem)
        {
            DiscountAndTaxOnTotal = false;
        }
    }

    partial void OnWatermarkPictureChanged(bool value)
    {
        if (WatermarkPicture)
        {
            WatermarkText = false;
        }
    }

    partial void OnWatermarkTextChanged(bool value)
    {
        if (WatermarkText)
        {
            WatermarkPicture = false;
        }
    }

    #endregion

    #region Command Functions

    private bool CanExecuteSaveUserSettings()
    {
        return CurrentUser.EditSettings;
    }


    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
        CheckSettings();
    }

    private void ExecuteSaveUserSettings()
    {
        Settings.Default.DiscountAndTaxOnTotal = DiscountAndTaxOnTotal;
        Settings.Default.DiscountAndTaxPerItem = DiscountAndTaxPerItem;
        Settings.Default.DefaultDiscountValue = DefaultDiscountValue;
        Settings.Default.DefaultGSTValue = DefaultGSTValue;
        Settings.Default.ShowInvoiceOnCreating = ShowInvoice;
        Settings.Default.CreateUIMenuAtStartup = CreateUIMenu;
        Settings.Default.IsDefaultFBRInvoice = IsFBRDefaultInvoice;
        Settings.Default.PrintCopies = PrintCopies;
        Settings.Default.PictureWatermark = WatermarkPicture;
        Settings.Default.TextWatermark = WatermarkText;
        Settings.Default.InvoiceWatermarkText = InvoiceWatermarkText;
        Settings.Default.InvoiceWatermarkFont = InvoiceWatermarkFont;
        Settings.Default.InvoiceWatermarkTextTransparency = InvoiceWatermarkTextTransparency;
        Settings.Default.InvoiceWatermarkImageTransparency = InvoiceWatermarkImageTransparency;
        Settings.Default.DefaultPaymentType = SelectedPaymentType.PaymentType;
        Settings.Default.RowHeight = RowHeight;
        Settings.Default.RowFontSize = RowFontSize;
        Settings.Default.ShowCurrentSale = ShowCurrentSale;
        Settings.Default.Save();
    }

    private void ExecuteSelectNewFont()
    {
        System.Windows.Forms.FontDialog fd = new();
        fd.ShowEffects = false;
        fd.ShowHelp = false;
        fd.ScriptsOnly = false;
        fd.MaxSize = 60;
        fd.MinSize = 20;
        var result = fd.ShowDialog();
        if (result == System.Windows.Forms.DialogResult.OK)
        {
            InvoiceWatermarkFont = fd.Font;
        }
    }
    #endregion
    private void CheckSettings()
    {
        DiscountAndTaxOnTotal = Settings.Default.DiscountAndTaxOnTotal;
        DiscountAndTaxPerItem = Settings.Default.DiscountAndTaxPerItem;
        DefaultDiscountValue = Settings.Default.DefaultDiscountValue;
        DefaultGSTValue = Settings.Default.DefaultGSTValue;
        ShowInvoice = Settings.Default.ShowInvoiceOnCreating;
        CreateUIMenu = Settings.Default.CreateUIMenuAtStartup;
        IsFBRDefaultInvoice = Settings.Default.IsDefaultFBRInvoice;
        PrintCopies = Settings.Default.PrintCopies;
        WatermarkPicture = Settings.Default.PictureWatermark;
        WatermarkText = Settings.Default.TextWatermark;
        InvoiceWatermarkText = Settings.Default.InvoiceWatermarkText;
        InvoiceWatermarkFont = Settings.Default.InvoiceWatermarkFont;
        InvoiceWatermarkTextTransparency = Settings.Default.InvoiceWatermarkTextTransparency;
        InvoiceWatermarkImageTransparency = Settings.Default.InvoiceWatermarkImageTransparency;
        RowHeight = Settings.Default.RowHeight;
        RowFontSize = Settings.Default.RowFontSize;
        ShowCurrentSale = Settings.Default.ShowCurrentSale;

        SelectedPaymentType = PaymentTypeList.FirstOrDefault(i => i.PaymentType == Settings.Default.DefaultPaymentType);
    }
}