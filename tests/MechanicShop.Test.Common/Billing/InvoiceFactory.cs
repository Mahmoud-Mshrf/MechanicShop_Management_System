using MechanicShop.Domain.Common.Results;
using MechanicShop.Domain.WorkOrders.Invoices;

namespace MechanicShop.Test.Common.Billing;

public static class InvoiceFactory
{
    public static Result<Invoice> CreateInvoice(
        Guid? id = null,
        Guid? workOrderId = null,
        List<InvoiceLineItem>? items = null,
        decimal? discount = null,
        decimal? taxAmount = null,
        TimeProvider? timeProvider = null)
    {
        return Invoice.Create(
            id: id ?? Guid.NewGuid(),
            workOrderId: workOrderId ?? Guid.NewGuid(),
            discountAmount: discount ?? 0,
            taxAmount: taxAmount ?? 0,
            timeProvider: timeProvider ?? TimeProvider.System,
            lineItems: items ?? 
            [
                InvoiceLineItem.Create(
                    Guid.NewGuid(),
                    1,
                    "Oil Change",
                    2,
                    50).Value
            ]);
    }
}