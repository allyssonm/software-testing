using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.User;
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
        private readonly UserLoginPage _userLoginPage;
        private string _productUrl;

        public Order_AddItemToOrderSteps(AutomationWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _orderPage = new OrderPage(_testsFixture.BrowserHelper);
            _userLoginPage = new UserLoginPage(_testsFixture.BrowserHelper);
        }

        [Given(@"the user is already logged in")]
        public void GivenTheUserIsAlreadyLoggedIn()
        {
            // Arrange
            var user = new User.User()
            {
                Email = "test@teste.com",
                Password = "!Asdf1234"
            };
            _testsFixture.User = user;

            // Act
            var login = _userLoginPage.Login(user);

            // Assert
            Assert.True(login);
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
            Assert.True(_orderPage.IsProductAvailable());
        }
        
        [Given(@"it is available on stock")]
        public void GivenItIsAvailableOnStock()
        {
            // Assert
            Assert.True(_orderPage.GetProductQuantity() > 0);
        }
        
        [When(@"the user adds an unit to order")]
        public void WhenTheUserAddsAnUnitToOrder()
        {
            // Act
            _orderPage.ClickOnPurchaseNow();
        }
        
        [Then(@"the user will be redirected to the purchase summary")]
        public void ThenTheUserWillBeRedirectedToThePurchaseSummary()
        {
            // Assert
            Assert.True(_orderPage.IsProductOnCart());
        }
        
        [Then(@"the order's total price will be exactly the same as the added item")]
        public void ThenTheOrderSTotalPriceWillBeExactlyTheSameAsTheAddedItem()
        {
            // Arrange
            var unitPrice = _orderPage.GetProductUnitPrice();
            var totalPrice = _orderPage.GetTotalPrice();

            // Assert
            Assert.Equal(unitPrice, totalPrice);
        }
    }
}
