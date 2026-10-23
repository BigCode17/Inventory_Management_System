using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models;

public class Supplier
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string ContactPerson { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public ICollection<Product> Products { get; set; } = [];
}