using System;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.Order
{
    [Binding]
    public class Order_AddItemToOrderSteps
    {
        [Given(@"a product is in showcase")]
        public void GivenAProductIsInShowcase()
        {
            // Arrange

            // Act

            // Assert
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
