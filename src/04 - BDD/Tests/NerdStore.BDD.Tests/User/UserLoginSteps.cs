using NerdStore.BDD.Tests.Config;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    [CollectionDefinition(nameof(AutomationWebTestsFixture))]
    public class UserLoginSteps
    {
        private readonly UserLoginPage _userLoginPage;
        private readonly AutomationWebTestsFixture _testsFixture;

        public UserLoginSteps(AutomationWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _userLoginPage = new UserLoginPage(_testsFixture.BrowserHelper);
        }

        [When(@"He click on login")]
        public void WhenHeClickOnLogin()
        {
            // Act
            _userLoginPage.ClickOnLoginLink();

            // Assert
            Assert.Contains(_testsFixture.Configuration.LoginUrl, _userLoginPage.GetUrl());
        }
        
        [When(@"Fill in the login form data")]
        public void WhenFillInTheLoginFormData(Table table)
        {
            // Arrange
            var user = new User()
            {
                Email = "test@teste.com",
                Password = "!Asdf1234"
            };
            _testsFixture.User = user;

            // Act
            _userLoginPage.FillLoginForm(user);

            // Assert
            Assert.True(_userLoginPage.IsLoginFormFilled(user));
        }
        
        [When(@"Click the login button")]
        public void WhenClickTheLoginButton()
        {
            _userLoginPage.ClickOnLoginButton();
        }
    }
}
