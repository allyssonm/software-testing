using NerdStore.Core.Data;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public void Add(Order order)
        {
            throw new NotImplementedException();
        }

        public void AddOrderItem(OrderItem order)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetDraftOrderByClientId(Guid clientId)
        {
            throw new NotImplementedException();
        }

        public void Update(Order order)
        {
            throw new NotImplementedException();
        }

        public void UpdateOrderItem(OrderItem order)
        {
            throw new NotImplementedException();
        }
    }
}
