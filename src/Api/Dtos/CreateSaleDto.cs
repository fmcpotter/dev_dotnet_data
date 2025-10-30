using System;
using System.Collections.Generic;

namespace Api.Dtos
{
    public record CreateSaleItemDto(string ProductExternalId, string ProductDescription, decimal Quantity, decimal UnitPrice, decimal Discount = 0m);
    public record CreateSaleDto(string SaleNumber, DateTime Date, string ClientExternalId, string ClientDescription, string BranchExternalId, string BranchDescription, IEnumerable<CreateSaleItemDto> Items);
}