namespace CounterPOS.Model;

public interface IFBRParametersService
{
    FBRParametersModel? GetFBRParametersAsync();
    Task<bool> UpdateFBRParametersAsync(FBRParametersModel objUpdateFBRParameters);
}