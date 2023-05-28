namespace CounterPOS.Common;

public interface IEasyPrinting
{
    void PrintEasily(object reportDataSource, InvoiceType invoiceType, bool showInvoice, bool areWePrinting, bool isPaid = false, short printCopiesFromUser = 2);
}