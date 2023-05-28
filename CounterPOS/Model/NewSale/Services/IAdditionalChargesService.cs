namespace CounterPOS.Model;

public interface IAdditionalChargesService
{
    ObservableCollection<AdditionalChargesModel> GetAdditionalChargesAsync();
    Task<bool> UpdateAdditionalCharges(ObservableCollection<AdditionalChargesModel> objAdditionalCharges);
}