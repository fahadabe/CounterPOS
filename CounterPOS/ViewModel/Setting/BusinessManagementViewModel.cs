namespace CounterPOS.ViewModel;

public partial class BusinessManagementViewModel : ObservableObject
{
    private readonly ICompanyService _companyService;
    private readonly IImageHelper _imageHelper;
    private readonly IDialogMessages _messages;

    [ObservableProperty]
    private CompanyModel _CompanyDetails;

    [ObservableProperty]
    private ObservableCollection<string> _CountryList;

    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private object? _SelectedLogo;

    private bool isImageChanged = false;

    public BusinessManagementViewModel(ICompanyService companyService, IImageHelper imageHelper, IDialogMessages messages)
    {
        _companyService = companyService;
        _imageHelper = imageHelper;
        _messages = messages;
    }
    public DelegateCommand BrowseCompanyLogoCommand => new DelegateCommand(ExecuteBrowseCompanyLogo);

    public DelegateCommand LoadDataCommand => new DelegateCommand(ExecuteLoadData);

    public DelegateCommand RemoveCompanyLogoCommand => new DelegateCommand(ExecuteRemoveCompanyLogo);

    public AsyncCommand SaveCompanyRecordCommand => new AsyncCommand(ExecuteInsertNewCompanyRecord, CanExecuteSaveCompanyInfo);

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

    private bool CanExecuteSaveCompanyInfo()
    {
        return CurrentUser.EditSettings;
    }

    private void ExecuteBrowseCompanyLogo()
    {
        var path = _imageHelper.BrowseImage();
        if (!string.IsNullOrWhiteSpace(path))
        {
            SelectedLogo = path;
            isImageChanged = true;
        }
        else
        {
            isImageChanged = false;
        }
    }

    private async Task ExecuteInsertNewCompanyRecord()
    {
        if (!string.IsNullOrWhiteSpace(CompanyDetails.Name))
        {
            if (await _companyService.UpdateCompanyInfoAsync(CompanyDetails, SelectedLogo.ToString(), isImageChanged, SelectedLogo))
            {
                CommonProperties.CompanyDetails = _companyService.GetCompanyInfo();
                CompanyDetails = CommonProperties.CompanyDetails;
                isImageChanged = false;
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private void ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        CountryList = GetCountryList();
    }

    private void ExecuteRemoveCompanyLogo()
    {
        SelectedLogo = string.Empty;
        isImageChanged = false;
    }

    partial void OnCompanyDetailsChanged(CompanyModel value)
    {
        if (CompanyDetails is not null)
        {
            SelectedLogo = CompanyDetails.Logo;
        }
    }
}