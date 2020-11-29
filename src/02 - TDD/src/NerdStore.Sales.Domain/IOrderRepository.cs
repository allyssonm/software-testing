using NerdStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void Update(Order order);
        Task<Order> GetDraftOrderByClientId(Guid clientId);
        Task<IEnumerable<Order>> GetListByClientId(Guid clientId);

        void AddOrderItem(OrderItem order);
        void UpdateOrderItem(OrderItem order);
        void RemoveItem(OrderItem order);
        Task<OrderItem> GetOrderItemByOrder(Guid orderId, Guid productId);

        Task<Voucher> GetVoucherByCode(string code);
    }
}
