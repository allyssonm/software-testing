using NerdStore.BDD.Tests.Config;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Order
{
    [Binding]
    [CollectionDefinition(nameof(AutomationWebTestsFixture))]
    public class Order_AddItemToOrderSteps
    {
        private readonly AutomationWebTestsFixture _testsFixture;
        private readonly OrderPage _orderPage;
        private string _productUrl;

        public Order_AddItemToOrderSteps(AutomationWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _orderPage = new OrderPage(_testsFixture.BrowserHelper);
        }

        [Given(@"a product is in showcase")]
        public void GivenAProductIsInShowcase()
        {
            // Arrange
            _orderPage.AccessProductsShowcase();

            // Act
            _orderPage.GetProductDetails();
            _productUrl = _orderPage.GetUrl();

            // Assert
            Assert.True(_orderPage.CheckIfProductIsAvailable());
        }
        
        [Given(@"it is available on stock")]
        public void GivenItIsAvailableOnStock()
        {
            // Arrange

            // Act

            // Assert
        }
        
        [Given(@"the user is already logged in")]
        public void GivenTheUserIsAlreadyLoggedIn()
        {
            // Arrange

            // Act

            // Assert
        }
        
        [When(@"the user adds an unit to order")]
        public void WhenTheUserAddsAnUnitToOrder()
        {
            // Arrange

            // Act

            // Assert
        }
        
        [Then(@"the user will be redirected to the purchase summary")]
        public void ThenTheUserWillBeRedirectedToThePurchaseSummary()
        {
            // Arrange

            // Act

            // Assert
        }
        
        [Then(@"the order's total price will be exactly the same as the added item")]
        public void ThenTheOrderSTotalPriceWillBeExactlyTheSameAsTheAddedItem()
        {
            // Arrange

            // Act

            // Assert
        }
    }
}
