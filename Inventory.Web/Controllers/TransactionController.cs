using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Data;
using Inventory.Web.Models;

namespace Inventory.Web.Controllers;

public class TransactionController(AppDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var logs = await context.Set<Transaction>()
            .Include(l => l.Product)
            .OrderByDescending(l => l.TransactionDate)
            .ToListAsync();

        return View(logs);
    }

    public async Task<IActionResult> Details(int id)
    {
        var stock = await context.Set<Transaction>()
            .Include(l => l.Product)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (stock == null)
        {
            return NotFound();
        }

        return View(stock);
    }
}