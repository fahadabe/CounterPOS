namespace CounterPOS.Common;

public interface IDialogMessages
{
    Task<bool> AskUser(string message);
   
    
    void ShowErrorMessage(string message = "Something went wrong, Try again.");
    void ShowErrorNotification(string message = "Something went wrong, Try again.");
    void ShowInfoNotification(string message);
    
    void ShowSuccessNotification(string message);
    void ShowWarningNotification(string message);
}