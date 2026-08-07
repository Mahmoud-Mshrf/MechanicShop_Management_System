using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.WorkOrders.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.RepairTasks.Commands.UpdateRepairTask;

public class UpdateRepairTaskCommandHandler(IAppDbContext context,HybridCache cache,ILogger<UpdateRepairTaskCommandHandler> logger) : IRequestHandler<UpdateRepairTaskCommand, Result<Updated>>
{
public async Task<Result<Updated>> Handle(
    UpdateRepairTaskCommand request,
    CancellationToken cancellationToken)
{
    var repairTask = await context.RepairTasks
        .Include(x => x.Parts)
        .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

    if (repairTask is null)
        return ApplicationErrors.RepairTaskNotFound;

    var includedInActiveOrders = await context.WorkOrders.AnyAsync(
        x =>
            x.RepairTasks.Any(r => r.Id == request.Id) &&
            x.OrderState == OrderState.InProgress,
        cancellationToken);

    if (includedInActiveOrders)
        return ApplicationErrors.IncludedInActiveOrders;

    var updateResult = repairTask.Update(
        request.Name,
        request.LaborCost,
        request.EstimatedDurationInMinutes);

    if (updateResult.IsError)
        return updateResult.Errors;

    //---------------------------------------
    // Validate request
    //---------------------------------------

    var requestParts = request.RepairTaskPartDtos;

    var requestIds = requestParts
        .Select(x => x.Id)
        .ToList();

    if (requestIds.Count != requestIds.Distinct().Count())
        return ApplicationErrors.DuplicateParts;

    var existingIds = await context.Parts
        .Where(x => requestIds.Contains(x.Id))
        .Select(x => x.Id)
        .ToListAsync(cancellationToken);

    if (existingIds.Count != requestIds.Count)
        return ApplicationErrors.PartNotFound;

    //---------------------------------------
    // Remove deleted parts
    //---------------------------------------

    var idsToRemove = repairTask.Parts
        .Select(x => x.PartId)
        .Except(requestIds)
        .ToList();

    if (idsToRemove.Count > 0)
    {
        var removeResult = repairTask.RemoveParts(idsToRemove);

        if (removeResult.IsError)
            return removeResult.Errors;
    }

    //---------------------------------------
    // Add new parts / update existing ones
    //---------------------------------------

    foreach (var dto in requestParts)
    {
        if (repairTask.Parts.Any(x => x.PartId == dto.Id))
        {
            var result = repairTask.UpdatePartQuantity(dto.Id, dto.Quantity);

            if (result.IsError)
                return result.Errors;
        }
        else
        {
            var result = repairTask.AddPart(dto.Id, dto.Quantity);

            if (result.IsError)
                return result.Errors;
        }
    }

    await context.SaveChangesAsync(cancellationToken);

    await cache.RemoveByTagAsync("repair-tasks", cancellationToken);

    logger.LogInformation(
        "Repair task {RepairTaskId} updated successfully.",
        repairTask.Id);

    return Result.Updated;
}
}