using MechanicShop.Application.Common.Errors;
using MechanicShop.Application.Common.Interfaces;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.RepairTasks.Parts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace MechanicShop.Application.Features.Parts.Commands.CreatePart;

public class CreatePartCommandHandler(IAppDbContext context , HybridCache cache , ILogger<CreatePartCommandHandler> logger) : IRequestHandler<CreatePartCommand, Result<Created>>
{
    public async Task<Result<Created>> Handle(CreatePartCommand request, CancellationToken cancellationToken)
    {
        var part = await context.Parts.FirstOrDefaultAsync(x=>x.Name.Trim().ToLower()==request.Name.Trim().ToLower());
        if (part is not null)
        {
            return ApplicationErrors.PartWithSameNameAlreadyCreated;
        }

        var creationResult = Part.Create(Guid.NewGuid(),request.Name,request.Cost);

        if (creationResult.IsError)
        {
            return creationResult.Errors;
        }

        logger.LogInformation("Part {PartName} was created successfully",request.Name);
        context.Parts.Add(creationResult.Value);
        await context.SaveChangesAsync(cancellationToken);
        await  cache.RemoveByTagAsync("Parts");
        return Result.Created;
        
    }
}