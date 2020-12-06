using Features.Tests;
using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApplication.MVC;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Feature.Tests.PriorityOrderer", "Features.Tests")]
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class UserTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public UserTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "User register success"), TestPriority(1)]
        [Trait("Integration", "Web - User")]
        public async Task User_RegisterUser_ShouldExecuteWithSuccess()
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

        [Fact(DisplayName = "User register weak password"), TestPriority(3)]
        [Trait("Integration", "Web - User")]
        public async Task User_RegisterUserWeakPassword_ShouldReturnErrorMessage()
        {
            // Arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.GetAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            _testsFixture.GenerateUserAndPassword();
            var weakpass = "123456";

            var formData = new Dictionary<string, string>
            {
                { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
                {"Input.Email", _testsFixture.UserEmail },
                {"Input.Password", weakpass },
                {"Input.ConfirmPassword", weakpass},
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
            Assert.Contains($"Passwords must have at least one non alphanumeric character.", responseString);
            Assert.Contains($"Passwords must have at least one lowercase (&#x27;a&#x27;-&#x27;z&#x27;).", responseString);
            Assert.Contains($"Passwords must have at least one uppercase (&#x27;A&#x27;-&#x27;Z&#x27;).", responseString);
        }

        [Fact(DisplayName = "User login success"), TestPriority(2)]
        [Trait("Integration", "Web - User")]
        public async Task User_PerformLogin_ShouldExecuteWithSuccess()
        {
            // Arrange
            var initialResponse = await _testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = _testsFixture.GetAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var formData = new Dictionary<string, string>
            {
                { _testsFixture.AntiForgeryFieldName, antiForgeryToken },
                {"Input.Email", _testsFixture.UserEmail },
                {"Input.Password", _testsFixture.UserPassword },
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Identity/Account/Login")
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
