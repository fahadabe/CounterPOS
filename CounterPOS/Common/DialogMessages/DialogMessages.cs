using HandyControl.Controls;
using HandyControl.Data;

namespace CounterPOS.Common;

public class DialogMessages : IDialogMessages
{
    //private Random random = new Random();
    
    //private List<string> errorList = new List<string> { "Error", "Oops", "Oh no...", "Failed", "Uh-oh!", "Whoops" };
    //private List<string> successList = new List<string> { "Success", "Yoo!", "Cheers", "Congratulations", "Good Job" };

    //public string RandomErrorTitle()
    //{
    //    int index = random.Next(errorList.Count);
    //    return errorList[index];
    //}

    //public string RandomSuccessTitle()
    //{
    //    int index = random.Next(successList.Count);
    //    return successList[index];
    //}

    public void ShowErrorMessage(string message = "Something went wrong, Try again.")
    {
        Growl.ErrorGlobal(message);
        //XMessageBox.Show(RandomErrorTitle(), message, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    //public  void ShowSuccessMessage(string message)
    //{
    //    XMessageBox.Show(RandomSuccessTitle(), message, MessageBoxButton.OK, MessageBoxImage.Information);
    //}

    //public  void ShowInfoMessage(string message, string title = "Information")
    //{
    //    XMessageBox.Show(title, message, MessageBoxButton.OK, MessageBoxImage.Information);
    //}

    //public  void ShowWarningMessage(string message, string title = "Warning")
    //{
    //    XMessageBox.Show(title, message, MessageBoxButton.OK, MessageBoxImage.Warning);
    //}

    public Task<bool> AskUser(string message)
    {
        var tcs = new TaskCompletionSource<bool>();
        Growl.AskGlobal(new GrowlInfo
        {
            Message = message,
            ShowDateTime = false,
            ActionBeforeClose = isConfirmed =>
            {
                tcs.SetResult(isConfirmed);
                return true;
            }
        });
        return tcs.Task;
    }
    //var messageBoxResult = XMessageBox.Show("Confirmation", message, MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

    //if (messageBoxResult == MessageBoxResult.Yes)
    //{
    //    return true;
    //}
    //else
    //{
    //    return false;
    //}
    public void ShowSuccessNotification(string message)
    {
        var growlInfo = new GrowlInfo()
        {
            WaitTime = 1,
            Message = message,
            ShowDateTime = false
        };
        Growl.SuccessGlobal(growlInfo);
    }

    public void ShowErrorNotification(string message = "Something went wrong, Try again.")
    {
        var growlInfo = new GrowlInfo()
        {
            WaitTime = 1,
            Message = message,
            ShowDateTime = false,
        };
        Growl.ErrorGlobal(growlInfo);
    }

    public void ShowInfoNotification(string message)
    {
        var growlInfo = new GrowlInfo()
        {
            WaitTime = 1,
            Message = message,
            ShowDateTime = false,
        };
        Growl.InfoGlobal(growlInfo);
    }

    public void ShowWarningNotification(string message)
    {
        var growlInfo = new GrowlInfo()
        {
            WaitTime = 1,
            Message = message,
            ShowDateTime = false,
        };
        Growl.WarningGlobal(growlInfo);
    }

}