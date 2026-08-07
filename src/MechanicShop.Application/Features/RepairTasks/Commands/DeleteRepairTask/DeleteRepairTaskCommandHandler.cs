using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.WorkOrders.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.RepairTasks.Commands.DeleteRepairTask;

public class DeleteRepairTaskCommandHandler(IAppDbContext context,HybridCache cache,Logger<DeleteRepairTaskCommand> logger) : IRequestHandler<DeleteRepairTaskCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeleteRepairTaskCommand request, CancellationToken cancellationToken)
    {
        
        // 1- check if it is null
        var repairTask = await context.RepairTasks.FindAsync(request.Id);
        if (repairTask is null)
        {
            return ApplicationErrors.RepairTaskNotFound;
        }
        // 2- check if it is included in active work orders (Scheduled or in progress)
        var IncludedInActiveOrders = await context.WorkOrders.Include(x=>x.RepairTasks)
            .Where(x=>x.OrderState==OrderState.InProgress|| x.OrderState==OrderState.Scheduled)
            .AnyAsync(x=>x.RepairTasks.Contains(repairTask));
        if (IncludedInActiveOrders)
        {
            return ApplicationErrors.IncludedInActiveOrders;
        }
        // 3- delete if its free from responsibilities
        context.RepairTasks.Remove(repairTask);
        await cache.RemoveByTagAsync("repair-tasks");
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Repair task :{TaskName} with id {TaskId} was deleted successfully",repairTask.Name,repairTask.Id);

        return Result.Deleted;
        
    }
}