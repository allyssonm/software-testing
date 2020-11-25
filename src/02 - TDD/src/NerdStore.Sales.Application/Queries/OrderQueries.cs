using NerdStore.Sales.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Queries
{
    public class OrderQueries : IOrderQueries
    {
        public Task<CartViewModel> GetClientCart(Guid clientId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderViewModel>> GetClientOrders(Guid clientId)
        {
            throw new NotImplementedException();
        }
    }
}
