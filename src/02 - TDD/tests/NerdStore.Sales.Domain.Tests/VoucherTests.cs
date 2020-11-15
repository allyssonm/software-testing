using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace NerdStore.Sales.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Voucher Type Value Valid")]
        [Trait("TDD", "Sales - Voucher")]
        public void Voucher_ValidateVoucherTypeValue_ShouldBeValid()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-CAD", null, 15, 1, VoucherDiscountType.Value, DateTime.UtcNow.AddDays(15), true, false);

            // Act
            var result = voucher.ValidateApplicability();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Type Value Invalid")]
        [Trait("TDD", "Sales - Voucher")]
        public void Voucher_ValidateVoucherTypeValue_ShouldBeInvalid()
        {
            // Arrange
            var voucher = new Voucher("", null, null, 0, VoucherDiscountType.Value, DateTime.UtcNow.AddDays(-1), false, true);

            // Act
            var result = voucher.ValidateApplicability();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherApplicabilityValidation.ActiveErrorMessage, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicabilityValidation.CodeErrorMessage, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicabilityValidation.ExpirationDateErrorMessage, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicabilityValidation.QuantityErrorMessage, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicabilityValidation.UsedErrorMessage, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherApplicabilityValidation.DiscountValueErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
