using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add OrderItem New Order")]
        [Trait("TDD", "Order Tests")]
        public void AddOrderItem_NewOrder_ShouldUpdatePrice()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Item", 2, 100);

            // Act
            order.AddOrderItem(orderItem);

            // Assert
            Assert.Equal(200, order.TotalPrice);
        }

        [Fact(DisplayName = "Add Existent OrderItem To Order")]
        [Trait("TDD", "Order Tests")]
        public void AddOrderItem_ExistentOrderItem_ShouldIncrementUnitsAndSumPrices()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Item", 2, 100);
            order.AddOrderItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Test Item", 1, 100);

            // Act
            order.AddOrderItem(orderItem2);

            // Assert
            Assert.Equal(300, order.TotalPrice);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems.FirstOrDefault(x => x.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Add OrderItem Units Above the Allowed")]
        [Trait("TDD", "Order Tests")]
        public void AddOrderItem_OrderItemUnitsAboveTheAllowed_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Item", Order.MAX_ORDER_ITEMS + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.AddOrderItem(orderItem));
        }
    }
}