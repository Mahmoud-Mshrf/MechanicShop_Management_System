using System.Reflection.Metadata;

namespace MechanicShop.Domain.Common;

public abstract class Entity
{
    public Guid Id {get;}
    protected Entity()
    {
        
    }
    protected Entity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }

}

public abstract class AuditableEntity : Entity
{
    protected  AuditableEntity()
    {
        
    }

    protected AuditableEntity(Guid id) : base(id)
    {
        
    }

    public DateTimeOffset CreatedAtUtc {get;set;}
    public string? CreatedBy {get;set;}

    public DateTimeOffset LastModifiedUtc {get;set;}

    public string? LastModifiedBy{get;set;}
    
}