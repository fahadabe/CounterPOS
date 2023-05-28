
using DevExpress.CodeParser;

namespace CounterPOS.Model;

public class ProductService : IProductService
{
    private readonly IDialogMessages _messages;
    private readonly IImageHelper _imageHelper;
    public ProductService(IDialogMessages messages, IImageHelper imageHelper)
    {
        _messages = messages;
        _imageHelper = imageHelper;
    }
    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_AddProductAsync = "INSERT INTO tblProducts(Name, Description, PurchasePrice , SalePrice, Category, Picture, AddedDate, Identifier, PCTCode, IsVariantProduct, IsAvailable, Discount, Tax, PurchaseQuantity) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n)";
    private readonly string Q_AddSizedProductAsync1 = "INSERT INTO tblProducts (Name, Description, PurchasePrice , SalePrice, Category, Picture, AddedDate, Identifier, PCTCode,  IsVariantProduct, IsAvailable, Discount, Tax, PurchaseQuantity) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n)";
    private readonly string Q_AddSizedProductAsync2 = "INSERT INTO tblProductSize (ProductID, ProductSize, PurchasePrice ,SalePrice, Category, PCTCode, Identifier, Tax, Discount, PurchaseQuantity) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j)";
    private readonly string Q_CheckIfProductExistAsync = "SELECT count(*) FROM tblProducts WHERE Name = @productName";
    private readonly string Q_DeleteProductAsync1 = "DELETE FROM tblProducts WHERE Id = @a";
    private readonly string Q_DeleteProductAsync2 = "DELETE FROM tblProductSize WHERE ProductID = @a";
    private readonly string Q_DeleteProductAsync3 = "DELETE FROM tblProducts WHERE ID = @a";
    private readonly string Q_GetAllProductsAsync = "Select * From tblProducts";
    private readonly string Q_GetLastRowIDAsync = "SELECT max(ID) from tblProducts";
    private readonly string Q_GetSizesForSelectedProduct = "Select * From tblProductSize WHERE ProductID = @productId";
    private readonly string Q_GetProductPerformance = "Select TransactionID, Name as ProductName, Qty, CAST(Price AS REAL) as Price, CAST(Discount AS REAL) as Discount, CAST(Tax AS REAL) as Tax, CAST(Total AS REAL) as Total From tblSaleTransactionsDetails where Name = @productName";
    private readonly string Q_GetDateForTransaction = "SELECT InvoiceDate FROM tblSaleTransactions WHERE TransactionID = @transactionID";
    private readonly string Q_UpdateProductAsync = "UPDATE tblProducts SET Name = @a, Description= @b, PurchasePrice = @c, SalePrice= @d, Category= @e, Picture= @f, AddedDate= @g, Identifier= @h, PCTCode= @i, IsVariantProduct= @j, IsAvailable= @k, Discount= @l, Tax= @m, PurchaseQuantity = @n WHERE ID = @id";
    private readonly string Q_UpdateSizedProductAsync1 = "UPDATE tblProducts SET Name = @a, Description= @b, PurchasePrice = @c, SalePrice= @d, Category= @e, Picture= @f, AddedDate= @g, Identifier= @h, PCTCode= @i, IsVariantProduct= @j, IsAvailable= @k, Discount= @l, Tax= @m, PurchaseQuantity = @n WHERE ID = @id";
    private readonly string Q_UpdateSizedProductAsync2 = "INSERT INTO tblProductSize (ProductID, ProductSize, PurchasePrice ,SalePrice, Category, TimesSold, PurchaseQuantity, PCTCode, Identifier, Tax, Discount) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k)";
    private readonly string Q_GetTransactionAsync = "SELECT * FROM tblSaleTransactions WHERE TransactionID = @transactionID";
    private readonly string Q_GetProductSizesFromDatabaseAsync = "SELECT ProductSize, TimesSold FROM tblProductSize WHERE ProductID = @productID";
    private readonly string Q_DeleteSizesFromDatabaseAsync = "DELETE FROM tblProductSize WHERE ProductID = @productID";
    private readonly string Q_GetLastProductID = "SELECT max(ID) from tblProducts";
    public async Task<bool> AddProductAsync(ProductModel ObjNewProduct, string picture)
    {
        try
        {
            var result = 0;
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            result = await db.ExecuteAsync(Q_AddProductAsync, new
                {
                    a = ObjNewProduct.Name,
                    b = ObjNewProduct.Description,
                    c = ObjNewProduct.PurchasePrice,
                    d = ObjNewProduct.SalePrice,
                    e = ObjNewProduct.Category,
                    f = _imageHelper.NewBitmapToByte(_imageHelper.NewImageToBitmap(picture)),
                    g = ObjNewProduct.AddedDate.ToString(SQLiteDefaultDateFormat),
                    h = ObjNewProduct.Identifier,
                    i = ObjNewProduct.PCTCode,
                    j = ObjNewProduct.IsVariantProduct ? 1 : 0,
                    k = ObjNewProduct.IsAvailable ? 1 : 0,
                    l = ObjNewProduct.Discount,
                    m = ObjNewProduct.Tax,
                    n = ObjNewProduct.PurchaseQuantity
                }); ;
            
            if (result > 0)
            {
                _messages.ShowSuccessNotification("Product Added");
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
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<bool> AddSizedProductAsync(ProductModel ObjNewProduct, string picture, ObservableCollection<ProductSizes> ObjProductSize)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
            using IDbTransaction transaction = db.BeginTransaction();
                
                    try
                    {
                        await db.ExecuteAsync(Q_AddSizedProductAsync1, new
                        {
                            a = ObjNewProduct.Name,
                            b = ObjNewProduct.Description,
                            c = ObjNewProduct.PurchasePrice,
                            d = ObjNewProduct.SalePrice,
                            e = ObjNewProduct.Category,
                            f = _imageHelper.NewBitmapToByte(_imageHelper.NewImageToBitmap(picture)),
                            g = ObjNewProduct.AddedDate.ToString(SQLiteDefaultDateFormat),
                            h = ObjNewProduct.Identifier,
                            i = ObjNewProduct.PCTCode,
                            j = ObjNewProduct.IsVariantProduct ? 1 : 0,
                            k = ObjNewProduct.IsAvailable ? 1 : 0,
                            l = ObjNewProduct.Discount,
                            m = ObjNewProduct.Tax,
                            n = ObjNewProduct.PurchaseQuantity
                        });

                        int LastRowID = await GetLastRowIDAsync(db);

                        foreach (var item in ObjProductSize)
                        {
                            await db.ExecuteAsync(Q_AddSizedProductAsync2, new
                            {
                                a = LastRowID,
                                b = item.ProductSize,
                                c = item.PurchasePrice,
                                d = item.SalePrice,
                                e = ObjNewProduct.Category,
                                f = item.PCTCode,
                                g = item.Identifier,
                                h = ObjNewProduct.Tax,
                                i = ObjNewProduct.Discount,
                                j = item.PurchaseQuantity
                            });
                        }

                        transaction.Commit();
                        if (transaction.ReturnSuccess())
                        {
                            _messages.ShowSuccessNotification("Product Added");
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

    public async Task<bool> CheckIfProductExistAsync(string productName)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return await db.ExecuteScalarAsync<bool>(Q_CheckIfProductExistAsync, new { productName });
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteProductAsync(ProductModel objDeleteProduct)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = 0;
                if (objDeleteProduct.IsVariantProduct)
                {
                    db.Open();
                using var transaction = db.BeginTransaction();
                try
                {

                    await db.ExecuteAsync(Q_DeleteProductAsync1, new
                    {
                        a = objDeleteProduct.ID
                    });


                    await db.ExecuteAsync(Q_DeleteProductAsync2, new
                    {
                        a = objDeleteProduct.ID
                    });

                    transaction.Commit();
                    if (transaction.ReturnSuccess())
                    {
                        _messages.ShowSuccessNotification("Product Deleted");
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
                else
                {

                    result = db.Execute(Q_DeleteProductAsync3, new
                    {
                        a = objDeleteProduct.ID
                    });
                    if (result > 0)
                    {
                        _messages.ShowSuccessNotification("Product Deleted");
                        return true;
                    }
                    else
                    {
                        _messages.ShowErrorNotification();
                        db.Close();
                        return false;
                    }
                }
                
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<ObservableCollection<ProductModel>> GetAllProductsAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<ProductModel>(Q_GetAllProductsAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductModel>();
        }
    }

    public async Task<int> GetLastRowIDAsync(IDbConnection db)
    {
        try
        {
            return await db.ExecuteScalarAsync<int>(Q_GetLastRowIDAsync);
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }

    public ObservableCollection<ProductSizes> GetSizesForSelectedProduct(int productId)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = db.Query<ProductSizes>(Q_GetSizesForSelectedProduct, new { productId });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductSizes>();
        }
    }

    public async Task<ObservableCollection<ProductPerformanceModel>> GetProductPerformance(string productName)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<ProductPerformanceModel>(Q_GetProductPerformance, new { productName });
            ObservableCollection<ProductPerformanceModel> data = new();

            foreach (var item in collection)
            {
                data.Add(new ProductPerformanceModel
                {
                    ProductName = item.ProductName,
                    TransactionID = item.TransactionID,
                    Date = GetDateForTransaction(item.TransactionID, db),
                    Qty = Convert.ToInt32(item.Qty),
                    Price = Convert.ToDecimal(item.Price),
                    Discount = Convert.ToDecimal(item.Discount),
                    Tax = Convert.ToDecimal(item.Tax),
                    Total = Convert.ToDecimal(item.Total)
                });
            }
            return data;


        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductPerformanceModel>();
        }
    }



    private DateTime GetDateForTransaction(int transactionID, IDbConnection conn)
    {
        try
        {
        return conn.QuerySingle<DateTime>(Q_GetDateForTransaction, new { transactionID });
        }
        catch (Exception e) 
        {
            _messages.ShowErrorMessage(e.Message);
            return DateTime.Now;
        }
    }

    public async Task<bool> UpdateProductAsync(ProductModel objUpdateProduct, object picture, bool isImageChanged, object selectedPicture)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
            using IDbTransaction transaction = db.BeginTransaction();
                
                    try
                    {
                        await db.ExecuteAsync(Q_UpdateProductAsync, new
                        {
                            a = objUpdateProduct.Name,
                            b = objUpdateProduct.Description,
                            c = objUpdateProduct.PurchasePrice,
                            d = objUpdateProduct.SalePrice,
                            e = objUpdateProduct.Category,
                            f = _imageHelper.EditImageToBitmapToByte(picture, isImageChanged, selectedPicture),
                            g = objUpdateProduct.AddedDate.ToString(SQLiteDefaultDateFormat),
                            h = objUpdateProduct.Identifier,
                            i = objUpdateProduct.PCTCode,
                            j = objUpdateProduct.IsVariantProduct ? 1 : 0,
                            k = objUpdateProduct.IsAvailable ? 1 : 0,
                            l = objUpdateProduct.Discount,
                            m = objUpdateProduct.Tax,
                            n = objUpdateProduct.PurchaseQuantity,
                            id = objUpdateProduct.ID
                        });
                        transaction.Commit();
                        if (transaction.ReturnSuccess())
                        {
                            _messages.ShowSuccessNotification("Product Updated");
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

    public async Task<bool> UpdateSizedProductAsync(ProductModel objUpdateProduct, ObservableCollection<ProductSizes> ObjProductSize, object picture, bool isImageChanged, object selectedPicture)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
                using IDbTransaction transaction = db.BeginTransaction();
                    try
                    {
                        await db.ExecuteAsync(Q_UpdateSizedProductAsync1, new
                        {
                            a = objUpdateProduct.Name,
                            b = objUpdateProduct.Description,
                            c = objUpdateProduct.PurchasePrice,
                            d = objUpdateProduct.SalePrice,
                            e = objUpdateProduct.Category,
                            f = _imageHelper.EditImageToBitmapToByte(picture, isImageChanged, selectedPicture),
                            g = objUpdateProduct.AddedDate.ToString(SQLiteDefaultDateFormat),
                            h = objUpdateProduct.Identifier,
                            i = objUpdateProduct.PCTCode,
                            j = objUpdateProduct.IsVariantProduct ? 1 : 0,
                            k = objUpdateProduct.IsAvailable ? 1 : 0,
                            l = objUpdateProduct.Discount,
                            m = objUpdateProduct.Tax,
                            n = objUpdateProduct.PurchaseQuantity,
                            id = objUpdateProduct.ID
                        });

                        
                        var newGeneratedSizes = await GeneratingNewProductSizesAsync(objUpdateProduct.ID, db, ObjProductSize);

                        foreach (var item in newGeneratedSizes)
                        {
                            await db.ExecuteAsync(Q_UpdateSizedProductAsync2, new
                            {
                                a = objUpdateProduct.ID,
                                b = item.ProductSize,
                                c = item.PurchasePrice,
                                d = item.SalePrice,
                                e = objUpdateProduct.Category,
                                f = item.TimesSold,
                                g = item.PurchaseQuantity,
                                h = item.PCTCode,
                                i = item.Identifier,
                                j = objUpdateProduct.Tax,
                                k = objUpdateProduct.Discount

                            });
                        }

                        transaction.Commit();
                        if (transaction.ReturnSuccess())
                        {
                            _messages.ShowSuccessNotification("Product Updated");
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

    public async Task<TransactionModel> GetTransactionAsync(int transactionID)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return await db.QuerySingleAsync<TransactionModel>(Q_GetTransactionAsync, new { transactionID });
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return null;
        }
    }

    public async Task<ObservableCollection<ProductSizes>> GeneratingNewProductSizesAsync(int productID, IDbConnection db, ObservableCollection<ProductSizes> productSizesFromUser)
    {
        try
        {
            var sizesFromDatabase = await GetProductSizesFromDatabaseAsync(db, productID);

            await DeleteSizesFromDatabaseAsync(db, productID);

            return AssignNewProductSizes(productSizesFromUser, sizesFromDatabase);
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductSizes>();
        }
    }

    private ObservableCollection<ProductSizes> AssignNewProductSizes(ObservableCollection<ProductSizes> productSizesFromUser, ObservableCollection<ProductSizes> sizesFromDatabase)
    {
        try
        {
            ObservableCollection<ProductSizes> newProductSizes = new();
            for (int i = 0; i < productSizesFromUser.Count; i++)
            {
                if (CheckIfIndexExist(sizesFromDatabase, i))
                {
                    newProductSizes.Add(new ProductSizes
                    {
                        ProductSize = productSizesFromUser[i].ProductSize,
                        PurchasePrice = productSizesFromUser[i].PurchasePrice,
                        SalePrice = productSizesFromUser[i].SalePrice,
                        TimesSold = 0,
                        PurchaseQuantity = productSizesFromUser[i].PurchaseQuantity,
                        PCTCode = productSizesFromUser[i].PCTCode,
                        Identifier = productSizesFromUser[i].Identifier

                    });
                }
                else
                {
                    newProductSizes.Add(new ProductSizes
                    {
                        ProductSize = productSizesFromUser[i].ProductSize,
                        PurchasePrice = productSizesFromUser[i].PurchasePrice,
                        SalePrice = productSizesFromUser[i].SalePrice,
                        TimesSold = sizesFromDatabase[i].TimesSold,
                        PurchaseQuantity = productSizesFromUser[i].PurchaseQuantity,
                        PCTCode = productSizesFromUser[i].PCTCode,
                        Identifier = productSizesFromUser[i].Identifier
                    });
                }
            }

            return newProductSizes;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductSizes>();
        }
    }

    private bool CheckIfIndexExist(ObservableCollection<ProductSizes> productSizesFromDatabase, int index)
    {
        return productSizesFromDatabase.ElementAtOrDefault(index) == null;
    }

    private async Task<ObservableCollection<ProductSizes>> GetProductSizesFromDatabaseAsync(IDbConnection db, int productID)
    {
        try
        {
            var collection = await db.QueryAsync<ProductSizes>(Q_GetProductSizesFromDatabaseAsync, new { productID });
            return collection.ToObservableCollection();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ProductSizes>();
        }
    }

    private async Task DeleteSizesFromDatabaseAsync(IDbConnection db, int productID)
    {
        try
        {

            await db.ExecuteAsync(Q_DeleteSizesFromDatabaseAsync, new { productID });
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    public int GetLastProductID()
    {
        
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return db.ExecuteScalar<int>(Q_GetLastProductID);
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }

    }
}