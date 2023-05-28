namespace CounterPOS.Model;

public class DashboardService : IDashboardService
{
    private readonly IDialogMessages _messages;
    public DashboardService(IDialogMessages messages)
    {
        _messages = messages;
    }

    private readonly List<string> Q_cashQueries = new()
    {
         $"SELECT SUM(CAST(GrandTotal AS REAL)) as Sale, COUNT(GrandTotal) as TransactionCount FROM tblSaleTransactions WHERE InvoiceDate = DATE('now', 'localtime') AND PaymentType = @paymentType",
         $"SELECT SUM(CAST(GrandTotal AS REAL)) as Sale, COUNT(GrandTotal) as TransactionCount FROM tblSaleTransactions WHERE InvoiceDate = DATE('now', '-1 days', 'localtime') AND PaymentType = @paymentType",
         $"SELECT SUM(CAST(GrandTotal AS REAL)) as Sale, COUNT(GrandTotal) as TransactionCount FROM tblSaleTransactions WHERE strftime('%W/%Y', InvoiceDate) == strftime('%W/%Y', 'now') AND PaymentType = @paymentType",
         $"SELECT SUM(CAST(GrandTotal AS REAL)) as Sale, COUNT(GrandTotal) as TransactionCount FROM tblSaleTransactions WHERE InvoiceDate BETWEEN DATE('now', 'start of month') AND DATE('now', 'localtime') AND PaymentType = @paymentType",
         $"SELECT SUM(CAST(GrandTotal AS REAL)) as Sale, COUNT(GrandTotal) as TransactionCount FROM tblSaleTransactions WHERE InvoiceDate BETWEEN DATE('now', 'start of year') AND DATE('now', 'localtime') AND PaymentType = @paymentType",
         $"SELECT SUM(CAST(GrandTotal AS REAL)) as Sale, COUNT(GrandTotal) as TransactionCount FROM tblSaleTransactions WHERE PaymentType = @paymentType"
    };

    private readonly string Q_GetAllExpanseReportAsync = "SELECT ExpanseCategory as CategoryName, SUM(Amount) as Amount FROM tblExpanses GROUP BY ExpanseCategory";
    private readonly string Q_GetMonthlyExpanse = "SELECT STRFTIME('%Y-%m', ExpanseDate) AS Month, sum(Amount) AS Expanse FROM tblExpanses GROUP BY STRFTIME('%Y-%m', ExpanseDate)";
    private readonly string Q_GetMonthlyPurchaseExpanse = "SELECT STRFTIME('%Y-%m', PurchaseDate) AS Month, SUM(CAST(GrandTotal AS REAL)) AS PurchaseExpanse FROM tblPurchaseTransactions where PaymentType = @paymentType GROUP BY STRFTIME('%Y-%m', PurchaseDate)";
    private readonly string Q_GetMonthlySale = "SELECT STRFTIME('%Y-%m', InvoiceDate) AS Month, SUM(CAST(GrandTotal AS REAL)) AS Sale FROM tblSaleTransactions where PaymentType = @paymentType GROUP BY STRFTIME('%Y-%m', InvoiceDate)";
    private readonly string Q_GetThisMonthExpanseReport = "SELECT ExpanseCategory as CategoryName, SUM(Amount) as Amount FROM tblExpanses WHERE ExpanseDate BETWEEN DATE('now', 'start of month') AND DATE('now', 'localtime') GROUP BY ExpanseCategory";
    private readonly string Q_GetTodayExpanseReport = "SELECT ExpanseCategory as CategoryName, SUM(Amount) as Amount FROM tblExpanses WHERE ExpanseDate = DATE('now', 'localtime') GROUP BY ExpanseCategory";
    private readonly string Q_GetTodaySale = "SELECT SUM(CAST(GrandTotal AS REAL)) as GrandTotal FROM tblSaleTransactions WHERE InvoiceDate = DATE('now', 'localtime') AND PaymentType = @paymentType";
    private readonly string Q_GetTodaysDiscountAsync = "SELECT SUM(DiscountPrice) FROM tblSaleTransactions WHERE InvoiceDate = DATE('now', 'localtime') AND PaymentType = @paymentType";
    private readonly string Q_GetTodayTransactionsCount = "SELECT Count(GrandTotal) FROM tblSaleTransactions WHERE InvoiceDate = DATE('now', 'localtime') AND PaymentType = @paymentType";
    private readonly string Q_GetTopSellingItems = "SELECT ID, Name, IsVariantProduct, SUM(TimesSold) AS QuantitySold FROM tblProducts GROUP BY Name";
    private readonly string Q_GetTopSellingSizes = "SELECT * FROM tblProductSize Where ProductID = @productID";
    private readonly string Q_GetWeeklySale = "SELECT InvoiceDate as Day, SUM(CAST(GrandTotal AS REAL)) as Sale FROM tblSaleTransactions WHERE DATE(InvoiceDate) BETWEEN date('now', 'weekday 0', '-6 day') AND date('now', 'weekday 0') AND PaymentType = @paymentType GROUP BY strftime('%d', InvoiceDate) ORDER By Day ASC";
    private readonly string Q_GetYesterdayExpanseReport = "SELECT ExpanseCategory as CategoryName, SUM(Amount) as Amount FROM tblExpanses WHERE ExpanseDate = DATE('now', '-1 days', 'localtime') GROUP BY ExpanseCategory";
    private readonly List<string> Q_saleIdentifier = new()
    {
        "Today",
        "Yesterday",
        "This Week",
        "This Month",
        "Year to Date",
        "All In"
    };

