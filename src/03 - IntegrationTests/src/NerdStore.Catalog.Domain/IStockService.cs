using System;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Domain
{
    public interface IStockService : IDisposable
    {
        Task<bool> DebitStock(Guid productId, int quantity);
        Task<bool> Restock(Guid productId, int quantity);
    }
}
