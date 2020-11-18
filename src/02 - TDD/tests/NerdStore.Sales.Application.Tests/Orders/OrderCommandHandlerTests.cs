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
        private readonly Guid _clientId;
        private readonly Guid _productId;
        private readonly Order _order;
        private readonly AutoMocker _mocker;
        private readonly OrderCommandHandler _orderHandler;

        public OrderCommandHandlerTests()
        {
            _clientId = Guid.NewGuid();
            _productId = Guid.NewGuid();

            _order = Order.OrderFactory.NewOrderDraft(_clientId);

            _mocker = new AutoMocker();
            _orderHandler = _mocker.CreateInstance<OrderCommandHandler>();
        }

        [Fact(DisplayName = "Add New OrderItem to Order Returns Success")]
        [Trait("TDD", "Sales - Order Command Handler")]
        public async Task AddOrderItem_NewOrder_ShouldReturnSuccess()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Test Item", 2, 100);

            _mocker.GetMock<IOrderRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.Add(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add New OrderItem to Draft Order Returns Success")]
        [Trait("TDD", "Sales - Order Command Handler")]
        public async Task AddOrderItem_NewOrderItemToDraftOrder_ShouldReturnSuccess()
        {
            // Arrange
            var orderItem = new OrderItem(Guid.NewGuid(), "Test Item", 2, 100);
            _order.AddOrderItem(orderItem);

            var orderCommand = new AddOrderItemCommand(_clientId, Guid.NewGuid(), "Test Item", 2, 100);

            _mocker.GetMock<IOrderRepository>().Setup(x => x.GetDraftOrderByClientId(_clientId)).Returns(Task.FromResult(_order));
            _mocker.GetMock<IOrderRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.AddOrderItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add Existent OrderItem to Draft Order Returns Success")]
        [Trait("TDD", "Sales - Order Command Handler")]
        public async Task AddOrderItem_ExistentOrderItemToDraftOrder_ShouldReturnSuccess()
        {
            // Arrange
            var existentOrderItem = new OrderItem(_productId, "Test Item", 2, 100);
            _order.AddOrderItem(existentOrderItem);

            var orderCommand = new AddOrderItemCommand(_clientId, _productId, "Test Item", 2, 100);

            _mocker.GetMock<IOrderRepository>().Setup(x => x.GetDraftOrderByClientId(_clientId)).Returns(Task.FromResult(_order));
            _mocker.GetMock<IOrderRepository>().Setup(x => x.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UpdateOrderItem(It.IsAny<OrderItem>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.Update(It.IsAny<Order>()), Times.Once);
            _mocker.GetMock<IOrderRepository>().Verify(x => x.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Add OrderItem Invalid Command")]
        [Trait("TDD", "Sales - Order Command Handler")]
        public async Task AddOrderItem_InvalidCommand_ShouldReturnFalseAndThrowEventNotifications()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = await _orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mocker.GetMock<IMediator>().Verify(x => x.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
