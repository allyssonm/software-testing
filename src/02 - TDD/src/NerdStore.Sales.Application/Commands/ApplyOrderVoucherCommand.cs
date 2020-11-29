using FluentValidation;
using NerdStore.Core.Messages;
using System;

namespace NerdStore.Sales.Application.Commands
{
    public class ApplyOrderVoucherCommand : Command
    {
        public Guid ClientId { get; private set; }
        public string VoucherCode { get; private set; }

        public ApplyOrderVoucherCommand(Guid clientId, string voucherCode)
        {
            ClientId = clientId;
            VoucherCode = voucherCode;
        }

        public override bool IsValid()
        {
            ValidationResult = new ApplyOrderVoucherCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ApplyOrderVoucherCommandValidation : AbstractValidator<ApplyOrderVoucherCommand>
    {
        public ApplyOrderVoucherCommandValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do cliente inválido");

            RuleFor(c => c.VoucherCode)
                .NotEmpty()
                .WithMessage("O código do voucher não pode ser vazio");
        }
    }
}
