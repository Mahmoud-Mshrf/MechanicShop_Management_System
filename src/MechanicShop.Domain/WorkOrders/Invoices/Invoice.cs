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

    public static Result<InvoiceLineItem> Create(Guid invoiceId, int lineNumber, string? description, int quantity, decimal unitPrice)
    {
        if (Guid.Empty == invoiceId)
        {
            return InvoiceLineItemErrors.InvoiceIdRequired;
        }
        if (lineNumber <= 0)
        {
            return InvoiceLineItemErrors.LineNumberInvalid;
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            return InvoiceLineItemErrors.DescriptionRequired;
        }
        if (quantity <=0)
        {
            return InvoiceLineItemErrors.QuantityInvalid;
        }
        if (unitPrice <=0)
        {
            return InvoiceLineItemErrors.UnitPriceInvalid;
        }

        return new InvoiceLineItem(invoiceId,lineNumber,description,quantity,unitPrice);
    }

}
public static class InvoiceLineItemErrors
{
    public static Error InvoiceIdRequired => Error.Validation(
        code: "InvoiceLineItemErrors.InvoiceIdRequired",
        description: "InvoiceId is required.");

    public static Error LineNumberInvalid => Error.Validation(
        code: "InvoiceLineItemErrors.LineNumberInvalid",
        description: "Line number must be greater than 0.");

    public static Error DescriptionRequired => Error.Validation(
        code: "InvoiceLineItemErrors.DescriptionRequired",
        description: "Description is required.");

    public static Error QuantityInvalid => Error.Validation(
        code: "InvoiceLineItemErrors.QuantityInvalid",
        description: "Quantity must be greater than 0.");

    public static Error UnitPriceInvalid => Error.Validation(
        code: "InvoiceLineItemErrors.UnitPriceInvalid",
        description: "Unit price must be greater than 0.");
}