
using MechanicShop.Domain.WorkOrders.Invoices;

namespace MechanicShop.Application.Common.Interfaces;

public interface IInvoicePdfGenerator
{
    byte[] Generate(Invoice invoice);
}