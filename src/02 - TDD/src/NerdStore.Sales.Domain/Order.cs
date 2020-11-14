using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Sales.Domain
{
    public class Order
    {
        private readonly List<OrderItem> _orderItems;
        public static int MAX_ORDER_ITEMS => 15;
        public static int MIN_ORDER_ITEMS => 1;

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public Guid ClientId { get; set; }
        public decimal TotalPrice { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        private void CalculateOrderPrice()
        {
            TotalPrice = OrderItems.Sum(x => x.CalculatePrice());
        }

        private bool OrderItemExists(OrderItem orderItem)
        {
            return _orderItems.Any(x => x.ProductId == orderItem.ProductId);
        }

        private void ValidateOrderItemInexistent(OrderItem orderItem)
        {
            if (!OrderItemExists(orderItem)) throw new DomainException($"Order item doesn't exist in order.");
        }

        private void ValidateOrderItemQuantityAllowed(OrderItem orderItem)
        {
            var itemsQuantity = orderItem.Quantity;

            if (OrderItemExists(orderItem))
            {
                var existentItem = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);
                itemsQuantity += existentItem.Quantity;
            }

            if (itemsQuantity > MAX_ORDER_ITEMS) throw new DomainException($"Max of {MAX_ORDER_ITEMS} units per product.");
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            ValidateOrderItemQuantityAllowed(orderItem);

            if (OrderItemExists(orderItem))
            {
                var existentItem = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);

                existentItem.AddUnits(orderItem.Quantity);
                orderItem = existentItem;

                _orderItems.Remove(existentItem);
            }

            _orderItems.Add(orderItem);
            CalculateOrderPrice();
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            ValidateOrderItemInexistent(orderItem);
            ValidateOrderItemQuantityAllowed(orderItem);

            var existentItem = _orderItems.FirstOrDefault(x => x.ProductId == orderItem.ProductId);

            _orderItems.Remove(existentItem);
            _orderItems.Add(orderItem);

            CalculateOrderPrice();
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            ValidateOrderItemInexistent(orderItem);

            _orderItems.Remove(orderItem);

            CalculateOrderPrice();
        }

        public void MarkAsDraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public static class OrderFactory
        {
            public static Order NewOrderDraft(Guid clientId)
            {
                var order = new Order()
                {
                    ClientId = clientId,
                };

                order.MarkAsDraft();
                return order;
            }
        }
    }

    public class OrderItem
    {
        public OrderItem(Guid productId, string productName, int quantity, decimal unitValue)
        {
            if (quantity < Order.MIN_ORDER_ITEMS) throw new DomainException($"Min of {Order.MIN_ORDER_ITEMS} units per product.");

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitPrice = unitValue;
        }

        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }

        internal void AddUnits(int units)
        {
            Quantity += units;
        }

        internal decimal CalculatePrice()
        {
            return Quantity * UnitPrice;
        }
    }

    public enum OrderStatus
    {
        Draft = 0,
        Initialized = 1,
        Paid = 4,
        Delivered = 5,
        Cancelled = 6
    }
}
