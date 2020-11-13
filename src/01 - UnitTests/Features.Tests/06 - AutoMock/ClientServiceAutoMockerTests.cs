using Features.Clients;
using MediatR;
using Moq;
using Moq.AutoMock;
using System.Linq;
using System.Threading;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClientBogusCollection))]

    public class ClientServiceAutoMockerTests
    {
        readonly ClientTestsBogusFixture _clientTestsBogusFixture;

        public ClientServiceAutoMockerTests(ClientTestsBogusFixture clientTestsBogusFixture)
        {
            _clientTestsBogusFixture = clientTestsBogusFixture;
        }

        [Fact(DisplayName = "Success on Adding Client")]
        [Trait("UnitTests", "Client Service AutoMock Tests")]
        public void ClientService_Add_ShouldReturnsSuccess()
        {
            // Arrange
            var client = _clientTestsBogusFixture.CreateValidClient();
            var mocker = new AutoMocker();
            var clientService = mocker.CreateInstance<ClientService>();

            // Act
            clientService.Add(client);

            // Assert
            Assert.True(client.IsValid());
            mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Once);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Fail on Adding Client")]
        [Trait("UnitTests", "Client Service AutoMock Tests")]
        public void ClientService_Add_ShouldReturnsFailDueToInvalidClient()
        {
            // Arrange
            var client = _clientTestsBogusFixture.CreateInvalidClient();
            var mocker = new AutoMocker();
            var clientService = mocker.CreateInstance<ClientService>();

            // Act
            clientService.Add(client);

            // Assert
            Assert.False(client.IsValid());
            mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Never);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get All Active Clients")]
        [Trait("UnitTests", "Client Service AutoMock Tests")]
        public void ClientService_GetAllActive_ShouldReturnsOnlyActiveClients()
        {
            // Arrange
            var mocker = new AutoMocker();
            var clientService = mocker.CreateInstance<ClientService>();

            mocker.GetMock<IClientRepository>().Setup(c => c.GetAll())
                .Returns(_clientTestsBogusFixture.GetRandomClients());

            // Act
            var clients = clientService.GetAllActive();

            // Assert 
            mocker.GetMock<IClientRepository>().Verify(r => r.GetAll(), Times.Once);
            Assert.True(clients.Any());
            Assert.False(clients.Count(c => !c.Active) > 0);
        }
    }
}