    public async Task<ObservableCollection<ExpanseReportModel>> GetAllExpanseReportAsync()
    {
        try
        {
            ObservableCollection<ExpanseReportModel> expanseReportModels = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QueryAsync<ExpanseReportModel>(Q_GetAllExpanseReportAsync);
                foreach (var item in result)
                {
                    expanseReportModels.Add(new ExpanseReportModel
                    {
                        CategoryName = item.CategoryName,
                        Amount = item.Amount
                    });
                }
            

            return expanseReportModels;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ExpanseReportModel>();
        }
    }


    public async Task<ObservableCollection<SaleCollectionModel>> GetCashSale(string paymentType)
    {
        try
        {
            ObservableCollection<SaleCollectionModel> collection = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            for (int i = 0; i < Q_cashQueries.Count; i++)
                {
                    var result = await db.QuerySingleAsync<SaleCollectionModel>(Q_cashQueries[i], new { paymentType });

                    collection.Add(new SaleCollectionModel
                    {
                        SaleIdentifier = Q_saleIdentifier[i],
                        Sale = result.Sale,
                        TransactionCount = result.TransactionCount
                    });
                }
            

            return collection;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<SaleCollectionModel>();
        }
    }

    public ObservableCollection<MonthlyExpanse> GetMonthlyExpanse(bool OnlyCurrentYear)
    {
        try
        {
            ObservableCollection<MonthlyExpanse> collection = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = db.Query<MonthlyExpanse>(Q_GetMonthlyExpanse);
                foreach (var item in result)
                {
                    if (!string.IsNullOrWhiteSpace(item.Month))
                    {
                        string[] tokens = item.Month.ToString().Split('-');
                        string year = tokens[0];
                        string month = tokens[1];
                        if (OnlyCurrentYear)
                        {
                            if (year == DateTime.Now.Year.ToString())
                            {
                                collection.Add(new MonthlyExpanse
                                {
                                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(month)) + "-" + year,
                                    Expanse = item.Expanse
                                });
                            }
                        }
                        else
                        {
                            collection.Add(new MonthlyExpanse
                            {
                                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(month)) + "-" + year,
                                Expanse = item.Expanse
                            });
                        }
                    }
                }
            
