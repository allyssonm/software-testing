using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NerdStore.Catalog.Application.Services;
using NerdStore.Core.Messages.CommonMessages;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Application.Queries;
using NerdStore.WebApplication.MVC.Models;

namespace NerdStore.WebApplication.MVC.Controllers
{
    [Authorize]
    public class CartControllerApi : ControllerBase
    {
        private readonly IProductAppService _productAppService;
        private readonly IOrderQueries _orderQueries;
        private readonly IMediator _mediatorHandler;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public CartControllerApi(INotificationHandler<DomainNotification> notifications,
                                  IProductAppService productAppService,
                                  IMediator mediatorHandler, 
                                  IOrderQueries orderQueries,
                                  IHttpContextAccessor httpContextAccessor, 
                                  SignInManager<IdentityUser> signInManager, 
                                  UserManager<IdentityUser> userManager,
                                  IOptions<AppSettings> appSettings) : base(notifications, mediatorHandler, httpContextAccessor)
        {
            _productAppService = productAppService;
            _mediatorHandler = mediatorHandler;
            _orderQueries = orderQueries;
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        [Route("api/cart")]
        public async Task<IActionResult> Get()
        {
            return Response(await _orderQueries.GetClientCart(ClientId));
        }

        [HttpPost]
        [Route("api/cart")]
        public async Task<IActionResult> Post([FromBody] ItemViewModel item)
        {
            var product = await _productAppService.GetById(item.Id);
            if (product == null) return BadRequest();

            if (product.StockQuantity < item.Quantity)
            {
                NotifyError("ErroValidacao","Produto com estoque insuficiente");
            }

            var command = new AddOrderItemCommand(ClientId, product.Id, product.Name, item.Quantity, product.Price);
            await _mediatorHandler.Send(command);

            return Response();
        }

        [HttpPut]
        [Route("api/cart/{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ItemViewModel item)
        {
            var product = await _productAppService.GetById(id);
            if (product == null) return BadRequest();

            var command = new UpdateOrderItemCommand(ClientId, product.Id, item.Quantity);
            await _mediatorHandler.Send(command);

            return Response();
        }

        [HttpDelete]
        [Route("api/cart/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _productAppService.GetById(id);
            if (product == null) return BadRequest();

            var command = new RemoveOrderItemCommand(ClientId, id);
            await _mediatorHandler.Send(command);
            
            return Response();
        }

        [AllowAnonymous]
        [HttpPost("api/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(await GenerateJwt(login.Email));
            }

            NotifyError("login","Usuário ou Senha incorretos");
            return Response();
        }

        private async Task<string> GenerateJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Emissor,
                Audience = _appSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var tokenResult = tokenHandler.WriteToken(token);
            return tokenResult;
        }
    }
}