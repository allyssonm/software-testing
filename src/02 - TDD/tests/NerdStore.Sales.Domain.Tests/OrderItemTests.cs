using NerdStore.Core.DomainObjects;
using System;
using Xunit;

namespace NerdStore.Sales.Domain.Tests
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "New OrderItem Units Under the Allowed")]
        [Trait("TDD", "Order Item Tests")]
        public void AddOrderItem_OrderItemUnitsUnderTheAllowed_ShouldReturnException()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Test Item", Order.MIN_ORDER_ITEMS - 1, 100));
        }
    }
}
