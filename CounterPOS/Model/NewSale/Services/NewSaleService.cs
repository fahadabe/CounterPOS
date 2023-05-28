namespace CounterPOS.Model;

public class NewSaleService : INewSaleService
{
    private readonly IDialogMessages _messages;

    public NewSaleService(IDialogMessages messages)
    {
        _messages = messages;
    }

    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_CreateProductMenuAsync = "Select (Category) From tblCategory";
    private readonly string Q_GetAllProductsWithCategory = "Select * From tblProducts where Category = @category AND IsAvailable = " + 1;
    private readonly string Q_GetProductSizes = "SELECT SizeID, ProductSize, CAST(PurchasePrice AS REAL) as PurchasePrice, CAST(SalePrice AS REAL) as SalePrice, Identifier, PCTCode, PurchaseQuantity FROM tblProductSize WHERE ProductID = @productID";
    private readonly string Q_GetAllProducts = "Select * From tblProducts where IsAvailable = " + 1;
    private readonly string Q_GetLatestRowFromDatabase = "SELECT max(TransactionID) from tblSaleTransactions";
    private readonly string Q_GetLasRow = "SELECT max(TransactionID) from tblSaleTransactions";
    private readonly string Q_InsertTransactionAsync1 = "INSERT INTO tblSaleTransactions (CustomerID, Narration, InvoiceDate, InvoiceTime, SubTotal, DiscountValue, DiscountPrice, GSTValue, GSTPrice, AdditionalCharges, GrandTotal, Cash, Change, PaymentType, IsPaid, Cashier, IsFBRInvoice, DiscountOnTotal, TransactionType) VALUES (@a,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n,@o,@p,@q,@r,@s,@u)";
    private readonly string Q_InsertTransactionAsync2 = "INSERT INTO tblSaleTransactionsDetails (TransactionID, Name, Qty, Price, Discount, Tax, Total, IsVariantProduct, ProductID) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i)";
    private readonly string Q_InsertTransactionAsync3 = "INSERT INTO tblFBRResponse (TransactionID, InvoiceNumber, Code, Response, Errors) VALUES (@a,@b,@c,@d,@e)";
    private readonly string Q_InsertTransactionAsync4 = "UPDATE tblProductSize SET TimesSold = TimesSold + @a WHERE ProductID = @b AND SizeID = @c";
    private readonly string Q_InsertTransactionAsync5 = "UPDATE tblProducts SET TimesSold = TimesSold + @a WHERE ID = @b";
    private readonly string Q_GetDeletedTransactions = "SELECT * from tblDeletedSales ORDER by TransactionID DESC";
    private readonly string Q_DeleteTransactionPermanently1 = "DELETE FROM tblDeletedSales WHERE TransactionID = @a";
    private readonly string Q_DeleteTransactionPermanently2 = "DELETE FROM tblDeletedSaleDetails WHERE TransactionID = @a";
    private readonly string Q_DeleteAnSaveTransactionAsync1 = "DELETE FROM tblFBRResponse WHERE TransactionID = @a";
    private readonly string Q_DeleteAnSaveTransactionAsync2 = "DELETE FROM tblSaleTransactions WHERE TransactionID = @a";
    private readonly string Q_DeleteAnSaveTransactionAsync3 = "DELETE FROM tblSaleTransactionsDetails WHERE TransactionID = @a";
    private readonly string Q_DeleteAnSaveTransactionAsync4 = "INSERT INTO tblDeletedSales (TransactionID, CustomerID, Narration, InvoiceDate, InvoiceTime, SubTotal, DiscountValue, DiscountPrice, GSTValue, GSTPrice, AdditionalCharges, GrandTotal, Cash, Change, PaymentType, IsPaid, Cashier, IsFBRInvoice, DiscountOnTotal, TransactionType) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n,@o,@p,@q,@r,@s,@t)";
    private readonly string Q_DeleteAnSaveTransactionAsync5 = "INSERT INTO tblDeletedSaleDetails (TransactionID, Name, Qty, Price, Discount, Tax, Total, IsVariantProduct, ProductID) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i)";
    private readonly string Q_DeleteAnSaveTransactionAsync6 = "UPDATE tblProductSize SET TimesSold = TimesSold - @a WHERE ProductID = @b AND ProductSize = @c";
    private readonly string Q_DeleteAnSaveTransactionAsync7 = "UPDATE tblProducts SET TimesSold = TimesSold - @a WHERE ID = @b";
    private readonly string Q_DeleteAnSaveTransactionAsync8 = "INSERT INTO tblDeletedFBRResponse (TransactionID, InvoiceNumber, Code, Response, Errors) VALUES (@a,@b,@c,@d,@e)";
    private readonly string Q_GetFBRResponse = "Select * FROM tblFBRResponse WHERE TransactionID = @a";
    private readonly string Q_GetTransationDetails = "Select * FROM tblSaleTransactionsDetails WHERE TransactionID = @a";
    private int LatestRow;

