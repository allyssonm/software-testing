using MediatR;
using NerdStore.Sales.Application.Events;
using NerdStore.Sales.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands
{
    public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddOrderItemCommand command, CancellationToken cancellationToken)
        {
            var orderItem = new OrderItem(command.ProductId, command.Name, command.Quantity, command.UnitValue);
            var order = Order.OrderFactory.NewOrderDraft(command.ClientId);
            order.AddOrderItem(orderItem);

            _orderRepository.Add(order);

            var @event = new AddedOrderItemEvent(
                order.ClientId,
                order.ClientId,
                command.ProductId,
                command.Name,
                command.UnitValue,
                command.Quantity);

            order.AddEvent(@event);

            return await _orderRepository.UnitOfWork.Commit();
        }
    }
}
