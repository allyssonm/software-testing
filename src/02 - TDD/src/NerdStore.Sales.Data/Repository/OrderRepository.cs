using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Sales.Domain;
using System;
using System.Threading.Tasks;

namespace NerdStore.Sales.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SalesContext _context;

        public OrderRepository(SalesContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public void Add(Order order)
        {
            _context.Orders.Add(order);
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
        }

        public async Task<Order> GetDraftOrderByClientId(Guid clientId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.ClientId == clientId && p.OrderStatus == OrderStatus.Draft);
            if (order == null) return null;

            await _context.Entry(order)
                .Collection(i => i.OrderItems).LoadAsync();

            if (order.VoucherId != null)
            {
                await _context.Entry(order)
                    .Reference(i => i.Voucher).LoadAsync();
            }

            return order;
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public void UpdateOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
