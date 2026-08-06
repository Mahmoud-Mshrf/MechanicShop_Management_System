using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Parts.Commands.DeletePart;

public sealed record DeletePartCommand(Guid Id):IRequest<Result<Deleted>>;