using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Parts.Commands.UpdatePart;

public sealed record UpdatePartCommand(Guid Id, UpdatePartRequest UpdatePartRequest):IRequest<Result<Updated>>;

public class UpdatePartRequest
{
    public string Name {get;set;}
    public decimal Cost {get;set;}
}