using Inventory.Web.Models;

namespace Inventory.Web.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // التحقق مما إذا كانت البيانات موجودة مسبقاً
        if (context.Products.Any() || context.Categories.Any() || context.Suppliers.Any())
        {
            return; // تم ملء البيانات سابقاً
        }

        // 1. إضافة الموردين (Suppliers)
        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Name = "Tech World Logistics",
                ContactPerson = "Ahmed Hassan",
                Phone = "01012345678",
                Email = "info@techworld.com"
            },
            new Supplier
            {
                Name = "Global Electronics Co.",
                ContactPerson = "Sara Mahmoud",
                Phone = "01123456789",
                Email = "sales@globalelectronics.com"
            },
            new Supplier
            {
                Name = "Elite Office Supplies",
                ContactPerson = "Mohamed Ali",
                Phone = "01234567890",
                Email = "support@elitesupplies.com"
            }
        };

        await context.Suppliers.AddRangeAsync(suppliers);
        await context.SaveChangesAsync();

        // 2. إضافة التصنيفات (Categories)
        var categories = new List<Category>
        {
            new Category
            {
                Name = "Electronics & Laptops",
                Description = "High-performance laptops, PCs, and electronic accessories."
            },
            new Category
            {
                Name = "Smartphones & Tablets",
                Description = "Latest smart mobile devices, tablets, and wearable tech."
            },
            new Category
            {
                Name = "Office Furniture & Gear",
                Description = "Ergonomic chairs, desks, and workspace essentials."
            }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        // 3. إضافة المنتجات (Products) مع الصور
        var products = new List<Product>
        {
            new Product
            {
                Name = "Dell XPS 15 Laptop",
                SKU = "LAP-DELL-XPS15",
                Price = 45000.00m,
                StockQuantity = 12,
                CategoryId = categories[0].Id,
                SupplierId = suppliers[0].Id,
                ImagePath = "/images/products/dell-xps-15.jpg",
                Images = new List<ProductImage>
                {
                    new ProductImage { ImagePath = "/images/products/dell-xps-15.jpg", IsMain = true },
                    new ProductImage { ImagePath = "/images/products/dell-xps-15-side.jpg", IsMain = false }
                }
            },
            new Product
            {
                Name = "MacBook Pro 16 M3",
                SKU = "LAP-APP-MBP16",
                Price = 85000.00m,
                StockQuantity = 8,
                CategoryId = categories[0].Id,
                SupplierId = suppliers[0].Id,
                ImagePath = "/images/products/macbook-pro.jpg",
                Images = new List<ProductImage>
                {
                    new ProductImage { ImagePath = "/images/products/macbook-pro.jpg", IsMain = true }
                }
            },
            new Product
            {
                Name = "Samsung Galaxy S24 Ultra",
                SKU = "MOB-SAM-S24U",
                Price = 52000.00m,
                StockQuantity = 25,
                CategoryId = categories[1].Id,
                SupplierId = suppliers[1].Id,
                ImagePath = "/images/products/s24-ultra.jpg",
                Images = new List<ProductImage>
                {
                    new ProductImage { ImagePath = "/images/products/s24-ultra.jpg", IsMain = true }
                }
            },
            new Product
            {
                Name = "iPhone 15 Pro Max",
                SKU = "MOB-APP-IP15PM",
                Price = 62000.00m,
                StockQuantity = 18,
                CategoryId = categories[1].Id,
                SupplierId = suppliers[1].Id,
                ImagePath = "/images/products/iphone15.jpg",
                Images = new List<ProductImage>
                {
                    new ProductImage { ImagePath = "/images/products/iphone15.jpg", IsMain = true }
                }
            },
            new Product
            {
                Name = "Ergonomic Mesh Office Chair",
                SKU = "FUR-CHR-ERG01",
                Price = 8500.00m,
                StockQuantity = 30,
                CategoryId = categories[2].Id,
                SupplierId = suppliers[2].Id,
                ImagePath = "/images/products/office-chair.jpg",
                Images = new List<ProductImage>
                {
                    new ProductImage { ImagePath = "/images/products/office-chair.jpg", IsMain = true }
                }
            }
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        // 4. إضافة سجل الحركة المعاملات (Transactions Log)
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                ProductId = products[0].Id,
                ActionType = ActionType.Created,
                TransactionDate = DateTime.Now.AddDays(-10),
                OldQuantity = 0,
                NewQuantity = 10,
                Details = "Initial stock entry during setup."
            },
            new Transaction
            {
                ProductId = products[0].Id,
                ActionType = ActionType.QuantityChanged,
                TransactionDate = DateTime.Now.AddDays(-5),
                OldQuantity = 10,
                NewQuantity = 12,
                Details = "Received 2 additional units from Tech World Logistics."
            },
            new Transaction
            {
                ProductId = products[2].Id,
                ActionType = ActionType.Created,
                TransactionDate = DateTime.Now.AddDays(-8),
                OldQuantity = 0,
                NewQuantity = 25,
                Details = "Initial inventory import."
            },
            new Transaction
            {
                ProductId = products[4].Id,
                ActionType = ActionType.QuantityChanged,
                TransactionDate = DateTime.Now.AddDays(-2),
                OldQuantity = 35,
                NewQuantity = 30,
                Details = "Sold 5 units to corporate client."
            }
        };

        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
    }
}