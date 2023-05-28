namespace CounterPOS.Common;

public interface IImageHelper
{
    string BrowseImage();
    
    byte[] EditImageToBitmapToByte(object image, bool isImageChanged, object selectedPicture);
    byte[] NewBitmapToByte(System.Drawing.Image image);
    System.Drawing.Image NewImageToBitmap(string image);
}