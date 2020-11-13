using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClientCollection))]
    public class InvalidClientTests
    {
        readonly ClientTestsFixture _clientTestsFixture;

        public InvalidClientTests(ClientTestsFixture clientTestsFixture)
        {
            _clientTestsFixture = clientTestsFixture;
        }

        [Fact(DisplayName = "New Invalid Client Fixture")]
        [Trait("UnitTests", "Client Fixture Tests")]
        public void Client_NewClient_ShouldNotBeValid()
        {
            // Arrage
            var client = _clientTestsFixture.CreateInvalidClient();

            // Act
            var result = client.IsValid();

            // Assert
            Assert.False(result);
            Assert.NotEqual(0, client.ValidationResult.Errors.Count);
        }
    }
}
