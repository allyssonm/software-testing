using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    [CollectionDefinition(nameof(AutomationWebTestsFixture))]
    public class CommonSteps
    {
        private readonly UserRegisterPage _registerUserPage;
        private readonly AutomationWebTestsFixture _testsFixture;

        public CommonSteps(AutomationWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _registerUserPage = new UserRegisterPage(_testsFixture.BrowserHelper);
        }

        [Given(@"That the visitor is accessing the store website")]
        public void GivenThatTheVisitorIsAccessingTheStoreWebsite()
        {
            // Act
            _registerUserPage.AccessStoreWebsite();

            // Assert
            Assert.Contains(_testsFixture.Configuration.DomainUrl, _registerUserPage.GetUrl());
        }

        [Then(@"He will be redirected to the showcase")]
        public void ThenHeWillBeRedirectedToTheShowcase()
        {
            // Assert
            Assert.Equal(_testsFixture.Configuration.ShowcaseUrl, _registerUserPage.GetUrl());
        }

        [Then(@"A greeting with your email will be displayed in the top menu")]
        public void ThenAGreetingWithYourEmailWillBeDisplayedInTheTopMenu()
        {
            // Assert
            Assert.True(_registerUserPage.ValidateLoggedUserGreeting(_testsFixture.User));
        }
    }
}
