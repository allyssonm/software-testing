using System;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    public class UserLoginSteps
    {
        [When(@"He click on login")]
        public void WhenHeClickOnLogin()
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Fill in the login form data")]
        public void WhenFillInTheLoginFormData(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"Click the login button")]
        public void WhenClickTheLoginButton()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
