using NerdStore.Core.DomainObjects;
using System;

namespace NerdStore.Sales.Domain
{
    public class OrderItem : Entity
    {
        protected OrderItem() { }

        public OrderItem(Guid productId, string productName, int quantity, decimal unitValue)
        {
            if (quantity < Order.MIN_ORDER_ITEMS) throw new DomainException($"Min of {Order.MIN_ORDER_ITEMS} units per product.");

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitValue;
        }

        public Guid OrderId { get; set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        public Order Order { get; set; }

        internal void AddUnits(int units)
        {
            Quantity += units;
        }

        internal void UpdateUnits(int units)
        {
            Quantity = units;
        }

        internal decimal CalculatePrice()
        {
            return Quantity * UnitPrice;
        }

        internal void LinkOrder(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
