using System;

namespace Domain.Events
{
    public record PurchaseCreated(Guid SaleId, string SaleNumber, DateTime Date, decimal TotalAmount);
    public record PurchaseUpdated(Guid SaleId, string SaleNumber, DateTime Date, decimal TotalAmount);
    public record PurchaseCancelled(Guid SaleId, string SaleNumber, DateTime Date);
    public record ItemCancelled(Guid SaleId, Guid ItemId, string ProductExternalId);
}