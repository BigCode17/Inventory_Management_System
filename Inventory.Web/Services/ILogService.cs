using Inventory.Web.Models;

namespace Inventory.Web.Services;

public interface ILogService
{
    void AddLog(int productId, ActionType actionType, string productName,
            string details, int? oldQty = null, int? newQty = null,
            int? supplierId = null);
}