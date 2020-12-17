using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    public class CommonSteps
    {
        [Given(@"That the visitor is accessing the store website")]
        public void GivenThatTheVisitorIsAccessingTheStoreWebsite()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"He will be redirected to the showcase")]
        public void ThenHeWillBeRedirectedToTheShowcase()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"A greeting with your email will be displayed in the top menu")]
        public void ThenAGreetingWithYourEmailWillBeDisplayedInTheTopMenu()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
