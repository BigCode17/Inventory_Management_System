using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Data;

namespace Inventory.Web.Pages.Category;

public class IndexModel(AppDbContext context) : PageModel
{
    public IList<Models.Category> Categories { get; set; } = default!;

    [Microsoft.AspNetCore.Mvc.TempData]
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync()
    {
        Categories = await context.Categories
                                    .Include(c => c.Products)
                                    .ToListAsync();
    }
}