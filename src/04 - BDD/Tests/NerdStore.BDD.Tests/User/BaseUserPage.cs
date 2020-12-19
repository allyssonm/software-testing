using NerdStore.BDD.Tests.Config;

namespace NerdStore.BDD.Tests.User
{
    public abstract class BaseUserPage : PageObjectModel
    {
        protected BaseUserPage(SeleniumHelper helper) : base(helper) { }

        public void AccessStoreWebsite() =>
            Helper.GoToUrl(Helper.Configuration.DomainUrl);

        public bool ValidateLoggedUserGreeting(User user) =>
            Helper.GetTextElementById("greetingUser").Contains(user.Email);
    }
}
