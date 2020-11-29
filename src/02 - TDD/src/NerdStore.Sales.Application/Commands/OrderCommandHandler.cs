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
    public class OrderCommandHandler :
        IRequestHandler<AddOrderItemCommand, bool>,
        IRequestHandler<UpdateOrderItemCommand, bool>,
        IRequestHandler<RemoveOrderItemCommand, bool>,
        IRequestHandler<ApplyOrderVoucherCommand, bool>
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

        public async Task<bool> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
        {
            if (!IsCommandValid(command)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(command.ClientId);

            if (order == null)
            {
                await _mediator.Publish(new DomainNotification("order", "Pedido não encontrado!"), cancellationToken);
                return false;
            }

            var orderItem = await _orderRepository.GetOrderItemByOrder(order.Id, command.ProductId);

            if (!order.OrderItemExists(orderItem))
            {
                await _mediator.Publish(new DomainNotification("order", "Item do pedido não encontrado!"), cancellationToken);
                return false;
            }

            order.UpdateUnits(orderItem, command.Quantity);
            order.AddEvent(new UpdatedProductOrderEvent(command.ClientId, order.Id, command.ProductId, command.Quantity));

            _orderRepository.UpdateOrderItem(orderItem);
            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(RemoveOrderItemCommand command, CancellationToken cancellationToken)
        {
            if (!IsCommandValid(command)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(command.ClientId);

            if (order == null)
            {
                await _mediator.Publish(new DomainNotification("order", "Pedido não encontrado!"), cancellationToken);
                return false;
            }

            var orderItem = await _orderRepository.GetOrderItemByOrder(order.Id, command.ProductId);

            if (orderItem != null && !order.OrderItemExists(orderItem))
            {
                await _mediator.Publish(new DomainNotification("order", "Item do pedido não encontrado!"), cancellationToken);
                return false;
            }

            order.RemoveOrderItem(orderItem);
            order.AddEvent(new RemovedProductOrderEvent(command.ClientId, order.Id, command.ProductId));

            _orderRepository.RemoveItem(orderItem);
            _orderRepository.Update(order);

            return await _orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(ApplyOrderVoucherCommand command, CancellationToken cancellationToken)
        {
            if (!IsCommandValid(command)) return false;

            var order = await _orderRepository.GetDraftOrderByClientId(command.ClientId);

            if (order == null)
            {
                await _mediator.Publish(new DomainNotification("order", "Pedido não encontrado!"), cancellationToken);
                return false;
            }

            var voucher = await _orderRepository.GetVoucherByCode(command.VoucherCode);

            if (voucher == null)
            {
                await _mediator.Publish(new DomainNotification("order", "Voucher não encontrado!"), cancellationToken);
                return false;
            }

            var voucherAplicacaoValidation = order.ApplyVoucher(voucher);
            if (!voucherAplicacaoValidation.IsValid)
            {
                foreach (var error in voucherAplicacaoValidation.Errors)
                {
                    await _mediator.Publish(new DomainNotification(error.ErrorCode, error.ErrorMessage), cancellationToken);
                }

                return false;
            }

            order.AddEvent(new AppliedVoucherOrderEvent(command.ClientId, order.Id, voucher.Id));

            _orderRepository.Update(order);

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
