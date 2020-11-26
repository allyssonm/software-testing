using FluentValidation;
using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;

namespace NerdStore.Sales.Domain
{
    public class Voucher : Entity
    {
        public Voucher()
        {
        }

        public Voucher(string code, decimal? discountPercent, decimal? discountValue, int quantity,
            VoucherDiscountType voucherDiscountType, DateTime expirationDate, bool active, bool used)
        {
            Code = code;
            DiscountValue = discountValue;
            DiscountPercent = discountPercent;
            VoucherDiscountType = voucherDiscountType;
            Quantity = quantity;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
        }

        public string Code { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public decimal? DiscountPercent { get; private set; }
        public VoucherDiscountType VoucherDiscountType { get; private set; }
        public int Quantity { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        public ICollection<Order> Orders { get; set; }

        public ValidationResult ValidateApplicability()
        {
            return new VoucherApplicabilityValidation().Validate(this);
        }
    }

    public class VoucherApplicabilityValidation : AbstractValidator<Voucher>
    {
        public static string CodeErrorMessage => "Voucher sem código válido.";
        public static string ExpirationDateErrorMessage => "Este voucher está expirado.";
        public static string ActiveErrorMessage => "Este voucher não é mais válido.";
        public static string UsedErrorMessage => "Este voucher já foi utilizado.";
        public static string QuantityErrorMessage => "Este voucher não está mais disponível";
        public static string DiscountValueErrorMessage => "O valor do desconto precisa ser superior a 0";
        public static string DiscountPercentErrorMessage => "O valor da porcentagem de desconto precisa ser superior a 0";

        public VoucherApplicabilityValidation()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(CodeErrorMessage);

            RuleFor(c => c.ExpirationDate)
                .Must(IsExpirationDateGreatherThanCurrent)
                .WithMessage(ExpirationDateErrorMessage);

            RuleFor(c => c.Active)
                .Equal(true)
                .WithMessage(ActiveErrorMessage);

            RuleFor(c => c.Used)
                .Equal(false)
                .WithMessage(UsedErrorMessage);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityErrorMessage);

            When(f => f.VoucherDiscountType == VoucherDiscountType.Value, () =>
            {
                RuleFor(f => f.DiscountValue)
                    .NotNull()
                    .WithMessage(DiscountValueErrorMessage)
                    .GreaterThan(0)
                    .WithMessage(DiscountValueErrorMessage);
            });

            When(f => f.VoucherDiscountType == VoucherDiscountType.Percent, () =>
            {
                RuleFor(f => f.DiscountPercent)
                    .NotNull()
                    .WithMessage(DiscountPercentErrorMessage)
                    .GreaterThan(0)
                    .WithMessage(DiscountPercentErrorMessage);
            });
        }

        protected static bool IsExpirationDateGreatherThanCurrent(DateTime expirationDate)
        {
            return expirationDate >= DateTime.Now;
        }
    }
}
