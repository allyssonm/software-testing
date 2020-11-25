using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Sales.Application.Queries;

namespace NerdStore.WebApplication.MVC.Extensions
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IOrderQueries _pedidoQueries;

        // TODO: Obter cliente logado
        protected Guid ClienteId;


        public CartViewComponent(IOrderQueries pedidoQueries, IHttpContextAccessor httpContextAccessor)
        {
            _pedidoQueries = pedidoQueries;

            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) return;

            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            ClienteId = Guid.Parse(claim.Value);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var carrinho = await _pedidoQueries.GetClientCart(ClienteId);
            var itens = carrinho?.Items.Count ?? 0;

            return View(itens);
        }
    }
}