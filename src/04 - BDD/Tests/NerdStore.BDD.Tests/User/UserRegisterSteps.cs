using NerdStore.BDD.Tests.Config;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    [CollectionDefinition(nameof(AutomationWebTestsFixture))]
    public class UserRegisterSteps
    {
        private readonly UserRegisterPage _registerUserPage;
        private readonly AutomationWebTestsFixture _testsFixture;

        public UserRegisterSteps(AutomationWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _registerUserPage = new UserRegisterPage(_testsFixture.BrowserHelper);
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
            // Arrage
            _testsFixture.GenerateUserData();
            var user = _testsFixture.User;
            user.Password = "asdf_1234";

            // Act
            _registerUserPage.FillRegisterForm(user);

            // Assert
            Assert.True(_registerUserPage.IsRegisterFormFilled(user));
        }
        
        [When(@"Fill in the form data with a password without a special character")]
        public void WhenFillInTheFormDataWithAPasswordWithoutASpecialCharacter(Table table)
        {
            // Arrage
            _testsFixture.GenerateUserData();
            var user = _testsFixture.User;
            user.Password = "Asdf1234";

            // Act
            _registerUserPage.FillRegisterForm(user);

            // Assert
            Assert.True(_registerUserPage.IsRegisterFormFilled(user));
        }
        
        [Then(@"He will receive an error message that the password must contain a capital letter")]
        public void ThenHeWillReceiveAnErrorMessageThatThePasswordMustContainACapitalLetter()
        {
            // Assert
            Assert.True(_registerUserPage.ValidateErrorMessageOnFormSubmit("Passwords must have at least one uppercase ('A'-'Z')"));
        }
        
        [Then(@"He will receive an error message that the password must contain a special character")]
        public void ThenHeWillReceiveAnErrorMessageThatThePasswordMustContainASpecialCharacter()
        {
            // Assert
            Assert.True(_registerUserPage.ValidateErrorMessageOnFormSubmit("Passwords must have at least one non alphanumeric character"));
        }
    }
}
