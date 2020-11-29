using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Application.Services;
using NerdStore.Core.Messages.CommonMessages;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Application.Queries;

namespace NerdStore.WebApplication.MVC.Controllers
{
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IOrderQueries _orderQueries;
        private readonly IMediator _mediatorHandler;

        public CartController(INotificationHandler<DomainNotification> notifications,
                                  IProductAppService productAppService,
                                  IMediator mediatorHandler, 
                                  IOrderQueries orderQueries,
                                  IHttpContextAccessor httpContextAccessor) : base(notifications, mediatorHandler, httpContextAccessor)
        {
            _productAppService = productAppService;
            _mediatorHandler = mediatorHandler;
            _orderQueries = orderQueries;
        }

        [HttpGet]
        [Route("my-cart")]
        public async Task<IActionResult> Index()
        {
            return View(await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost]
        [Route("my-cart")]
        public async Task<IActionResult> AddItem(Guid id, int quantity)
        {
            var product = await _productAppService.GetById(id);
            if (product == null) return BadRequest();

            if (product.StockQuantity < quantity)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProductDetail", "Showcase", new { id });
            }

            var command = new AddOrderItemCommand(ClientId, product.Id, product.Name, quantity, product.Price);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            TempData["Erros"] = GetErrorMessages();
            return RedirectToAction("ProductDetail", "Showcase", new { id });
        }

        [HttpPost]
        [Route("remove-item")]
        public async Task<IActionResult> RemoveItem(Guid id)
        {
            var product = await _productAppService.GetById(id);
            if (product == null) return BadRequest();

            var command = new RemoveOrderItemCommand(ClientId, id);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost]
        [Route("update-item")]
        public async Task<IActionResult> UpdateItem(Guid id, int quantity)
        {
            var product = await _productAppService.GetById(id);
            if (product == null) return BadRequest();

            var command = new UpdateOrderItemCommand(ClientId, id, quantity);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost]
        [Route("apply-voucher")]
        public async Task<IActionResult> ApplyVoucher(string voucherCode)
        {
            var command = new ApplyOrderVoucherCommand(ClientId, voucherCode);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _orderQueries.GetClientCart(ClientId));
        }

        [HttpGet]
        [Route("purchase-summary")]
        public async Task<IActionResult> PurchaseSummary()
        {
            return View(await _orderQueries.GetClientCart(ClientId));
        }
    }
}