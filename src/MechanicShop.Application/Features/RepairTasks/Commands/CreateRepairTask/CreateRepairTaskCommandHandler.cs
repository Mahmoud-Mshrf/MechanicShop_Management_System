using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.RepairTasks.Dtos;
using MechanicShop.Application.Features.RepairTasks.Mappers;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.RepairTasks.Commands.CreateRepairTask;

public class CreateRepairTaskCommandHandler(IAppDbContext context, ILogger<CreateRepairTaskCommandHandler> logger, HybridCache cache) : IRequestHandler<CreateRepairTaskCommand, Result<RepairTaskDto>>
{
    public async Task<Result<RepairTaskDto>> Handle(CreateRepairTaskCommand request, CancellationToken cancellationToken)
    {
        var task =await context.RepairTasks.FirstOrDefaultAsync(x=>x.Name!.Trim().ToLower() == request.Name.Trim().ToLower());
        if (task is not null)
        {
            return ApplicationErrors.RepairTaskWithSameNameAlreadyExists;
        }
        var creationResult = RepairTask.Create(Guid.NewGuid(),request.Name,request.LaborCost,request.EstimatedDurationInMinutes);
        if (creationResult.IsError)
        {
            return creationResult.Errors;
        }
        var parts =await context.Parts.ToListAsync();
        List<RepairTaskPart> taskParts = [];
        foreach (var p in request.Dtos)
        {
            if (!parts.Any(x=>x.Id==p.Id))
            {
                return ApplicationErrors.PartNotFound;
            }
            creationResult.Value.AddPart(p.Id,p.Quantity);
        }
        context.RepairTasks.Add(creationResult.Value);
        await cache.RemoveByTagAsync("repair-tasks");
        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("RepairTask created successfully");
        // var creationResult = RepairTask.Create(Guid.NewGuid(),request.Name,request.LaborCost,request.EstimatedDurationInMinutes);
        return creationResult.Value.ToDto();
    }
}
