using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Sales.Application.Events;
using NerdStore.Sales.Domain;
using System.Linq;
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
            if (!IsCommandValid(command)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(command.ClientId);
            var orderItem = new OrderItem(command.ProductId, command.Name, command.Quantity, command.UnitValue);

            if (order == null)
            {
                order = Order.OrderFactory.NewOrderDraft(command.ClientId);
                order.AddOrderItem(orderItem);

                _orderRepository.Add(order);
            }
            else
            {
                var existentOrderItem = order.OrderItemExists(orderItem);
                order.AddOrderItem(orderItem);

                if (existentOrderItem)
                {
                    _orderRepository.UpdateOrderItem(order.OrderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId));
                }
                else
                {
                    _orderRepository.AddOrderItem(orderItem);
                }

                _orderRepository.Update(order);
            }

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

        private bool IsCommandValid(Command command)
        {
            if (command.IsValid()) return true;

            foreach (var error in command.ValidationResult.Errors)
            {
                _mediator.Publish(new DomainNotification(command.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
