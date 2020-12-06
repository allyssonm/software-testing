using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApplication.MVC;
using NerdStore.WebApplication.MVC.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public string UserToken;

        public readonly StoreAppFactory<TStartup> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                HandleCookies = true,
                MaxAutomaticRedirections = 7,
                BaseAddress = new Uri("http://localhost")
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

        public async Task PerformWebLogin()
        {
            var initialResponse = await Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = GetAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var formData = new Dictionary<string, string>
            {
                { AntiForgeryFieldName, antiForgeryToken },
                {"Input.Email", "test@teste.com" },
                {"Input.Password", "!Asdf1234" },
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            var response = await Client.SendAsync(postRequest);
        }

        public async Task PerformApiLogin()
        {
            var vm = new LoginViewModel()
            {
                Email = "test@teste.com",
                Password = "!Asdf1234"
            };

            Client = Factory.CreateClient();

            var response = await Client.PostAsJsonAsync("api/login", vm);
            response.EnsureSuccessStatusCode();
            UserToken = await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
