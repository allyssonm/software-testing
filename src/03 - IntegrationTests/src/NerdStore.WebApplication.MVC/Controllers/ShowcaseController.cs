using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalog.Application.Services;

namespace NerdStore.WebApplication.MVC.Controllers
{
    public class ShowcaseController : Controller
    {
        private readonly IProductAppService _produtoAppService;

        public ShowcaseController(IProductAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet]
        [Route("")]
        [Route("vitrine")]
        public async Task<IActionResult> Index()
        {
            return View(await _produtoAppService.GetAll());
        }

        [HttpGet]
        [Route("produto-detalhe/{id}")]
        public async Task<IActionResult> ProdutoDetalhe(Guid id)
        {
            return View(await _produtoAppService.GetById(id));
        }
    }
}