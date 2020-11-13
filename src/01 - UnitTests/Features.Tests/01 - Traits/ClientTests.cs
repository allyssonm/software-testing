using Features.Clients;
using System;
using Xunit;

namespace Features.Tests
{
    public class ClientTests
    {
        [Fact(DisplayName = "New Valid Client")]
        [Trait("UnitTests", "Client Trait Tests")]
        public void Client_NewClient_ShouldBeValid()
        {
            // Arrage
            var client = new Client(
                Guid.NewGuid(),
                "Michael",
                "Scott",
                DateTime.UtcNow.AddYears(-42),
                "michael@scott.com",
                true,
                DateTime.UtcNow
            );

            // Act
            var result = client.IsValid();

            // Assert
            Assert.True(result);
            Assert.Equal(0, client.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "New Invalid Client")]
        [Trait("UnitTests", "Client Trait Tests")]
        public void Client_NewClient_ShouldNotBeValid()
        {
            // Arrage
            var client = new Client(
                Guid.NewGuid(),
                "",
                "",
                DateTime.UtcNow,
                "michael2scott.com",
                true,
                DateTime.UtcNow
            );

            // Act
            var result = client.IsValid();

            // Assert
            Assert.False(result);
            Assert.NotEqual(0, client.ValidationResult.Errors.Count);
        }
    }
}
