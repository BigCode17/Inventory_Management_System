using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Web.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string SKU { get; set; }

    [Required]
    [Range(0, 999999)]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    [Column(TypeName = "decimal(18,2)")]
    public int StockQuantity { get; set; }

    public string? ImagePath { get; set; }
    public List<ProductImage> Images { get; set; } = [];

    public int? CategoryId { get; set; }
    public Category? Category { get; set; }
    public int? SupplierId { get; set; }

    public Supplier? Supplier { get; set; }
    // public ICollection<Transactions> Transactions { get; set; } = [];

}