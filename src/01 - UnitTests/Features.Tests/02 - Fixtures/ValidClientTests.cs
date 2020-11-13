using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClientCollection))]
    public class ValidClientTests
    {
        readonly ClientTestsFixture _clientTestsFixture;

        public ValidClientTests(ClientTestsFixture clientTestsFixture)
        {
            _clientTestsFixture = clientTestsFixture;
        }

        [Fact(DisplayName = "New Valid Client Fixture")]
        [Trait("UnitTests", "Client Fixture Tests")]
        public void Client_NewClient_ShouldBeValid()
        {
            // Arrage
            var client = _clientTestsFixture.CreateValidClient();

            // Act
            var result = client.IsValid();

            // Assert
            Assert.True(result);
            Assert.Equal(0, client.ValidationResult.Errors.Count);
        }
    }
}
