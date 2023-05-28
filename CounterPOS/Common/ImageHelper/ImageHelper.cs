namespace CounterPOS.Common;

public class ImageHelper : IImageHelper
{
    private readonly IDialogMessages _messages;
    public ImageHelper(IDialogMessages messages)
    {
        _messages = messages;
    }

    public string BrowseImage()
    {
        try
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Title = "Select Product Image";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg; *.jpeg; *.jpe; *.png";
            if (dlg.ShowDialog() == true)
            {
                
                    return dlg.FileName.ToString();
               
            }
            return string.Empty;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return string.Empty;
        }
    }

    

    #region NewProduct

    public byte[] NewBitmapToByte(System.Drawing.Image image)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            // Convert Image to byte[]
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            byte[] imageBytes = ms.ToArray();
            return imageBytes;
        }
    }

    public System.Drawing.Image NewImageToBitmap(string image)
    {
        Bitmap newie = new Bitmap(1, 1);
        if (!string.IsNullOrWhiteSpace(image))
        {
            Bitmap bitmap = new(image);
            return bitmap;
        }
        return newie;
    }

    #endregion NewProduct

    #region EditProduct

    public byte[] EditImageToBitmapToByte(object image, bool isImageChanged, object selectedPicture)
    {
        Bitmap newie = new Bitmap(1, 1);
        if (isImageChanged)
        {
            if (!string.IsNullOrWhiteSpace(image.ToString()))
            {
                Bitmap bitmap = new(image.ToString());
                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    return imageBytes;
                }
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    newie.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    return imageBytes;
                }
            }
        }
        else
        {
            if (selectedPicture is null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    newie.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    return imageBytes;
                }
            }

            else if (string.IsNullOrWhiteSpace(selectedPicture.ToString()))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    newie.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    return imageBytes;
                }
            }


            return (System.Byte[])selectedPicture;
        }
    }

    #endregion EditProduct
}