using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events
{
    public class RemovedProductOrderEvent : Event
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public RemovedProductOrderEvent(Guid clientId, Guid orderId, Guid productId)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
            ProductId = productId;
        }
    }
}
