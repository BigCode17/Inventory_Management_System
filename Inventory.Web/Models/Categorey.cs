using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Category Name")]
    public string Name { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }
    public ICollection<Product> Products { get; set; } = [];
}