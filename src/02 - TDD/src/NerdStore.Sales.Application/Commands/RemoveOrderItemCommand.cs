using FluentValidation;
using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Commands
{
    public class RemoveOrderItemCommand : Command
    {
        public Guid ClientId { get; private set; }
        public Guid ProductId { get; private set; }

        public RemoveOrderItemCommand(Guid clientId, Guid productId)
        {
            ClientId = clientId;
            ProductId = productId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveOrderItemCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoveOrderItemCommandValidation : AbstractValidator<RemoveOrderItemCommand>
    {
        public RemoveOrderItemCommandValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do produto inválido");
        }
    }
}
