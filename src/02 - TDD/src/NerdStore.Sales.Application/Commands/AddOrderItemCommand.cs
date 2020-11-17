using FluentValidation;
using NerdStore.Core.Messages;
using NerdStore.Sales.Domain;
using System;

namespace NerdStore.Sales.Application.Commands
{
    public class AddOrderItemCommand : Command
    {
        public AddOrderItemCommand(Guid clientId, Guid productId, string name, int quantity, decimal unitValue)
        {
            ClientId = clientId;
            ProductId = productId;
            Name = name;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public Guid ClientId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddOrderItemCommandValidation : AbstractValidator<AddOrderItemCommand>
    {
        public static string ClientIdErrorMessage => "Id do cliente inválido";
        public static string ProductIdErrorMessage => "Id do produto inválido";
        public static string NameErrorMessage => "O nome do produto não foi informado";
        public static string QtyMaxErrorMessage => $"A quantidade máxima de um item é {Order.MAX_ORDER_ITEMS}";
        public static string QtyMinErrorMessage => "A quantidade miníma de um item é 1";
        public static string ValueErrorMessage => "O valor do item precisa ser maior que 0";

        public AddOrderItemCommandValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(ClientIdErrorMessage);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMessage);

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(NameErrorMessage);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QtyMinErrorMessage)
                .LessThanOrEqualTo(Order.MAX_ORDER_ITEMS)
                .WithMessage(QtyMaxErrorMessage);

            RuleFor(c => c.UnitValue)
                .GreaterThan(0)
                .WithMessage(ValueErrorMessage);
        }
    }
}
