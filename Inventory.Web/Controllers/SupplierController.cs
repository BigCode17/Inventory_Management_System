using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Data;
using Inventory.Web.Models;

namespace Inventory.Web.Controllers
{
    public class SupplierController(AppDbContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await context.Suppliers.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                context.Add(supplier);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await context.Suppliers.FindAsync(id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier supplier)
        {
            if (id != supplier.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {

                    context.Update(supplier);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier.Id))
                        return NotFound();

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var supplier = await context.Suppliers
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var supplier = await context.Suppliers.FindAsync(id);
            bool hasProducts = await context.Products.AnyAsync(p => p.SupplierId == id);
            // addition 
            if(supplier is not null)
            {
                if (hasProducts)
                {
                    TempData["ErrorMessage"] = "Cannot delete supplier because it has associated products.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
                try
                {
                    context.Suppliers.Remove(supplier);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    // حماية إضافية في حال حدوث تعارض أثناء الحذف
                    TempData["ErrorMessage"] = "An error occurred while deleting the supplier. Please try again.";
                    return RedirectToAction(nameof(Delete), new { id });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
            return context.Suppliers.Any(s => s.Id == id);
        }
    }
}