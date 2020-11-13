using Bogus;
using Bogus.DataSets;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClientBogusCollection))]
    public class ClientBogusTests
    {
        readonly ClientTestsBogusFixture _clientTestsBogusFixture;

        public ClientBogusTests(ClientTestsBogusFixture clientTestsBogusFixture)
        {
            _clientTestsBogusFixture = clientTestsBogusFixture;
        }

        [Fact(DisplayName = "New Valid Client")]
        [Trait("UnitTests", "Client Bogus Tests")]
        public void Client_NewClient_ShouldBeValid()
        {
            // Arrage
            var client = _clientTestsBogusFixture.CreateValidClient();

            // Act
            var result = client.IsValid();

            // Assert
            Assert.True(result);
            Assert.Equal(0, client.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "New Invalid Client")]
        [Trait("UnitTests", "Client Bogus Tests")]
        public void Client_NewClient_ShouldNotBeValid()
        {
            // Arrage
            var gender = new Faker().PickRandom<Name.Gender>();
            var client = _clientTestsBogusFixture.CreateInvalidClient();

            // Act
            var result = client.IsValid();

            // Assert
            Assert.False(result);
            Assert.NotEqual(0, client.ValidationResult.Errors.Count);
        }
    }
}
