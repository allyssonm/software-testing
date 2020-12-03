using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApplication.MVC;
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using Xunit;

namespace NerdStore.WebApp.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationWebTestsFixtureCollection))]
    public class IntegrationWebTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupWebTests>> { }

    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>> { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public string AntiForgeryFieldName = "__RequestVerificationToken";

        public string UserEmail;
        public string UserPassword;

        public readonly StoreAppFactory<TStartup> Factory;
        public readonly HttpClient Client;

        public IntegrationTestsFixture()
        {
            var options = new WebApplicationFactoryClientOptions
            {
            };

            Factory = new StoreAppFactory<TStartup>();
            Client = Factory.CreateClient(options);
        }

        public string GetAntiForgeryToken(string htmlBody)
        {
            var requestMatch = Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestMatch.Success)
            {
                return requestMatch.Groups[1].Captures[0].Value;
            }

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found in HTML", nameof(htmlBody));
        }

        public void GenerateUserAndPassword()
        {
            var faker = new Faker();
            UserEmail = faker.Internet.Email().ToLower();
            UserPassword = faker.Internet.Password(8, false, "", "@1Aa_");
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
