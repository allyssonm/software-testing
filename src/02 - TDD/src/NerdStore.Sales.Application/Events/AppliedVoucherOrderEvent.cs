using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Events
{
    public class AppliedVoucherOrderEvent : Event
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid VoucherId { get; private set; }

        public AppliedVoucherOrderEvent(Guid clientId, Guid orderId, Guid voucherId)
        {
            AggregateId = orderId;
            ClientId = clientId;
            OrderId = orderId;
            VoucherId = voucherId;
        }
    }
}
