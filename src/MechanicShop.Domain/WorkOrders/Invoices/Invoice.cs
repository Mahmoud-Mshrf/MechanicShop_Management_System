using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.WorkOrders.Invoices.Enums;

namespace MechanicShop.Domain.WorkOrders.Invoices;

public sealed class Invoice : AuditableEntity
{
    public Guid WorkOrderId {get;}
    public WorkOrder WorkOrder {get;set;}
    public DateTimeOffset IssuedAtUtc {get;}
    public decimal DiscountAmount {get;private set;}
    public decimal TaxAmount {get;private set;}
    public InvoiceStatus Status {get;private set;}
    public DateTimeOffset? PaidAt {get;private set;}
    private readonly List<InvoiceLineItem> _invoiceLineItems = [];
    public IEnumerable<InvoiceLineItem> InvoiceLineItems => _invoiceLineItems.AsReadOnly();
    public decimal SubTotal => _invoiceLineItems.Sum(x=>x.LineTotal);
    public decimal Total => SubTotal - (DiscountAmount + TaxAmount); 

    private Invoice()
    {
        
    }

    private Invoice(Guid id,Guid workOrderId, decimal discountAmount,decimal taxAmount,DateTimeOffset issuedAt,List<InvoiceLineItem> lineItems):base(id)
    {
        WorkOrderId = workOrderId;
        IssuedAtUtc = issuedAt;
        DiscountAmount = discountAmount;
        TaxAmount= taxAmount;
        _invoiceLineItems= lineItems; 
        Status = InvoiceStatus.UnPaid;
    }

    public static Result<Invoice> Create(Guid id,Guid workOrderId, decimal discountAmount,decimal taxAmount,TimeProvider timeProvider,List<InvoiceLineItem> lineItems)
    {
        if (workOrderId == Guid.Empty)
        {
            return InvoiceErrors.WorkOrderIdInvalid;
        }
        if (lineItems.Count ==0 || lineItems is null)
        {
            return InvoiceErrors.LineItemsEmpty;
        }

        return new Invoice(id,workOrderId,discountAmount,taxAmount,timeProvider.GetUtcNow(),lineItems);
    }

    public Result<Updated> ApplyDiscount(decimal amount)
    {
        if (Status!= InvoiceStatus.UnPaid)
        {
            return InvoiceErrors.InvoiceLocked;
        }
        if (amount > SubTotal)
        {
            return  InvoiceErrors.DiscountExceedsSubtotal;
        }
        if ( amount < 0)
        {
            return  InvoiceErrors.DiscountNegative;
        }
        DiscountAmount = amount;
        return Result.Updated;
    }
    public Result<Updated> MarkAsPaid(TimeProvider timeProvider)
    {
        if (Status!= InvoiceStatus.UnPaid)
        {
            return InvoiceErrors.InvoiceLocked;
        }
        Status = InvoiceStatus.Paid;
        PaidAt =timeProvider.GetUtcNow();
        return Result.Updated;
    }
}
