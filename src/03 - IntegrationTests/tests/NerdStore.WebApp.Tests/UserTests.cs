using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApplication.MVC;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UserTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public UserTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "User register sucess")]
        [Trait("Integration", "Web - User")]
        public async Task User_RegisterUser_ShouldExecuteWithSucess()
        {
            // Arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.GetAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            _testsFixture.GenerateUserAndPassword();

            var formData = new Dictionary<string, string>
            {
                { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
                {"Input.Email", _testsFixture.UserEmail },
                {"Input.Password", _testsFixture.UserPassword },
                {"Input.ConfirmPassword", _testsFixture.UserPassword },
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Register")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var response = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            var responseString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            Assert.Contains($"Hello {_testsFixture.UserEmail}!", responseString);
        }
    }
}
