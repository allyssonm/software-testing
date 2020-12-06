using Features.Tests;
using NerdStore.WebApp.Tests.Config;
using NerdStore.WebApplication.MVC;
using NerdStore.WebApplication.MVC.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Feature.Tests.PriorityOrderer", "Features.Tests")]
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class OrderApiTests
    {
        private readonly IntegrationTestsFixture<StartupApiTests> _testsFixture;

        public OrderApiTests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Add item to new order"), TestPriority(1)]
        [Trait("Integration", "Api - Order")]
        public async Task AddItem_NewOrder_ShouldBeSuccessful()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("d760d46c-4f1e-4e61-ae95-2f7ecd8297c7"),
                Quantity = 2
            };

            await _testsFixture.PerformApiLogin();
            _testsFixture.Client.AddToken(_testsFixture.UserToken);

            // Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/cart", itemInfo);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Add item to new order"), TestPriority(2)]
        [Trait("Integration", "Api - Order")]
        public async Task RemoveItem_ExistentOrder_ShouldBeSuccessful()
        {
            // Arrange
            var productId = new Guid("d760d46c-4f1e-4e61-ae95-2f7ecd8297c7");
            await _testsFixture.PerformApiLogin();
            _testsFixture.Client.AddToken(_testsFixture.UserToken);

            // Act
            var deleteResponse = await _testsFixture.Client.DeleteAsync($"api/cart/{productId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}
