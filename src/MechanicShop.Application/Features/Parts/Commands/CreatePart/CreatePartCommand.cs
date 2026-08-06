using MechanicShop.Application.Features.RepairTasks.Dtos;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Parts.Commands.CreatePart;

public sealed record CreatePartCommand(string Name , decimal Cost):IRequest<Result<Created>>;

