using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace MechanicShop.Application.Features.Parts.Commands.DeletePart;

public class DeletePartCommandHandler(IAppDbContext context,HybridCache cache) : IRequestHandler<DeletePartCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeletePartCommand request, CancellationToken cancellationToken)
    {
         // 1- checking if null
        var part = await context.Parts.FindAsync(request.Id);
        if (part is null)
        {
            return ApplicationErrors.PartNotFound;
        }
        // 2- checking if included in repairTasks
        if (context.RepairTasks.Any(x=>x.Parts.Any(x=>x.PartId==request.Id)))
        {
            return ApplicationErrors.ThereAreRepairTasksUsedThisPart;         
        }
        // 3- deleting if it is not included in any repair task
        context.Parts.Remove(part);

        await context.SaveChangesAsync(cancellationToken);
        await cache.RemoveByTagAsync("Parts");
        return Result.Deleted;
    }
}