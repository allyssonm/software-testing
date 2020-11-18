using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NerdStore.Sales.Domain
{
    public class Order : Entity, IAggregateRoot
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
        public decimal Discount { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public Voucher Voucher { get; private set; }
        public bool UsedVoucher { get; private set; }

        private void CalculateOrderPrice()
        {
            TotalPrice = OrderItems.Sum(x => x.CalculatePrice());
            CalculateTotalPriceWithVoucherDiscount();
        }

        public bool OrderItemExists(OrderItem orderItem)
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

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var validationResult = voucher.ValidateApplicability();
            if (!validationResult.IsValid) return validationResult;

            Voucher = voucher;
            UsedVoucher = true;
            CalculateOrderPrice();

            return validationResult;
        }

        public void CalculateTotalPriceWithVoucherDiscount()
        {
            if (!UsedVoucher) return;

            decimal discount = 0;
            var value = TotalPrice;

            if (Voucher.VoucherDiscountType == VoucherDiscountType.Percent)
            {
                if (Voucher.DiscountPercent.HasValue)
                {
                    discount = (value * Voucher.DiscountPercent.Value) / 100;
                    value -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    value -= discount;
                }
            }

            TotalPrice = value < 0 ? 0 : value;
            Discount = discount;
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
}
