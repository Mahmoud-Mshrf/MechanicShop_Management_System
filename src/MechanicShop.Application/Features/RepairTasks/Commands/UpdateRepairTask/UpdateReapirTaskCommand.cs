using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.RepairTasks.Dtos;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.RepairTasks.Commands.UpdateRepairTask;

public sealed record UpdateRepairTaskCommand(Guid Id,string Name,decimal LaborCost,RepairDurationInMinutes EstimatedDurationInMinutes,List<RepairTaskPartDto> RepairTaskPartDtos):IRequest<Result<Updated>>;

public class UpdateRepairTaskCommandHandler(IAppDbContext context,HybridCache cache,ILogger<UpdateRepairTaskCommandHandler> logger) : IRequestHandler<UpdateRepairTaskCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdateRepairTaskCommand request, CancellationToken cancellationToken)
    {
        var rTask = await context.RepairTasks.Include(x=>x.Parts).FirstOrDefaultAsync(x=>x.Id==request.Id);

        if (rTask is null)
        {
            return ApplicationErrors.RepairTaskNotFound;
        }

        var IncludedInActiveOrders= context.WorkOrders.Any(x=>x.RepairTasks.Any(x=>x.Id==request.Id)&& x.OrderState==Domain.WorkOrders.Enums.OrderState.InProgress);
        if (IncludedInActiveOrders)
        {
            return ApplicationErrors.IncludedInActiveOrders;
        }
        
        var updateResult =rTask.Update(request.Name,request.LaborCost,request.EstimatedDurationInMinutes);
        if (updateResult.IsError)
        {
            return updateResult.Errors;
        }
        var ExistedPartsIds = context.Parts.Select(x=>x.Id);
        var requestPartsIds = request.RepairTaskPartDtos.Select(x=>x.Id);
        var ValidParts = ExistedPartsIds.Any(x=>requestPartsIds.Contains(x));
        if (!ValidParts)
        {
            return ApplicationErrors.PartNotFound;
        }
        var taskPartsIds = rTask.Parts.Select(x=>x.PartId);
        var newIds = requestPartsIds.Except(taskPartsIds);
        var commonIds = requestPartsIds.Intersect(taskPartsIds);
        var finalIds = newIds.Union(commonIds);
        var delete= taskPartsIds.Except(requestPartsIds).ToList();
        rTask.RemoveParts(delete);
        foreach (var item in finalIds)
        {
            rTask.AddPart(item,request.RepairTaskPartDtos.First(x=>x.Id==item).Quantity);
        }
        return Result.Updated;
    }
}