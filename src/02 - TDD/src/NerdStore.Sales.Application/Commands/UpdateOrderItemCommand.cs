using FluentValidation;
using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Commands
{
    public class UpdateOrderItemCommand : Command
    {
        public Guid ClientId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public UpdateOrderItemCommand(Guid clientId, Guid productId, int quantity)
        {
            ClientId = clientId;
            ProductId = productId;
            Quantity = quantity;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateOrderItemCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateOrderItemCommandValidation : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemCommandValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do produto inválido");

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage("A quantidade miníma de um item é 1");

            RuleFor(c => c.Quantity)
                .LessThan(15)
                .WithMessage("A quantidade máxima de um item é 15");
        }
    }
}
