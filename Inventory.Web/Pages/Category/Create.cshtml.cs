using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Inventory.Web.Data;
using Inventory.Web.Models;

namespace Inventory.Web.Pages.Category;

public class CreateModel(AppDbContext context) : PageModel
{
    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Models.Category Category { get; set; } = default!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        context.Categories.Add(Category);
        await context.SaveChangesAsync();

        return RedirectToPage("./Index");
    }
}