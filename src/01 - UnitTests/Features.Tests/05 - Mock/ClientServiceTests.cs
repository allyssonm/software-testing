using Features.Clients;
using MediatR;
using Moq;
using System.Linq;
using System.Threading;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClientBogusCollection))]

    public class ClientServiceTests
    {
        readonly ClientTestsBogusFixture _clientTestsBogusFixture;

        public ClientServiceTests(ClientTestsBogusFixture clientTestsBogusFixture)
        {
            _clientTestsBogusFixture = clientTestsBogusFixture;
        }

        [Fact(DisplayName = "Success on Adding Client")]
        [Trait("UnitTests", "Client Service Mock Tests")]
        public void ClientService_Add_ShouldReturnsSuccess()
        {
            // Arrange
            var client = _clientTestsBogusFixture.CreateValidClient();
            var clientRepo = new Mock<IClientRepository>();
            var mediatr = new Mock<IMediator>();

            var clientService = new ClientService(clientRepo.Object, mediatr.Object);

            // Act
            clientService.Add(client);

            // Assert
            Assert.True(client.IsValid());
            clientRepo.Verify(r => r.Add(client), Times.Once);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Fail on Adding Client")]
        [Trait("UnitTests", "Client Service Mock Tests")]
        public void ClientService_Add_ShouldReturnsFailDueToInvalidClient()
        {
            // Arrange
            var client = _clientTestsBogusFixture.CreateInvalidClient();
            var clientRepo = new Mock<IClientRepository>();
            var mediatr = new Mock<IMediator>();

            var clientService = new ClientService(clientRepo.Object, mediatr.Object);

            // Act
            clientService.Add(client);

            // Assert
            Assert.False(client.IsValid());
            clientRepo.Verify(r => r.Add(client), Times.Never);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get All Active Clients")]
        [Trait("UnitTests", "Client Service Mock Tests")]
        public void ClientService_GetAllActive_ShouldReturnsOnlyActiveClients()
        {
            // Arrange
            var clientRepo = new Mock<IClientRepository>();
            var mediatr = new Mock<IMediator>();

            clientRepo.Setup(c => c.GetAll())
                .Returns(_clientTestsBogusFixture.GetRandomClients());

            var clientService = new ClientService(clientRepo.Object, mediatr.Object);

            // Act
            var clients = clientService.GetAllActive();

            // Assert 
            clientRepo.Verify(r => r.GetAll(), Times.Once);
            Assert.True(clients.Any());
            Assert.False(clients.Count(c => !c.Active) > 0);
        }
    }
}
