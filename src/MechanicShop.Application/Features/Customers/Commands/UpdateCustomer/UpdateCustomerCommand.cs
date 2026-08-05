using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;

public sealed record UpdateCustomerCommand(Guid Id,string Name , string PhoneNumber,string Email,List<UpdateVehicleCommand> vehicles):IRequest<Result<Updated>>;
