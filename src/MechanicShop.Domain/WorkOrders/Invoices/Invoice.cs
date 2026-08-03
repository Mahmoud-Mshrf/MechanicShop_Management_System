using MechanicShop.Domain.Common;
using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.WorkOrders.Invoices.Enums;

namespace MechanicShop.Domain.WorkOrders.Invoices;

public sealed class Invoice : AuditableEntity
{
    public Guid WorkOrderId {get;private set;}
    public DateTimeOffset IssuedAtUtc {get;private set;}
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

    private Invoice(Guid id,Guid workOrderId, decimal discountAmount,decimal taxAmount,List<InvoiceLineItem> lineItems):base(id)
    {
        WorkOrderId = workOrderId;
        IssuedAtUtc = DateTimeOffset.UtcNow;
        DiscountAmount = discountAmount;
        TaxAmount= taxAmount;
        _invoiceLineItems= lineItems; 
        Status = InvoiceStatus.UnPaid;
    }

    public static Result<Invoice> Create(Guid id,Guid workOrderId, decimal discountAmount,decimal taxAmount,List<InvoiceLineItem> lineItems)
    {
        if (workOrderId == Guid.Empty)
        {
            return InvoiceErrors.WorkOrderIdInvalid;
        }
        if (discountAmount <= 0)
        {
            return InvoiceErrors.DiscountNegative;
        }
        if (lineItems.Count <=0 || lineItems is null)
        {
            return InvoiceErrors.LineItemsEmpty;
        }

        return new Invoice(id,workOrderId,discountAmount,taxAmount,lineItems);
    }

    public Result<Updated> ApplyDiscount(decimal amount)
    {
        if (Status!= InvoiceStatus.UnPaid)
        {
            return InvoiceErrors.InvoiceLocked;
        }
        if (amount >= SubTotal || amount <= 0)
        {
            return  InvoiceErrors.DiscountExceedsSubtotal;
        }
        if ( amount <= 0)
        {
            return  InvoiceErrors.DiscountNegative;
        }
        DiscountAmount = amount;

        return Result.Updated;
    }
    public Result<Updated> MarkAsPaid()
    {
        if (Status!= InvoiceStatus.UnPaid)
        {
            return InvoiceErrors.InvoiceLocked;
        }
        PaidAt = DateTimeOffset.UtcNow;
        
        return Result.Updated;
    }
}

public static class InvoiceErrors
{
    public static readonly Error WorkOrderIdInvalid = Error.Validation(
        code: "Invoice.WorkOrderId.Invalid",
        description: "WorkOrderId is invalid");

    public static readonly Error LineItemsEmpty = Error.Validation(
        code: "Invoice.LineItems.Empty",
        description: "Invoice must have line items");

    public static readonly Error InvoiceLocked = Error.Validation(
        code: "Invoice.Locked",
        description: "Invoice is locked");

    public static readonly Error DiscountNegative = Error.Validation(
        code: "Invoice.Discount.Negative",
        description: "Discount cannot be negative");

    public static readonly Error DiscountExceedsSubtotal = Error.Validation(
        code: "Invoice.Discount.ExceedsSubtotal",
        description: "Discount exceeds subtotal");
}