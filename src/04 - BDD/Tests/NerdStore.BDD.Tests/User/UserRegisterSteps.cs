using System;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    public class UserRegisterSteps
    {
        [When(@"He click on register")]
        public void WhenHeClickOnRegister()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Fill in the form data")]
        public void WhenFillInTheFormData(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Click the register button")]
        public void WhenClickTheRegisterButton()
        {
            ScenarioContext.Current.Pending();
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
