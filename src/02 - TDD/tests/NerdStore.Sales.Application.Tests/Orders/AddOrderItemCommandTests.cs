using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Domain;
using System;
using System.Linq;
using Xunit;

namespace NerdStore.Sales.Application.Tests.Orders
{
    public class AddOrderItemCommandTests
    {
        [Fact(DisplayName = "Add Valid OrderItem Command")]
        [Trait("TDD", "Sales - Order Commands")]
        public void AddOrderItemCommand_CommandIsValid_ShouldPassValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Command", 2, 100);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Add Invalid OrderItem Command")]
        [Trait("TDD", "Sales - Order Commands")]
        public void AddOrderItemCommand_CommandIsInvalid_ShouldNotPassValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemCommandValidation.ClientIdErrorMessage, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemCommandValidation.ProductIdErrorMessage, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemCommandValidation.NameErrorMessage, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemCommandValidation.QtyMinErrorMessage, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemCommandValidation.ValueErrorMessage, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Add Quantity Not Allowed OrderItem Command")]
        [Trait("TDD", "Sales - Order Commands")]
        public void AddOrderItemCommand_QuantityIsNotAllowed_ShouldNotPassValidation()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Command", Order.MAX_ORDER_ITEMS + 1, 0);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemCommandValidation.QtyMaxErrorMessage, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
