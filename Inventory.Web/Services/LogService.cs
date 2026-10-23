using Inventory.Web.Models;
using Inventory.Web.Data;

namespace Inventory.Web.Services;

public class LogService(AppDbContext context) : ILogService
{
    public void AddLog(int productId, ActionType actionType, string productName,
                        string details, int? oldQty = null, int? newQty = null,
                        int? supplierId = null)
    {
        var log = new Transaction
        {
            ProductId = productId,
            ProductName = productName,
            ActionType = actionType,
            Details = details,
            OldQuantity = oldQty,
            NewQuantity = newQty,
            TransactionDate = DateTime.Now
        };

        context.Set<Transaction>().Add(log);
    }
}
