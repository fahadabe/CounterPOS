using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;

namespace CounterPOS.Common;

public class EasyPrinting : IEasyPrinting
{
    public void PrintEasily(object reportDataSource, InvoiceType invoiceType, bool showInvoice, bool shouldPrint, bool isPaid = false, short printCopiesFromUser = 0)
    {
        try
        {

        
        Settings.Default.Reload();

        bool textWatermark = Settings.Default.TextWatermark;
        bool pictureWatermark = Settings.Default.PictureWatermark;
        string invoiceWatermarkText = Settings.Default.InvoiceWatermarkText;
        Font invoiceWatermarkTextFontSize = Settings.Default.InvoiceWatermarkFont;
        int textTransparency = Settings.Default.InvoiceWatermarkTextTransparency;
        int imageTransparency = Settings.Default.InvoiceWatermarkImageTransparency;
        short printCopiesFromSettings = Settings.Default.PrintCopies;

        short copiesToPrint;
        if (printCopiesFromUser <= 0)
        {
            copiesToPrint = printCopiesFromSettings;
        }
        else
        {
            copiesToPrint = printCopiesFromUser;
        }

        var xtraReport = new XtraReport 
        {
            ReportUnit = ReportUnit.HundredthsOfAnInch,
            PaperKind = System.Drawing.Printing.PaperKind.Custom,
            RollPaper = true,
        };
         
            if (invoiceType == InvoiceType.SaleInvoice)
        {
            xtraReport.LoadLayout(@"Resources\Reports\SaleInvoice.repx");
        }
        else if (invoiceType == InvoiceType.PurchaseInvoice)
        {
            xtraReport.LoadLayout(@"Resources\Reports\PurchaseInvoice.repx");
        }
        else if (invoiceType == InvoiceType.ItemReportInvoice)
        {
            xtraReport.LoadLayout(@"Resources\Reports\ItemReport.repx");
        }

        xtraReport.DataSource = reportDataSource;

        if (isPaid)
        {
            xtraReport.DrawWatermark = true;
            if (textWatermark)
            {
                SetTextWatermark(xtraReport, invoiceWatermarkText, invoiceWatermarkTextFontSize, textTransparency);
            }
            else if (pictureWatermark)
            {
                SetPictureWatermark(xtraReport, imageTransparency);
            }
        }

        xtraReport.CreateDocument(true);


        if (shouldPrint)
        {
            PrintToolBase pTool = new PrintToolBase(xtraReport.PrintingSystem);
            pTool.PrinterSettings.Copies = copiesToPrint;
            pTool.Print();
        }


        if (showInvoice)
        {
            InvoiceViewer invoiceViewer = new(xtraReport);
            invoiceViewer.ShowDialog();
        }
        }
        catch (Exception e)
        {

            
        }

    }

    private void SetPictureWatermark(XtraReport report, int imageTransaparency)
    {
        Watermark pictureWatermark = new();
        pictureWatermark.ImageSource = DevExpress.XtraPrinting.Drawing.ImageSource.FromFile(@"Resources\paid.png");
        pictureWatermark.ImageAlign = ContentAlignment.MiddleCenter;
        pictureWatermark.ImageAlign = ContentAlignment.TopLeft;
        pictureWatermark.ImageTiling = false;
        pictureWatermark.ImageViewMode = ImageViewMode.Zoom;
        pictureWatermark.ImageTransparency = imageTransaparency;
        pictureWatermark.ShowBehind = true;
        report.Watermark.CopyFrom(pictureWatermark);
    }

    private void SetTextWatermark(XtraReport report, string watermarktext, Font invoiceWatermarkTextFontSize, int textTransparency)
    {
        Watermark textWatermark = new();
        textWatermark.Text = watermarktext;
        textWatermark.TextDirection = DirectionMode.ForwardDiagonal;
        textWatermark.ShowBehind = true;
        textWatermark.Font = invoiceWatermarkTextFontSize;
        textWatermark.TextTransparency = textTransparency;
        report.Watermark.CopyFrom(textWatermark);
    }
}