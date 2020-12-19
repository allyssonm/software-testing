using Bogus;
using Xunit;

namespace NerdStore.BDD.Tests.Config
{
    [CollectionDefinition(nameof(AutomationWebTestsFixture))]
    public class AutomationWebFixtureCollection : ICollectionFixture<AutomationWebTestsFixture> { }

    public class AutomationWebTestsFixture
    {
        public SeleniumHelper BrowserHelper;
        public readonly ConfigurationHelper Configuration;

        public User.User User;

        public AutomationWebTestsFixture()
        {
            User = new User.User();
            Configuration = new ConfigurationHelper();
            BrowserHelper = new SeleniumHelper(Browser.Chrome, Configuration, false);
        }

        public void GenerateUserData()
        {
            var faker = new Faker();
            User.Email = faker.Internet.Email().ToLower();
            User.Password = faker.Internet.Password(8, false, "", "@1Ab_");
        }
    }
}
