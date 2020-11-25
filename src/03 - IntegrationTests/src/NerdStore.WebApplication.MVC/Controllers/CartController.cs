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
        private readonly IProductAppService _produtoAppService;
        private readonly IOrderQueries _orderQueries;
        private readonly IMediator _mediatorHandler;

        public CartController(INotificationHandler<DomainNotification> notifications,
                                  IProductAppService produtoAppService,
                                  IMediator mediatorHandler, 
                                  IOrderQueries pedidoQueries,
                                  IHttpContextAccessor httpContextAccessor) : base(notifications, mediatorHandler, httpContextAccessor)
        {
            _produtoAppService = produtoAppService;
            _mediatorHandler = mediatorHandler;
            _orderQueries = pedidoQueries;
        }

        [HttpGet]
        [Route("meu-carrinho")]
        public async Task<IActionResult> Index()
        {
            return View(await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost]
        [Route("meu-carrinho")]
        public async Task<IActionResult> AdicionarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.GetById(id);
            if (produto == null) return BadRequest();

            if (produto.StockQuantity < quantidade)
            {
                TempData["Erro"] = "Produto com estoque insuficiente";
                return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
            }

            var command = new AddOrderItemCommand(ClientId, produto.Id, produto.Name, quantidade, produto.Price);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            TempData["Erros"] = GetErrorMessages();
            return RedirectToAction("ProdutoDetalhe", "Vitrine", new { id });
        }

        [HttpPost]
        [Route("remover-item")]
        public async Task<IActionResult> RemoverItem(Guid id)
        {
            var produto = await _produtoAppService.GetById(id);
            if (produto == null) return BadRequest();

            var command = new RemoveOrderItemCommand(ClientId, id);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost]
        [Route("atualizar-item")]
        public async Task<IActionResult> AtualizarItem(Guid id, int quantidade)
        {
            var produto = await _produtoAppService.GetById(id);
            if (produto == null) return BadRequest();

            var command = new UpdateOrderItemCommand(ClientId, id, quantidade);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost]
        [Route("aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
        {
            var command = new ApplyOrderVoucherCommand(ClientId, voucherCodigo);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                return RedirectToAction("Index");
            }

            return View("Index", await _orderQueries.GetClientCart(ClientId));
        }

        [HttpGet]
        [Route("resumo-da-compra")]
        public async Task<IActionResult> ResumoDaCompra()
        {
            return View(await _orderQueries.GetClientCart(ClientId));
        }
    }
}