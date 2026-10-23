using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Data;

namespace Inventory.Web.Pages.Category;

public class DeleteModel(AppDbContext context) : PageModel
{
    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null) return NotFound();

        var category = await context.Categories
                                        .Include(c => c.Products)
                                        .FirstOrDefaultAsync(m => m.Id == id);

        if (category == null) return NotFound();

        if (category.Products != null && category.Products.Count != 0)
        {
            TempData["ErrorMessage"] = "Cannot delete category because it has associated products.";
            return RedirectToPage("./Index");
        }

        context.Categories.Remove(category);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}