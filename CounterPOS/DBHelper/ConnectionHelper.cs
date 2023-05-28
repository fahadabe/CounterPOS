namespace CounterPOS.DBHelper;

public class ConnectionHelper
{
    public static string CnnVal()
    {
        return ConfigurationManager.ConnectionStrings["Counter"].ConnectionString;
    }
}