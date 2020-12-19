using NerdStore.BDD.Tests.Config;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    [CollectionDefinition(nameof(AutomationWebTestsFixture))]
    public class UserRegisterSteps
    {
        private readonly RegisterUserPage _registerUserPage;
        private readonly AutomationWebTestsFixture _testsFixture;

        public UserRegisterSteps(AutomationWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _registerUserPage = new RegisterUserPage(_testsFixture.BrowserHelper);
        }

        [When(@"He click on register")]
        public void WhenHeClickOnRegister()
        {
            // Act
            _registerUserPage.ClickOnRegisterLink();

            // Assert
            Assert.Contains(_testsFixture.Configuration.RegisterUrl, _registerUserPage.GetUrl());
        }
        
        [When(@"Fill in the form data")]
        public void WhenFillInTheFormData(Table table)
        {
            // Arrage
            _testsFixture.GenerateUserData();
            var user = _testsFixture.User;

            // Act
            _registerUserPage.FillRegisterForm(user);

            // Assert
            Assert.True(_registerUserPage.IsRegisterFormFilled(user));
        }
        
        [When(@"Click the register button")]
        public void WhenClickTheRegisterButton()
        {
            _registerUserPage.ClickOnRegisterButton();
        }
        
        [When(@"Fill in the form data with a password without capital letters")]
        public void WhenFillInTheFormDataWithAPasswordWithoutCapitalLetters(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Fill in the form data with a password without a special character")]
        public void WhenFillInTheFormDataWithAPasswordWithoutASpecialCharacter(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"He will receive an error message that the password must contain a capital letter")]
        public void ThenHeWillReceiveAnErrorMessageThatThePasswordMustContainACapitalLetter()
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"He will receive an error message that the password must contain a special character")]
        public void ThenHeWillReceiveAnErrorMessageThatThePasswordMustContainASpecialCharacter()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
