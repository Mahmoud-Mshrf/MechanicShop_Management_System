using System.Security.Cryptography.X509Certificates;
using MechanicShop.Application.Features.Parts.Dtos;
using MechanicShop.Domain.Common.Results;
using MediatR;

namespace MechanicShop.Application.Features.Parts.Commands.UpdatePart;

public sealed record UpdatePartCommand(Guid Id, UpdatePartRequestDto UpdatePartRequest):IRequest<Result<Updated>>;

// public class UpdatePartCommandHanlder : IRequestHandler<UpdatePartCommand, Result<Updated>>
// {
//     public Task<Result<Updated>> Handle(UpdatePartCommand request, CancellationToken cancellationToken)
//     {
//         // 1- checking if null
//         var part = 
//         // 2- checking if included in repairTasks and if it is included then check if these work orders is active(inProgress or scheduled)
//         // 3- deleting if it is not included in activeOrders 

    
//     }
// }



