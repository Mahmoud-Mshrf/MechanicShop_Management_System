using MechanicShop.Application.Features.Parts.Dtos;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Parts.Commands.UpdatePart;

public sealed record UpdatePartCommand(Guid Id, UpdatePartRequestDto UpdatePartRequest):IRequest<Result<Updated>>;




