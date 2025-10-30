using System;

namespace Domain.Models
{
    public class SaleItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string ProductExternalId { get; private set; } = string.Empty;
        public string ProductDescription { get; private set; } = string.Empty;

        public decimal Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal Total { get; private set; }
        public bool IsCancelled { get; private set; } = false;

        protected SaleItem() {}

        public SaleItem(string productExternalId, string productDescription, decimal quantity, decimal unitPrice, decimal discount = 0m)
        {
            ProductExternalId = productExternalId;
            ProductDescription = productDescription;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            CalculateTotal();
        }

        public void UpdateFrom(SaleItem other)
        {
            if (IsCancelled) throw new InvalidOperationException("Cannot modify cancelled item.");
            ProductExternalId = other.ProductExternalId;
            ProductDescription = other.ProductDescription;
            Quantity = other.Quantity;
            UnitPrice = other.UnitPrice;
            Discount = other.Discount;
            CalculateTotal();
        }

        public void Cancel()
        {
            IsCancelled = true;
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            var gross = Quantity * UnitPrice;
            Total = Math.Max(0, gross - Discount);
            if (IsCancelled) Total = 0m;
        }
    }
}