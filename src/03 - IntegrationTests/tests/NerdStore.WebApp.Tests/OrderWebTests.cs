using AngleSharp.Html.Parser;
using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApplication.MVC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [Collection(nameof(IntegrationWebTestsFixtureCollection))]
    public class OrderWebTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> _testsFixture;

        public OrderWebTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Add item to new order")]
        [Trait("Integration", "Web - Order")]
        public async Task AddItem_NewOrder_ShouldUpdateOrderTotalPrice()
        {
            // Arrange
            var productId = new Guid("d760d46c-4f1e-4e61-ae95-2f7ecd8297c7");
            const int quantity = 2;

            var initialResponse = await _testsFixture.Client.GetAsync($"/product-detail/{productId}");
            initialResponse.EnsureSuccessStatusCode();

            var formData = new Dictionary<string, string>
            {
                { "Id", productId.ToString() },
                { "quantity", quantity.ToString() }
            };

            await _testsFixture.PerformWebLogin();

            var postRequest = new HttpRequestMessage(HttpMethod.Post, "/my-cart")
            {
                Content = new FormUrlEncodedContent(formData)
            };

            // Act
            var response = await _testsFixture.Client.SendAsync(postRequest);

            // Assert
            response.EnsureSuccessStatusCode();

            var html = new HtmlParser().ParseDocumentAsync(await response.Content.ReadAsStringAsync()).Result.All;

            var formQuantity = html?.FirstOrDefault(x => x.Id == "quantity")?.GetAttribute("value")?.OnlyNumbers();
            var unitValue = html?.FirstOrDefault(x => x.Id == "unitValue")?.TextContent.Split(".")[0]?.OnlyNumbers();
            var totalPrice = html?.FirstOrDefault(x => x.Id == "totalPrice")?.TextContent.Split(".")[0]?.OnlyNumbers();

            Assert.Equal(totalPrice, unitValue * formQuantity);
        }
    }
}
