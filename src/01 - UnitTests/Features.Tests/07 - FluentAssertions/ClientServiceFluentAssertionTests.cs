using Features.Clients;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;
using System.Threading;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClientAutoMockerCollection))]
    public class ClienteServiceFluentAssertionTests
    {
        readonly ClientTestsAutoMockerFixture _clientTestsAutoMockerFixture;
        private readonly ClientService _clientService;

        public ClienteServiceFluentAssertionTests(ClientTestsAutoMockerFixture clienteTestsFixture)
        {
            _clientTestsAutoMockerFixture = clienteTestsFixture;
            _clientService = _clientTestsAutoMockerFixture.GetClientService();
        }

        [Fact(DisplayName = "Success on Adding Client")]
        [Trait("UnitTests", "Client Service Fluent Assertions Tests")]
        public void ClientService_Add_ShouldReturnsSuccess()
        {
            // Arrange
            var client = _clientTestsAutoMockerFixture.CreateValidClient();

            // Act
            _clientService.Add(client);

            // Assert
            client.IsValid().Should().BeTrue();
            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Once);
            _clientTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Fail on Adding Client")]
        [Trait("UnitTests", "Client Service Fluent Assertions Tests")]
        public void ClientService_Add_ShouldReturnsFailDueToInvalidClient()
        {
            // Arrange
            var client = _clientTestsAutoMockerFixture.CreateInvalidClient();

            // Act
            _clientService.Add(client);

            // Assert
            client.IsValid().Should().BeFalse("Has validation errors.");
            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.Add(client), Times.Never);
            _clientTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get All Active Clients")]
        [Trait("UnitTests", "Client Service Fluent Assertions Tests")]
        public void ClientService_GetAllActive_ShouldReturnsOnlyActiveClients()
        {
            // Arrange
            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>().Setup(c => c.GetAll())
                .Returns(_clientTestsAutoMockerFixture.GetRandomClients());

            // Act
            var clients = _clientService.GetAllActive();

            // Assert 
            clients.Should().HaveCountGreaterOrEqualTo(1).And.OnlyHaveUniqueItems();
            clients.Should().NotContain(c => !c.Active);

            _clientTestsAutoMockerFixture.Mocker.GetMock<IClientRepository>().Verify(r => r.GetAll(), Times.Once);

            _clientService.ExecutionTimeOf(c => c.GetAllActive())
                .Should()
                .BeLessOrEqualTo(50.Milliseconds(), "Is executed more than a thousand times per second.");
        }
    }
}
