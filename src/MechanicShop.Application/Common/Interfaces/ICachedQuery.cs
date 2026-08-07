using MediatR;

namespace MechanicShop.Application.Common.Interfaces;

public interface ICachedQuery
{
    string CachingKey {get;}
    string[] Tags {get;}
    TimeSpan Expiration {get;}
}

public interface ICachedQuery<TResponse> : ICachedQuery,IRequest<TResponse>;