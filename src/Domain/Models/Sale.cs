using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Models
{
    public class Sale
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string SaleNumber { get; private set; } = string.Empty;
        public DateTime Date { get; private set; }
        // External identity for client (desnormalização do descritivo)
        public string ClientExternalId { get; private set; } = string.Empty;
        public string ClientDescription { get; private set; } = string.Empty;
        public string BranchExternalId { get; private set; } = string.Empty;
        public string BranchDescription { get; private set; } = string.Empty;

        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; } = false;

        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        protected Sale() { } // EF Core

        public Sale(string saleNumber, DateTime date, string clientExternalId, string clientDescription, string branchExternalId, string branchDescription)
        {
            SaleNumber = saleNumber;
            Date = date;
            ClientExternalId = clientExternalId;
            ClientDescription = clientDescription;
            BranchExternalId = branchExternalId;
            BranchDescription = branchDescription;
            RecalculateTotal();
        }

        public void AddItem(SaleItem item)
        {
            if (IsCancelled) throw new InvalidOperationException("Cannot add item to cancelled sale.");
            _items.Add(item);
            RecalculateTotal();
        }

        public void UpdateItem(SaleItem updated)
        {
            var existing = _items.FirstOrDefault(i => i.Id == updated.Id);
            if (existing == null) throw new ArgumentException("Item not found");
            existing.UpdateFrom(updated);
            RecalculateTotal();
        }

        public void CancelItem(Guid itemId)
        {
            var item = _items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) throw new ArgumentException("Item not found");
            item.Cancel();
            RecalculateTotal();
        }

        public void Cancel()
        {
            IsCancelled = true;
        }

        public void UpdateHeader(string clientExternalId, string clientDescription, string branchExternalId, string branchDescription, DateTime date)
        {
            if (IsCancelled) throw new InvalidOperationException("Cannot modify cancelled sale.");
            ClientExternalId = clientExternalId;
            ClientDescription = clientDescription;
            BranchExternalId = branchExternalId;
            BranchDescription = branchDescription;
            Date = date;
        }

        private void RecalculateTotal()
        {
            TotalAmount = _items.Where(i => !i.IsCancelled).Sum(i => i.Total);
        }
    }
}