using NerdStore.Core.Data;

namespace NerdStore.Sales.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
    }
}
