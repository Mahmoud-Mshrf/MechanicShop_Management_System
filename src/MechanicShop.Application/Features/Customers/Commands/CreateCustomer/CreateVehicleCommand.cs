using MechanicShop.Application.Features.Customers.Commands.Dtos;
using MediatR;

namespace MechanicShop.Application.Features.Customers.Commands.CreateCustomer;

public sealed record CreateVehicleCommand(string Make , string Model , int Year,string LicensePlate):IRequest<VehicleDto>;