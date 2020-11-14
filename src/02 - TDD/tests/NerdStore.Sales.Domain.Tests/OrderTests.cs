using NerdStore.Core.DomainObjects;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add OrderItem New Order")]
        [Trait("TDD", "Sales - Order")]
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
        [Trait("TDD", "Sales - Order")]
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
        [Trait("TDD", "Sales - Order")]
        public void AddOrderItem_OrderItemUnitsAboveTheAllowed_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Item", Order.MAX_ORDER_ITEMS + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.AddOrderItem(orderItem));
        }

        [Fact(DisplayName = "Add OrderItem Units Above the Allowed")]
        [Trait("TDD", "Sales - Order")]
        public void AddOrderItem_ExistentOrderItemUnitsAboveTheAllowed_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Item", 1, 100);
            var orderItem2 = new OrderItem(productId, "Test Item", Order.MAX_ORDER_ITEMS, 100);
            order.AddOrderItem(orderItem);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.AddOrderItem(orderItem2));
        }

        [Fact(DisplayName = "Update OrderItem Inexistent")]
        [Trait("TDD", "Sales - Order")]
        public void UpdateOrderItem_OrderItemInexistentInOrder_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItemUpdated = new OrderItem(Guid.NewGuid(), "Test Item", 5, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateOrderItem(orderItemUpdated));
        }

        [Fact(DisplayName = "Update OrderItem Valid")]
        [Trait("TDD", "Sales - Order")]
        public void UpdateOrderItem_OrderItemValid_ShouldUpdateQuantity()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Item", 2, 100);
            order.AddOrderItem(orderItem);
            var orderItemUpdated = new OrderItem(productId, "Test Item", 5, 100);
            var newQuantity = orderItemUpdated.Quantity;

            // Act
            order.UpdateOrderItem(orderItemUpdated);

            // Assert
            Assert.Equal(newQuantity, order.OrderItems.FirstOrDefault(x => x.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Update OrderItem Validate TotalPrice")]
        [Trait("TDD", "Sales - Order")]
        public void UpdateOrderItem_OrderWithSeveralProducts_ShouldUpdateTotalPrice()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Foo", 2, 100);
            var orderItem2 = new OrderItem(productId, "Test Item", 3, 15);
            order.AddOrderItem(orderItem);
            order.AddOrderItem(orderItem2);

            var orderItemUpdated = new OrderItem(productId, "Test Item", 5, 15);
            var totalPrice = orderItem.Quantity * orderItem.UnitPrice +
                             orderItemUpdated.Quantity * orderItemUpdated.UnitPrice;

            // Act
            order.UpdateOrderItem(orderItemUpdated);

            // Assert
            Assert.Equal(totalPrice, order.TotalPrice);
        }

        [Fact(DisplayName = "Update OrderItem Units Above the Allowed")]
        [Trait("TDD", "Sales - Order")]
        public void UpdateOrderItem_ExistentOrderItemUnitsAboveTheAllowed_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Test Item", 3, 15);
            order.AddOrderItem(orderItem);

            var orderItemUpdated = new OrderItem(productId, "Test Item", Order.MAX_ORDER_ITEMS, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateOrderItem(orderItemUpdated));
        }

        [Fact(DisplayName = "Remove OrderItem Inexistent")]
        [Trait("TDD", "Sales - Order")]
        public void RemoveOrderItem_OrderItemInexistentInOrder_ShouldReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Item", 5, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.RemoveOrderItem(orderItem));
        }

        [Fact(DisplayName = "Remove OrderItem Existent Updates TotalPrice")]
        [Trait("TDD", "Sales - Order")]
        public void RemoveOrderItem_ExistentOrderItem_ShouldUpdateTotalPrice()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Item", 2, 100);
            var orderItem2 = new OrderItem(productId, "Test Item", 3, 15);
            order.AddOrderItem(orderItem);
            order.AddOrderItem(orderItem2);

            var totalPrice = orderItem2.Quantity * orderItem2.UnitPrice;

            // Act
            order.RemoveOrderItem(orderItem);

            // Assert
            Assert.Equal(totalPrice, order.TotalPrice);
        }
    }
}