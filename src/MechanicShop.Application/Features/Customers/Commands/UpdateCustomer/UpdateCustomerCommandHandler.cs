using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<Updated>>{
    public Task<Result<Updated>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
