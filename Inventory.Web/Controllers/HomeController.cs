using Inventory.Web.Data;
using Inventory.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Inventory.Web.Controllers;

public class HomeController(AppDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var now = DateTime.Now;
        var monthStart = new DateTime(now.Year, now.Month, 1);
        var products = context.Products.AsNoTracking();

        var monthlyTransactions = await context.Transactions
            .AsNoTracking()
            .Where(t => t.TransactionDate >= monthStart && t.TransactionDate <= now)
            .Select(t => new { t.OldQuantity, t.NewQuantity })
            .ToListAsync();

        var dashboardData = new DashboardViewModel
        {
            TotalProducts = await products.CountAsync(),
            LowStockProducts = await products.CountAsync(p => p.StockQuantity > 0 && p.StockQuantity <= 5),
            OutOfStockProducts = await products.CountAsync(p => p.StockQuantity == 0),
            TotalSuppliers = await context.Suppliers.CountAsync(),
            TotalCategories = await context.Categories.CountAsync(),
            TotalStockUnits = await products.SumAsync(p => (long)p.StockQuantity),
            TotalInventoryValue = await products.SumAsync(p => p.Price * p.StockQuantity),
            StockInThisMonth = monthlyTransactions.Sum(t => t.OldQuantity.HasValue && t.NewQuantity.HasValue && t.NewQuantity > t.OldQuantity ? t.NewQuantity.Value - t.OldQuantity.Value : 0),
            StockOutThisMonth = monthlyTransactions.Sum(t => t.OldQuantity.HasValue && t.NewQuantity.HasValue && t.NewQuantity < t.OldQuantity ? t.OldQuantity.Value - t.NewQuantity.Value : 0),
            RecentTransactions = await context.Transactions
                .AsNoTracking()
                .Include(t => t.Product)
                .OrderByDescending(t => t.TransactionDate)
                .Take(7)
                .ToListAsync()
        };

        return View(dashboardData);
    }

    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
