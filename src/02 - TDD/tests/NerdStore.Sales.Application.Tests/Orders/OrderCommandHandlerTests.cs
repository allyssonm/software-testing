using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Sales.Application.Tests.Orders
{
    public class OrderCommandHandlerTests
    {
        [Fact(DisplayName = "Add New OrderItem Returns Success")]
        [Trait("TDD", "Sales - Order Command Handler")]
        public async Task AddOrderItem_NewOrder_ShouldReturnSuccess()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Item", 2, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }
    }
}
