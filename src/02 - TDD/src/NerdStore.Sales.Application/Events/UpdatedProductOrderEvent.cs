using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events
{
    public class UpdatedProductOrderEvent : Event
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public UpdatedProductOrderEvent(Guid clientId, Guid orderId, Guid productId, int quantity)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
