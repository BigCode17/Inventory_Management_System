using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Data;

namespace Inventory.Web.Pages.Category;

public class DetailsModel(AppDbContext context) : PageModel
{
    public Models.Category Category { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var category = await context.Categories
                                        .Include(c => c.Products)
                                        .FirstOrDefaultAsync(m => m.Id == id);

        if (category == null) return NotFound();

        Category = category;
        return Page();
    }
}