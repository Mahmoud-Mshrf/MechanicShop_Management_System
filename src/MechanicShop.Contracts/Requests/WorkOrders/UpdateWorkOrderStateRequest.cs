using MechanicShop.Contracts.Common;

namespace MechanicShop.Contracts.Requests.WorkOrders;

public class UpdateWorkOrderStateRequest
{
    public WorkOrderState NewStatus { get; set; }
}