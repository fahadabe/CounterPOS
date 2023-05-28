namespace CounterPOS.ViewModel;

public partial class PaymentTypeViewModel : ObservableObject
{
    private readonly IPaymentTypeService _paymentTypeService;
    private readonly IDialogMessages _messages;
    public PaymentTypeViewModel(IPaymentTypeService paymentTypeService, IDialogMessages messages)
    {
        _paymentTypeService = paymentTypeService;
        _messages = messages;
    }

    #region Commands

    public AsyncCommand AddPaymentTypeCommand => new AsyncCommand(ExecuteAddPaymentType, CanExecuteAddPaymentType);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public AsyncCommand RefreshPaymentTypeCommand => new AsyncCommand(ExecuteRefreshPaymentTypes);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private UserModel? _CurrentUser = new();
    [ObservableProperty]
    private PaymentTypeModel? _NewPaymentType = new();
    [ObservableProperty]
    private ObservableCollection<PaymentTypeModel>? _PaymentTypeList;

    

    

    #endregion Properties

    #region Command Functions

    private bool CanExecuteAddPaymentType()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private async Task ExecuteAddPaymentType()
    {
        if (!string.IsNullOrWhiteSpace(NewPaymentType.PaymentType))
        {
            if (!_paymentTypeService.CheckIfPaymentTypeExist(NewPaymentType.PaymentType))
            {
                await _paymentTypeService.AddPaymentTypeAsync(NewPaymentType);
                PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
            }
            else
            {
                _messages.ShowInfoNotification($"{NewPaymentType.PaymentType} already exist.");
            }
        }
        else
        {
            _messages.ShowInfoNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
    }

    private async Task ExecuteRefreshPaymentTypes()
    {
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
    }

    #endregion Command Functions
}