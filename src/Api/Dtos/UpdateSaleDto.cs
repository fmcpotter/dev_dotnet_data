using System;
using System.Collections.Generic;

namespace Api.Dtos
{
    public record UpdateSaleItemDto(Guid Id, string ProductExternalId, string ProductDescription, decimal Quantity, decimal UnitPrice, decimal Discount = 0m);
    public record UpdateSaleDto(DateTime Date, string ClientExternalId, string ClientDescription, string BranchExternalId, string BranchDescription, IEnumerable<UpdateSaleItemDto> Items);
}