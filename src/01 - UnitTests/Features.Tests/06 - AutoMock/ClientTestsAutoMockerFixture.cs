using Bogus;
using Bogus.DataSets;
using Features.Clients;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Features.Tests
{ 
    [CollectionDefinition(nameof(ClientAutoMockerCollection))]
    public class ClientAutoMockerCollection : ICollectionFixture<ClientTestsAutoMockerFixture>
    {
    }

    public class ClientTestsAutoMockerFixture : IDisposable
    {
        public ClientService ClientService;
        public AutoMocker Mocker;

        public Client CreateValidClient() => GetClients(1, true).FirstOrDefault();

        public IEnumerable<Client> GetRandomClients()
        {
            var clients = new List<Client>();

            clients.AddRange(GetClients(30, true).ToList());
            clients.AddRange(GetClients(30, false).ToList());

            return clients;
        }

        public Client CreateInvalidClient()
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            return new Faker<Client>("pt_BR")
                .CustomInstantiator(f => new Client(
                        Guid.NewGuid(),
                        f.Name.FirstName(gender),
                        f.Name.LastName(gender),
                        f.Date.Past(1, DateTime.UtcNow.AddYears(1)),
                        "",
                        false,
                        DateTime.UtcNow))
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name.ToLower(), c.Surname.ToLower()));
        }

        private IEnumerable<Client> GetClients(int amount, bool active)
        {
            var gender = new Faker().PickRandom<Name.Gender>();

            var client = new Faker<Client>("pt_BR")
                .CustomInstantiator(f => new Client(
                        Guid.NewGuid(),
                        f.Name.FirstName(gender),
                        f.Name.LastName(gender),
                        f.Date.Past(80, DateTime.UtcNow.AddYears(-18)),
                        "",
                        active,
                        DateTime.UtcNow))
                .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.Name.ToLower(), c.Surname.ToLower()));

            return client.Generate(amount);
        }

        public ClientService GetClientService()
        {
            Mocker = new AutoMocker();
            ClientService = Mocker.CreateInstance<ClientService>();

            return ClientService;
        }

        public void Dispose()
        {
        }
    }
}
