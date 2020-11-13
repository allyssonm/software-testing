using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Features.Tests
{
    [Collection(nameof(ClientAutoMockerCollection))]
    public class ClientFluentAssertionsTests
    {
        private readonly ClientTestsAutoMockerFixture _clienteTestsFixture;
        readonly ITestOutputHelper _outputHelper;

        public ClientFluentAssertionsTests(ClientTestsAutoMockerFixture clienteTestsFixture,
                                            ITestOutputHelper outputHelper)
        {
            _clienteTestsFixture = clienteTestsFixture;
            _outputHelper = outputHelper;
        }


        [Fact(DisplayName = "New Valid Client")]
        [Trait("UnitTests", "Client Fluent Assert Tests")]
        public void Client_NewClient_ShouldBeValid()
        {
            // Arrange
            var cliente = _clienteTestsFixture.CreateValidClient();

            // Act
            var result = cliente.IsValid();

            // Assert 
            result.Should().BeTrue();
            cliente.ValidationResult.Errors.Should().HaveCount(0);
        }


        [Fact(DisplayName = "New Invalid Client")]
        [Trait("UnitTests", "Client Fluent Assert Tests")]
        public void Client_NewClient_ShouldNotBeValid()
        {
            // Arrange
            var cliente = _clienteTestsFixture.CreateInvalidClient();

            // Act
            var result = cliente.IsValid();

            // Assert 
            result.Should().BeFalse();
            cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1, "should have validation erros");

            _outputHelper.WriteLine($"{cliente.ValidationResult.Errors.Count} were found in this validation");
        }
    }
}
