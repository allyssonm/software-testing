using Features.Clients;
using System;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClientCollection))]
    public class ClientCollection : ICollectionFixture<ClientTestsFixture>
    {

    }

    public class ClientTestsFixture : IDisposable
    {
        public Client CreateValidClient()
            => new Client(Guid.NewGuid(),
                    "Michael",
                    "Scott",
                    DateTime.UtcNow.AddYears(-42),
                    "michael@scott.com",
                    true,
                    DateTime.UtcNow);

        public Client CreateInvalidClient()
            => new Client(Guid.NewGuid(),
                "",
                "",
                DateTime.UtcNow,
                "michael2scott.com",
                true,
                DateTime.UtcNow);

        public void Dispose()
        {
        }
    }
}
