using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Api.Dtos
{
    public record SaleItemDto(Guid Id, string ProductExternalId, string ProductDescription, decimal Quantity, decimal UnitPrice, decimal Discount, decimal Total, bool IsCancelled)
    {
        public static SaleItemDto FromDomain(SaleItem i) => new(i.Id, i.ProductExternalId, i.ProductDescription, i.Quantity, i.UnitPrice, i.Discount, i.Total, i.IsCancelled);
    }

    public record SaleDto(Guid Id, string SaleNumber, DateTime Date, string ClientExternalId, string ClientDescription, string BranchExternalId, string BranchDescription, decimal TotalAmount, bool IsCancelled, IEnumerable<SaleItemDto> Items)
    {
        public static SaleDto FromDomain(Sale s) => new(s.Id, s.SaleNumber, s.Date, s.ClientExternalId, s.ClientDescription, s.BranchExternalId, s.BranchDescription, s.TotalAmount, s.IsCancelled, s.Items.Select(i => SaleItemDto.FromDomain(i)));
    }
}