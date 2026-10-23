using System.ComponentModel.DataAnnotations.Schema;

namespace Inventory.Web.Models;

[Table("ProductImages")]
public class ProductImage
{
    public int Id { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public bool IsMain { get; set; } // لتحديد الصورة الرئيسية للمنتج

    public int ProductId { get; set; }
    public Product? Product { get; set; }
}