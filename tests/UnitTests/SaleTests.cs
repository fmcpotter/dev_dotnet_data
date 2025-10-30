using System;
using Bogus;
using Domain.Models;
using FluentAssertions;
using Xunit;

namespace UnitTests
{
    public class SaleTests
    {
        [Fact]
        public void Adding_items_should_calculate_total_correctly()
        {
            // Arrange
            var faker = new Faker();
            var sale = new Sale("S-001", DateTime.UtcNow, "client-1", "Cliente Teste", "branch-1", "Filial A");

            var item1 = new SaleItem("p-1", "Produto 1", 2m, 10m, 0m); // 20
            var item2 = new SaleItem("p-2", "Produto 2", 1m, 5m, 1m); // 4

            // Act
            sale.AddItem(item1);
            sale.AddItem(item2);

            // Assert
            sale.TotalAmount.Should().Be(24m);
        }

        [Fact]
        public void Cancelling_item_should_reduce_total()
        {
            var sale = new Sale("S-002", DateTime.UtcNow, "client-2", "Cliente", "branch-1", "Filial A");
            var item = new SaleItem("p-1", "Prod", 1, 10m, 0);
            sale.AddItem(item);
            sale.TotalAmount.Should().Be(10m);

            sale.CancelItem(item.Id);
            sale.TotalAmount.Should().Be(0m);
        }
    }
}