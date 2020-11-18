using NerdStore.Core.Data;
using System;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void Update(Order order);
        Task<Order> GetDraftOrderByClientId(Guid clientId);
        void AddOrderItem(OrderItem order);
        void UpdateOrderItem(OrderItem order);
    }
}
