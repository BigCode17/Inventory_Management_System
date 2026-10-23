using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory.Web.Data;
using Inventory.Web.Models;
using Inventory.Web.Services;

namespace Inventory.Web.Controllers;

public class ProductController(AppDbContext context, ILogService logService, IWebHostEnvironment environment) : Controller
{
    public async Task<IActionResult> Index(
        string? search,
        int? categoryId,
        int? supplierId,
        int page = 1)
    {
        int pageSize = 5;

        var products = context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            products = products.Where(p =>
                p.Name.Contains(search) ||
                (p.SKU != null && p.SKU.Contains(search)));
        }

        if (categoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == categoryId.Value);
        }

        if (supplierId.HasValue)
        {
            products = products.Where(p => p.SupplierId == supplierId.Value);
        }

        int totalProducts = await products.CountAsync();
        int totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);

        if (page < 1) page = 1;
        if (totalPages > 0 && page > totalPages) page = totalPages;

        var productList = await products
            .OrderBy(p => p.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        await LoadCategoriesAndSuppliers(categoryId, supplierId);

        ViewBag.Search = search;
        ViewBag.CategoryIdSelected = categoryId;
        ViewBag.SupplierIdSelected = supplierId;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(productList);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var product = await context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null) return NotFound();

        return View(product);
    }

    public async Task<IActionResult> Create()
    {
        await LoadCategoriesAndSuppliers();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product, IFormFile? imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                product.ImagePath = await SaveImageAsync(imageFile);
            }

            context.Products.Add(product);
            await context.SaveChangesAsync();

            logService.AddLog(
                productId: product.Id,
                actionType: ActionType.Created,
                productName: product.Name,
                details: "A new product has been added",
                oldQty: 0,
                newQty: product.StockQuantity
            );
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAndSuppliers(product.CategoryId, product.SupplierId);
        return View(product);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();

        await LoadCategoriesAndSuppliers(product.CategoryId, product.SupplierId);
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product, IFormFile? imageFile)
    {
        if (id != product.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                var existingProduct = await context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                if (existingProduct == null) return NotFound();

                if (imageFile != null && imageFile.Length > 0)
                {
                    // Remove old image file if replacing
                    DeleteImageFile(existingProduct.ImagePath);
                    product.ImagePath = await SaveImageAsync(imageFile);
                }
                else
                {
                    product.ImagePath = existingProduct.ImagePath;
                }

                context.Products.Update(product);

                if (existingProduct.StockQuantity != product.StockQuantity)
                {
                    logService.AddLog(
                        product.Id, 
                        ActionType.QuantityChanged, 
                        product.Name,
                        "Quantity has been updated", 
                        existingProduct.StockQuantity, 
                        product.StockQuantity
                    );
                }

                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        await LoadCategoriesAndSuppliers(product.CategoryId, product.SupplierId);
        return View(product);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var product = await context.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is not null)
        {
            DeleteImageFile(product.ImagePath);

            logService.AddLog(
                product.Id,
                ActionType.Deleted,
                product.Name,
                "Product has been deleted",
                product.StockQuantity,
                0
            );

            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return context.Products.Any(e => e.Id == id);
    }

    private async Task LoadCategoriesAndSuppliers(int? selectedCategory = null, int? selectedSupplier = null)
    {
        ViewBag.CategoryId = new SelectList(
            await context.Categories.OrderBy(c => c.Name).ToListAsync(), 
            "Id", 
            "Name", 
            selectedCategory);

        ViewBag.SupplierId = new SelectList(
            await context.Suppliers.OrderBy(s => s.Name).ToListAsync(), 
            "Id", 
            "Name", 
            selectedSupplier);
    }

    private async Task<string> SaveImageAsync(IFormFile imageFile)
    {
        string uploadsFolder = Path.Combine(environment.WebRootPath, "images");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
        string filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return "/images/" + fileName;
    }

    private void DeleteImageFile(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath)) return;

        string relativePath = imagePath.TrimStart('/');
        string fullPath = Path.Combine(environment.WebRootPath, relativePath);

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }
    }
}