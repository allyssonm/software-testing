using MediatR;
using NerdStore.Core.DomainObjects;
using System;
using System.Threading.Tasks;

namespace NerdStore.Catalog.Domain
{
    public class StockService : IStockService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;

        public StockService(IProductRepository productRepository,
                              IMediator mediator)
        {
            _productRepository = productRepository;
            _mediator = mediator;
        }

        public async Task<bool> DebitStock(Guid productId, int quantity)
        {
            if (!await DebitStockItem(productId, quantity)) return false;

            return await _productRepository.UnitOfWork.Commit();
        }

        private async Task<bool> DebitStockItem(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) return false;

            if (!product.HasStock(quantity))
            {
                await _mediator.Publish(new DomainNotification("Stock", $"Product - {product.Name} out of stock"));
                return false;
            }

            product.DebitStock(quantity);
            _productRepository.Update(product);
            return true;
        }

        public async Task<bool> Restock(Guid productId, int quantity)
        {
            var success = await RestockItem(productId, quantity);

            if (!success) return false;

            return await _productRepository.UnitOfWork.Commit();
        }

        private async Task<bool> RestockItem(Guid productId, int quantity)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null) return false;
            product.Restock(quantity);

            _productRepository.Update(product);

            return true;
        }

        public void Dispose()
        {
            _productRepository.Dispose();
        }
    }
}
