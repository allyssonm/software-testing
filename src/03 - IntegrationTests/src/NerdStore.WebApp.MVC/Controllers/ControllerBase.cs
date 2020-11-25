using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NerdStore.Core.Messages.CommonMessages;

namespace NerdStore.WebApp.MVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediator _mediatorHandler;

        protected Guid ClientId;

        protected ControllerBase(INotificationHandler<DomainNotification> notifications,
                                 IMediator mediatorHandler, 
                                 IHttpContextAccessor httpContextAccessor)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;

            if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) return;

            var claim = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            ClientId = Guid.Parse(claim.Value);
        }

        protected bool IsValidOperation()
        {
            return !_notifications.HasNotification();
        }

        protected IEnumerable<string> GetErrorMessages()
        {
            return _notifications.GetNotifications().Select(c => c.Value).ToList();
        }

        protected void NotifyError(string codigo, string mensagem)
        {
            _mediatorHandler.Publish(new DomainNotification(codigo, mensagem));
        }

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
            {
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }

            return BadRequest(new
            {
                success = false,
                errors = _notifications.GetNotifications().Select(n => n.Value)
            });
        }
    }
}