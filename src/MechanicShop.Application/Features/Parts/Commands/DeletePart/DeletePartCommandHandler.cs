using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Parts.Commands.DeletePart;

public class DeletePartCommandHandler(IAppDbContext context) : IRequestHandler<DeletePartCommand, Result<Deleted>>
{
    public async Task<Result<Deleted>> Handle(DeletePartCommand request, CancellationToken cancellationToken)
    {
        var part = await context.Parts.FindAsync(request.Id);
        if (part is null)
        {
            return ApplicationErrors.PartNotFound;
        }

        if (context.RepairTasks.Any(x=>x.Parts.Any(x=>x.PartId==request.Id)))
        {
            return ApplicationErrors.ThereAreRepairTasksUsedThisPart;         
        }

        context.Parts.Remove(part);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Deleted;
    }
}