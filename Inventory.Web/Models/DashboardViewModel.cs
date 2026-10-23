namespace Inventory.Web.Models;

public class DashboardViewModel
{
    public int TotalProducts { get; init; }
    public int LowStockProducts { get; init; }
    public int OutOfStockProducts { get; init; }
    public int TotalSuppliers { get; init; }
    public int TotalCategories { get; init; }


    public long TotalStockUnits { get; init; }

    public decimal TotalInventoryValue { get; init; }

    public long StockInThisMonth { get; init; }
    public long StockOutThisMonth { get; init; }

    public IEnumerable<Transaction> RecentTransactions { get; init; } = [];


}