    private byte[] ConvertToByte(object blob)
    {
        if (blob == DBNull.Value || blob == null)
        {
            return new byte[0];
        }
        else
        {
            return (byte[])blob;
        }
    }

    #region Use these methods if using tabcontrol to show products, use your own logic(in xaml)

    public async Task CreateProductMenuAsync(ObservableCollection<Tabs> tabCollection)
    {
        try
        {
            IEnumerable<string> tabs;
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            tabs = await db.QueryAsync<string>(Q_CreateProductMenuAsync);

                foreach (var item in tabs.ToList())
                {
                    tabCollection.Add(new Tabs { Header = item, Data = await GetProducts(item, db) });
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    public async Task<ObservableCollection<BaseProductModel>> GetAllProducts(string category, IDbConnection db)
    {
        try
        {
            var data = await db.QueryAsync<BaseProductModel>(Q_GetAllProductsWithCategory, new { category });
            return data.ToObservableCollection();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<BaseProductModel>();
        }
    }

    private async Task<ObservableCollection<BaseProductModel>> GetProducts(string CategoryName, IDbConnection db)
    {
        ObservableCollection<BaseProductModel> data = new();

        foreach (var item in await GetAllProducts(CategoryName, db))
        {
            if (item is not null)
            {
                if (item.IsVariantProduct)
                {
                    data.Add(new MultiSizeProductModel
                    {
                        ViewName = "MultiSizeProductUC",
                        ID = item.ID,
                        Name = item.Name,
                        SizeID = item.SizeID,
                        Picture = ConvertToByte(item.Picture),
                        //SalePrice = item.SalePrice,
                        //Discount = item.Discount,
                        //Tax = item.Tax,
                        //Identifier = item.Identifier,
                        //PCTCode = item.PCTCode,
                        //IsVariantProduct = true,

                        Category = item.Category,
                        ProductSizes = await AddProductSizes(
                            item.ID,
                            ConvertToByte(item.Picture),
                            item.Name,
                            item.Discount,
                            item.Tax,
                            item.Category,
                            db)
                    }
                    );
                }
                else
                {
                    data.Add(new SimpleProductModel
                    {
                        ViewName = "SimpleProductUC",
                        Name = item.Name,
                        Picture = ConvertToByte(item.Picture),
                        SalePrice = item.SalePrice,
                        Discount = item.Discount,
                        Tax = item.Tax,
                        Identifier = item.Identifier,
                        PCTCode = item.PCTCode,
                        IsVariantProduct = false,
                        Category = item.Category,
                        PurchaseQuantity = item.PurchaseQuantity,
                        ID = item.ID
                    });
                }
            }
        }
        return data;
    }

    private async Task<ObservableCollection<ProductSizes>> AddProductSizes(
        int productID,
        byte[] picture,
        string name,
        decimal discount,
        decimal tax,
        string category,
        IDbConnection db
        )
    {
        try
        {
            ObservableCollection<ProductSizes> sizes = new();
            var collection = await GetProductSizes(db, productID);
            foreach (var item in collection)
            {
                sizes.Add(new ProductSizes
                {
                    Picture = picture,
                    ItemName = name,
                    ProductSize = item.ProductSize,
                    PurchasePrice = item.PurchasePrice,
                    SalePrice = item.SalePrice,
                    Discount = discount,
                    Tax = tax,
                    Identifier = item.Identifier,
                    PCTCode = item.PCTCode,
                    IsVariantProduct = true,
                    Category = category,
                    ProductID = productID,
                    SizeID = item.SizeID,
                    PurchaseQuantity = item.PurchaseQuantity,
                });
            }

            return sizes;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductSizes>();
        }
    }

    private async Task<ObservableCollection<ProductSizes>> GetProductSizes(IDbConnection db, int productID)
    {
        try
        {
            var collection = await db.QueryAsync<ProductSizes>(Q_GetProductSizes, new { productID });
            return collection.ToObservableCollection();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductSizes>();
        }
    }

    #endregion Use these methods if using tabcontrol to show products, use your own logic(in xaml)

    #region Use these methods if to show products in controls like combobox, datagrid etc

    public async Task<ObservableCollection<BaseProductModel>> GetAllProducts()
    {
        try
        {
            ObservableCollection<BaseProductModel> data = new();
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QueryAsync<BaseProductModel>(Q_GetAllProducts);
                foreach (var item in result.ToObservableCollection())
                {
                    if (item is not null)
                    {
                        if (item.IsVariantProduct)
                        {
                            ObservableCollection<ProductSizes> sizes;
                            sizes = await GetProductSizesList(item.ID, db);

                            foreach (var sizeItem in sizes)
                            {
                                data.Add(new BaseProductModel
                                {
                                    ID = item.ID,
                                    Name = $"{item.Name} {sizeItem.ProductSize}",
                                    Picture = ConvertToByte(item.Picture),
                                    Description = item.Description,
                                    SalePrice = sizeItem.SalePrice,
                                    PurchasePrice = sizeItem.PurchasePrice,
                                    Discount = item.Discount,
                                    Tax = item.Tax,
                                    Identifier = sizeItem.Identifier,
                                    PCTCode = sizeItem.PCTCode,
                                    IsVariantProduct = true,
                                    Category = item.Category,
                                    SizeID = sizeItem.SizeID,
                                    PurchaseQuantity = sizeItem.PurchaseQuantity
                                });
                            }
                        }
                        else
                        {
                            data.Add(new BaseProductModel
                            {
                                ID = item.ID,
                                Name = item.Name,
                                Picture = ConvertToByte(item.Picture),
                                SalePrice = item.SalePrice,
                                Discount = item.Discount,
                                Tax = item.Tax,
                                Identifier = item.Identifier,
                                PCTCode = item.PCTCode,
                                IsVariantProduct = false,
                                Category = item.Category,
                                PurchasePrice = item.PurchasePrice
                            });
                        }
                    }
                }
            
            return data;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<BaseProductModel>();
        }
    }

    private async Task<ObservableCollection<ProductSizes>> GetProductSizesList(int productID, IDbConnection db)
    {
        try
        {
            var sizes = await db.QueryAsync<ProductSizes>(Q_GetProductSizes, new { productID });
            return sizes.ToObservableCollection();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductSizes>();
        }
    }

    #endregion Use these methods if to show products in controls like combobox, datagrid etc

    public int GetLatestRowFromDatabase(IDbConnection db)
    {
        try
        {
            return LatestRow = db.ExecuteScalar<int>(Q_GetLatestRowFromDatabase);
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }

    public int GetLasRow()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return LatestRow = db.ExecuteScalar<int>(Q_GetLasRow);
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }

    public async Task<bool> InsertTransactionAsync(TransactionModel ObjTransactionModel, ObservableCollection<CartModel> ObjTransactionDetails, FBRResponseModel fBRResponseModel)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
            using IDbTransaction transaction = db.BeginTransaction();
                
                    try
                    {
                        await db.ExecuteAsync(Q_InsertTransactionAsync1, new
                        {
                            a = ObjTransactionModel.CustomerID,
                            c = ObjTransactionModel.Narration,
                            d = ObjTransactionModel.InvoiceDate.ToString(SQLiteDefaultDateFormat),
                            e = ObjTransactionModel.InvoiceTime,
                            f = ObjTransactionModel.SubTotal,
                            g = ObjTransactionModel.DiscountValue,
                            h = ObjTransactionModel.DiscountPrice,
                            i = ObjTransactionModel.GSTValue,
                            j = ObjTransactionModel.GSTPrice,
                            k = ObjTransactionModel.AdditionalCharges,
                            l = ObjTransactionModel.GrandTotal,
                            m = ObjTransactionModel.Cash,
                            n = ObjTransactionModel.Change,
                            o = ObjTransactionModel.PaymentType,
                            p = ObjTransactionModel.IsPaid,
                            q = ObjTransactionModel.Cashier,
                            r = ObjTransactionModel.IsFBRInvoice ? 1 : 0,
                            s = ObjTransactionModel.DiscountOnTotal ? 1 : 0,
                            u = ObjTransactionModel.TransactionType.ToString()
                        });

                        foreach (var item in ObjTransactionDetails)
                        {
                            await db.ExecuteAsync(Q_InsertTransactionAsync2, new
                            {
                                a = GetLatestRowFromDatabase(db),
                                b = item.Name,
                                c = item.Qty,
                                d = item.SalePrice,
                                e = item.Discount,
                                f = item.Tax,
                                g = item.Total,
                                h = item.IsVariantProduct,
                                i = item.ProductID
                            });
                        }

                        if (ObjTransactionModel.IsFBRInvoice)
                        {
                            await db.ExecuteAsync(Q_InsertTransactionAsync3, new
                            {
                                a = GetLatestRowFromDatabase(db),
                                b = fBRResponseModel.InvoiceNumber,
                                c = fBRResponseModel.Code,
                                d = fBRResponseModel.Response,
                                e = fBRResponseModel.Errors
                            });
                        }

                        foreach (var item in ObjTransactionDetails)
                        {
                            if (item.IsVariantProduct)
                            {
                                await db.ExecuteAsync(Q_InsertTransactionAsync4, new
                                {
                                    a = item.Qty,
                                    b = item.ProductID,
                                    c = item.SizeID
                                });
                            }
                            else
                            {
                                await db.ExecuteAsync(Q_InsertTransactionAsync5, new
                                {
                                    a = item.Qty,
                                    b = item.ProductID
                                });
                            }
                        }

                        transaction.Commit();
                        db.Close();
                        return transaction.ReturnSuccess();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        _messages.ShowErrorMessage(e.Message);
                        return false;
                    }
                
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<ObservableCollection<TransactionModel>> GetDeletedTransactions()
    {
        try
        {
        using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
        var collection = await db.QueryAsync<TransactionModel>(Q_GetDeletedTransactions);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionModel>();
        }
    }

    public async Task<bool> DeleteTransactionPermanently(ObservableCollection<TransactionModel> transactions)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
            using IDbTransaction transaction = db.BeginTransaction();
                
                    try
                    {
                        foreach (var item in transactions)
                        {
                            await db.ExecuteAsync(Q_DeleteTransactionPermanently1, new
                            {
                                a = item.TransactionID
                            });

                            await db.ExecuteAsync(Q_DeleteTransactionPermanently2, new
                            {
                                a = item.TransactionID
                            });
                        }

                        transaction.Commit();
                        if (transaction.ReturnSuccess())
                        {
                            return true;
                        }
                        else
                        {
                            _messages.ShowErrorNotification();
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        _messages.ShowErrorMessage(e.Message);
                        return false;
                    }
                    finally
                    {
                        db.Close();
                        transaction.Dispose();
                    }
                
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    //this deletes transaction from tblSaleTransactin and tblSaleTransactionDetails and save them in another table tblDeletedSales and tblDeletedSaleDetails
    public async Task<bool> DeleteAnSaveTransactionAsync(TransactionModel ObjTransactionModel)
    {

        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
                db.Open();
                using IDbTransaction transaction = db.BeginTransaction();
                
                    try
                    {
                        FBRResponseModel fBRResponse = new();
                        var transactionDetails = await GetTransationDetails(ObjTransactionModel.TransactionID, db);
                        if (ObjTransactionModel.IsFBRInvoice)
                        {
                            fBRResponse = await GetFBRResponse(ObjTransactionModel.TransactionID, db);

                            await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync1, new
                            {
                                a = ObjTransactionModel.TransactionID
                            });
                        }

                        await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync2, new
                        {
                            a = ObjTransactionModel.TransactionID
                        });

                        await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync3, new
                        {
                            a = ObjTransactionModel.TransactionID
                        });

                        await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync4, new
                        {
                            a = ObjTransactionModel.TransactionID,
                            b = ObjTransactionModel.CustomerID,
                            c = ObjTransactionModel.Narration,
                            d = ObjTransactionModel.InvoiceDate.ToString(SQLiteDefaultDateFormat),
                            e = ObjTransactionModel.InvoiceTime,
                            f = ObjTransactionModel.SubTotal,
                            g = ObjTransactionModel.DiscountValue,
                            h = ObjTransactionModel.DiscountPrice,
                            i = ObjTransactionModel.GSTValue,
                            j = ObjTransactionModel.GSTPrice,
                            k = ObjTransactionModel.AdditionalCharges,
                            l = ObjTransactionModel.GrandTotal,
                            m = ObjTransactionModel.Cash,
                            n = ObjTransactionModel.Change,
                            o = ObjTransactionModel.PaymentType,
                            p = ObjTransactionModel.IsPaid,
                            q = ObjTransactionModel.Cashier,
                            r = ObjTransactionModel.IsFBRInvoice ? 1 : 0,
                            s = ObjTransactionModel.DiscountOnTotal ? 1 : 0,
                            t = ObjTransactionModel.TransactionType.ToString()
                        });

                        foreach (var item in transactionDetails)
                        {
                            await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync5, new
                            {
                                a = item.TransactionID,
                                b = item.Name,
                                c = item.Qty,
                                d = item.Price,
                                e = item.Discount,
                                f = item.Tax,
                                g = item.Total,
                                h = item.IsVariantProduct,
                                i = item.ProductID
                            });

                            //minus timesSold

                            if (item.IsVariantProduct)
                            {

                                
                                string input = item.Name;
                                string[] words = input.Split(' ');
                                string productSize = words[words.Length - 1];

                                await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync6, new
                                {
                                    a = item.Qty,
                                    b = item.ProductID,
                                    c = productSize
                                });
                            }
                            else
                            {
                                await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync7, new
                                {
                                    a = item.Qty,
                                    b = item.ProductID
                                    

                                });
                            }
                        }

                        //delete fbr response for transaction and save in another table 

                        if (ObjTransactionModel.IsFBRInvoice)
                        {
                            await db.ExecuteAsync(Q_DeleteAnSaveTransactionAsync8, new
                            {
                                a = fBRResponse.TransactionID,
                                b = fBRResponse.InvoiceNumber,
                                c = fBRResponse.Code,
                                d = fBRResponse.Response,
                                e = fBRResponse.Errors
                            });
                        }

                        
                        transaction.Commit();
                        if (transaction.ReturnSuccess())
                        {
                            _messages.ShowSuccessNotification("Transaction Deleted");
                            return true;
                        }
                        else
                        {
                            _messages.ShowErrorNotification();
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        _messages.ShowErrorMessage(e.Message);
                        return false;
                    }
                    finally
                    {
                        db.Close();
                        transaction.Dispose();
                    }
                
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    private async Task<FBRResponseModel> GetFBRResponse(int transactionID, IDbConnection db)
    {
        var response = await db.QuerySingleAsync<FBRResponseModel>(Q_GetFBRResponse, new
        {
            a = transactionID
        });

        return response;
    }

    private async Task<ObservableCollection<TransactionDetailsModel>> GetTransationDetails(int transactionID, IDbConnection db)
    {
        try
        {
        var details = await db.QueryAsync<TransactionDetailsModel>(Q_GetTransationDetails, new
        {
            a = transactionID
        });

        return details.ToObservableCollection();


        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionDetailsModel>();
            
        }
    }

    public int ReturnLatestRow()
    {
        return LatestRow;
    }
}