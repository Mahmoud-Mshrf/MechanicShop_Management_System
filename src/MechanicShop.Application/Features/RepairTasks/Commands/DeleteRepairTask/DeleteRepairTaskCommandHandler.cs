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
        var repairTask = await context.RepairTasks.FirstOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
        if (repairTask is null)
        {
            return ApplicationErrors.RepairTaskNotFound;
        }
        // 2- check if it is included in active work orders (Scheduled or in progress)
        // var IncludedInActiveOrders = await context.WorkOrders.Include(x=>x.RepairTasks)
        //     .Where(x=>x.OrderState==OrderState.InProgress|| x.OrderState==OrderState.Scheduled)
        //     .AnyAsync(x=>x.RepairTasks.Contains(repairTask));
        // or 
        // var inUse = await context.WorkOrders.AnyAsync(x=>x.RepairTasks.Any(x=>x.Id==request.Id)); or 
        var inUse = await context.WorkOrders.SelectMany(x=>x.RepairTasks).AnyAsync(x=>x.Id==request.Id,cancellationToken);
        if (inUse)
        {
            return ApplicationErrors.IncludedInActiveOrders;
        }
        // 3- delete if its free from responsibilities
        context.RepairTasks.Remove(repairTask);
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync("repair-tasks");
        logger.LogInformation("Repair task :{TaskName} with id {TaskId} was deleted successfully",repairTask.Name,repairTask.Id);

        return Result.Deleted;
        
    }
}