            return collection;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<MonthlyExpanse>();
        }
    }

    public ObservableCollection<MonthlyPurchaseExpanse> GetMonthlyPurchaseExpanse(bool OnlyCurrentYear, string paymentType)
    {
        try
        {
            ObservableCollection<MonthlyPurchaseExpanse> monthlyPurchaseExpanses = new();
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection =  db.Query<MonthlyPurchaseExpanse>(Q_GetMonthlyPurchaseExpanse, new
                {
                    paymentType
                });
                foreach (var item in collection)
                {
                    if (!string.IsNullOrWhiteSpace(item.Month))
                    {
                        string[] tokens = item.Month.ToString().Split('-');
                        string year = tokens[0];
                        string month = tokens[1];
                        if (OnlyCurrentYear)
                        {
                            if (year == DateTime.Now.Year.ToString())
                            {
                                monthlyPurchaseExpanses.Add(new MonthlyPurchaseExpanse
                                {
                                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(month)) + "-" + year,
                                    PurchaseExpanse = item.PurchaseExpanse
                                });
                            }
                        }
                        else
                        {
                            monthlyPurchaseExpanses.Add(new MonthlyPurchaseExpanse
                            {
                                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(month)) + "-" + year,
                                PurchaseExpanse = item.PurchaseExpanse
                            });
                        }
                    }
                }
            
            return monthlyPurchaseExpanses;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<MonthlyPurchaseExpanse>();
        }
    }

    public ObservableCollection<MonthlySale> GetMonthlySale(bool OnlyCurrentYear, string paymentType)
    {
        try
        {
            ObservableCollection<MonthlySale> collection = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = db.Query<MonthlySale>(Q_GetMonthlySale, new { paymentType });

                foreach (var item in result)
                {
                    if (!string.IsNullOrWhiteSpace(item.Month))
                    {
                        string[] tokens = item.Month.ToString().Split('-');
                        string year = tokens[0];
                        string month = tokens[1];
                        if (OnlyCurrentYear)
                        {
                            if (year == DateTime.Now.Year.ToString())
                            {
                                collection.Add(new MonthlySale
                                {
                                    Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(month)) + "-" + year,
                                    Sale = item.Sale
                                });
                            }
                        }
                        else
                        {
                            collection.Add(new MonthlySale
                            {
                                Month = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Convert.ToInt32(month)) + "-" + year,
                                Sale = item.Sale
                            });
                        }
                    }
                }
            
            return collection;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<MonthlySale>();
        }
    }

    public async Task<ObservableCollection<ExpanseReportModel>> GetThisMonthExpanseReport()
    {
        try
        {
            ObservableCollection<ExpanseReportModel> expanseReportModels = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QueryAsync<ExpanseReportModel>(Q_GetThisMonthExpanseReport);
                foreach (var item in result.ToObservableCollection())
                {
                    expanseReportModels.Add(new ExpanseReportModel
                    {
                        CategoryName = item.CategoryName,
                        Amount = item.Amount
                    });
                }
            
            return expanseReportModels;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ExpanseReportModel>();
        }
    }

    public async Task<ObservableCollection<ExpanseReportModel>> GetTodayExpanseReport()
    {
        try
        {
            ObservableCollection<ExpanseReportModel> expanseReportModels = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QueryAsync<ExpanseReportModel>(Q_GetTodayExpanseReport);
                foreach (var item in result.ToObservableCollection())
                {
                    expanseReportModels.Add(new ExpanseReportModel
                    {
                        CategoryName = item.CategoryName,
                        Amount = item.Amount
                    });
                }
            

            return expanseReportModels;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ExpanseReportModel>();
        }
    }

    public async Task<decimal> GetTodaySale(string paymentType)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return await db.ExecuteScalarAsync<decimal>(Q_GetTodaySale, new { paymentType });
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }

    public async Task<decimal> GetTodaysDiscountAsync(string paymentType)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return await db.ExecuteScalarAsync<decimal>(Q_GetTodaysDiscountAsync, new { paymentType });
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }

    public async Task<int> GetTodayTransactionsCount(string paymentType)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return await db.ExecuteScalarAsync<int>(Q_GetTodayTransactionsCount, new { paymentType });
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }

    public async Task<ObservableCollection<TopSellingItemsModel>> GetTopSellingItems()
    {
        try
        {
            ObservableCollection<TopSellingItemsModel> topSellingItemsModels = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TopSellingItemsModel>(Q_GetTopSellingItems);
                foreach (var item in collection)
                {
                    if (item.IsVariantProduct == false)
                    {
                        topSellingItemsModels.Add(new TopSellingItemsModel
                        {
                            ID = item.ID,
                            Name = item.Name,
                            QuantitySold = item.QuantitySold
                        });
                    }
                    else
                    {
                        ObservableCollection<TopSellingProductSizeModel> topSellingSizes = new();
                        topSellingSizes = await GetTopSellingSizes(item.ID, db);
                        foreach (var item2 in topSellingSizes)
                        {
                            topSellingItemsModels.Add(new TopSellingItemsModel
                            {
                                ID = item.ID,
                                Name = $"{item.Name} {item2.ProductSize}",
                                QuantitySold = item2.TimesSold
                            });
                        }
                    }
                }
            
            return topSellingItemsModels.OrderByDescending(t => t.QuantitySold).ToObservableCollection();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TopSellingItemsModel>();
        }
    }

    public async Task<ObservableCollection<TopSellingProductSizeModel>> GetTopSellingSizes(string productID, IDbConnection db)
    {
        try
        {
            var collection = await db.QueryAsync<TopSellingProductSizeModel>(Q_GetTopSellingSizes, new { productID });
            return collection.ToObservableCollection();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TopSellingProductSizeModel>();
        }
    }

    public async Task<ObservableCollection<WeeklySale>> GetWeeklySale(string? paymentType)
    {
        try
        {
            ObservableCollection<WeeklySale> weeklySales = new();

            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QueryAsync<WeeklySale>(Q_GetWeeklySale, new { paymentType });
                foreach (var item in result.ToObservableCollection())
                {
                    weeklySales.Add(new WeeklySale
                    {
                        Day = item.Day,
                        Sale = item.Sale
                    });
                }
            
            return weeklySales;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<WeeklySale>();
        }
    }

    public async Task<ObservableCollection<ExpanseReportModel>> GetYesterdayExpanseReport()
    {
        try
        {
            ObservableCollection<ExpanseReportModel> expanseReportModels = new();
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QueryAsync<ExpanseReportModel>(Q_GetYesterdayExpanseReport);
                foreach (var item in result.ToObservableCollection())
                {
                    expanseReportModels.Add(new ExpanseReportModel
                    {
                        CategoryName = item.CategoryName,
                        Amount = item.Amount
                    });
                }
            
            return expanseReportModels;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ExpanseReportModel>();
        }
    }
}