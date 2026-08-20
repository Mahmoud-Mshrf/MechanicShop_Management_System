using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace MechanicShop.Domain.Common;

public abstract class Entity
{
    public Guid Id {get;}
    private readonly List<DomainEvent> _domainEvents=[];
    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    protected Entity()
    {
        
    }
    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
