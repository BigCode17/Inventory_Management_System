using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Data;

namespace Inventory.Web.Pages.Category;

public class EditModel(AppDbContext context) : PageModel
{
    [BindProperty]
    public Models.Category Category { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null) return NotFound();

        var category = await context.Categories.FirstOrDefaultAsync(m => m.Id == id);
        if (category == null) return NotFound();

        Category = category;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        context.Attach(Category).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Categories.Any(e => e.Id == Category.Id))
            {
                return NotFound();
            }
            throw;
        }

        return RedirectToPage("./Index");
    }
}