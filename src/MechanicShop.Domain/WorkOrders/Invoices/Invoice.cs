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

}

public class InvoiceLineItem
{
    public Guid InvoiceId {get;private set;}
    public int LineNumber {get;private set;}
    public string? Description {get;private set;}
    public int Quantity {get; private set;}
    public decimal UnitPrice {get;private set;}
    public decimal LineTotal => UnitPrice * Quantity ;

    private InvoiceLineItem()
    {
    }
    private InvoiceLineItem(Guid invoiceId, int lineNumber, string? description, int quantity, decimal unitPrice)
    {
        InvoiceId = invoiceId;
        LineNumber = lineNumber;
        Description = description;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    // public static Result<InvoiceLineItem> Create(Guid invoiceId, int lineNumber, string? description, int quantity, decimal unitPrice)
    // {
    //     if (Guid.Empty == invoiceId)
    //     {
    //         return InvoiceLineItemErrors.EmptyInvoiceId;
    //     }
    //     if (lineNumber <= 0)
    //     {
    //         return InvoiceLineItemErrors.InvalidLineNumber;
    //     }
    //     if (string.IsNullOrWhiteSpace(description))
    //     {
    //         return InvoiceLineItemErrors.DescriptionIsRequired;
    //     }
    //     if (quantity <=0)
    //     {
    //         return InvoiceLineItemErrors.InvalidQuantity;
    //     }
    //     if (unitPrice <=0)
    //     {
    //         return InvoiceLineItemErrors.InvalidUnitPrice;
    //     }
    // }
}