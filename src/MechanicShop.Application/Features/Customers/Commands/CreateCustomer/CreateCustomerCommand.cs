using MechanicShop.Application.Features.Customers.Commands.Dtos;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.Customers.Vehicles;
using MediatR;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateCustomerCommand(string Name , string PhoneNumber,string Email,List<CreateVehicleCommand> vehicles):IRequest<Result<CustomerDto>>;
