using System.ComponentModel.DataAnnotations;

namespace Inventory.Web.Models;

public enum ActionType
{
    Created,
    QuantityChanged,
    Deleted
}

public class Transaction
{
    [Required(ErrorMessage = "ID  is required.")]
    [Display(Name = "ID")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Action type  is required.")]
    [Display(Name = "Action type")]
    public ActionType ActionType { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public int? OldQuantity { get; set; }
    public int? NewQuantity { get; set; }
    public string Details { get; set; }   // simple description for the event
    
    public string ProductName { get; set; } = string.Empty;
    public int? ProductId { get; set; } // foregin key
    public Product? Product { get; set; } = null!; // navigation property
    // public Supplier Supplier { get; set; }
}
