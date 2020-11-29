using NerdStore.Sales.Application.Queries.ViewModels;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Queries
{
    public class OrderQueries : IOrderQueries
    {
        private readonly IOrderRepository _orderRepository;

        public OrderQueries(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<CartViewModel> GetClientCart(Guid clientId)
        {
            var order = await _orderRepository.GetDraftOrderByClientId(clientId);
            if (order == null) return null;

            var cart = new CartViewModel
            {
                ClientId = order.ClientId,
                TotalPrice = order.TotalPrice,
                OrderId = order.Id,
                DiscountValue = order.Discount,
                SubTotal = order.Discount + order.TotalPrice
            };

            if (order.VoucherId != null)
            {
                cart.VoucherCode = order.Voucher.Code;
            }

            foreach (var item in order.OrderItems)
            {
                cart.Items.Add(new CartItemViewModel
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.UnitPrice * item.Quantity
                });
            }

            return cart;
        }

        public async Task<IEnumerable<OrderViewModel>> GetClientOrders(Guid clientId)
        {
            var orders = await _orderRepository.GetListByClientId(clientId);

            orders = orders.Where(p => p.OrderStatus == OrderStatus.Paid || p.OrderStatus == OrderStatus.Cancelled)
                .OrderByDescending(p => p.Code);

            if (!orders.Any()) return null;

            var ordersView = new List<OrderViewModel>();

            foreach (var order in orders)
            {
                ordersView.Add(new OrderViewModel
                {
                    Id = order.Id,
                    TotalPrice = order.TotalPrice,
                    OrderStatus = (int)order.OrderStatus,
                    Code = order.Code,
                    RegisterDate = order.RegisterDate
                });
            }

            return ordersView;
        }
    }
}
