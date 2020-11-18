using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events
{
    public class AddedOrderItemEvent : Event
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductNome { get; set; }
        public decimal UnitValue { get; private set; }
        public int Quantity { get; private set; }

        public AddedOrderItemEvent(Guid clientId, Guid orderId, Guid productId, string productName, decimal unitValue, int quanitity)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
            ProductId = productId;
            ProductNome = productName;
            UnitValue = unitValue;
            Quantity = quanitity;
        }
    }
}
