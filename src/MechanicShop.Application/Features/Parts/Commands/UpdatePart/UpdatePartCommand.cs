using System.Security.Cryptography.X509Certificates;
using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Application.Features.Parts.Dtos;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Parts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

namespace MechanicShop.Application.Features.Parts.Commands.UpdatePart;

public sealed record UpdatePartCommand(Guid Id, UpdatePartRequestDto UpdatePartRequest):IRequest<Result<Updated>>;

public class UpdatePartCommandHandler(IAppDbContext context,HybridCache cache,ILogger<UpdatePartCommandHandler> logger) : IRequestHandler<UpdatePartCommand, Result<Updated>>
{
    public async Task<Result<Updated>> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
    {
        // 1- checking if null
        var part = await context.Parts.FirstOrDefaultAsync(x=>x.Id==request.Id,cancellationToken);
        if (part is null)
        {
            logger.LogWarning("Part with id : {partId} is not found ",request.Id);
            return ApplicationErrors.PartNotFound;
        }
        // 2- checking if included in repairTasks
        var inUse = await context.RepairTasks.SelectMany(x=>x.Parts).AnyAsync(x=>x.PartId==part.Id);
        if (inUse)
        {
            logger.LogWarning("Part with id : {partId} is included in repair tasks so can't be deleted ",part.Id);
            return ApplicationErrors.ThereAreRepairTasksUsedThisPart;
        }
        // 3- updating if it is not included in activeOrders 
        part.Update(request.UpdatePartRequest.Name,request.UpdatePartRequest.Cost);
        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync("Parts");
        return Result.Updated;
    }
}



