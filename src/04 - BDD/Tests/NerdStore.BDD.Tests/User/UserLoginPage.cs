using NerdStore.BDD.Tests.Config;

namespace NerdStore.BDD.Tests.User
{
    public class UserLoginPage : BaseUserPage
    {
        public UserLoginPage(SeleniumHelper helper) : base(helper)
        {
        }

        public void ClickOnLoginLink() =>
            Helper.ClickOnLinkByText("Login");

        public void FillLoginForm(User user)
        {
            Helper.FillTextBoxById("Input_Email", user.Email);
            Helper.FillTextBoxById("Input_Password", user.Password);
        }

        public bool IsLoginFormFilled(User user)
        {
            if (Helper.GetTextBoxValueById("Input_Email") != user.Email) return false;
            if (Helper.GetTextBoxValueById("Input_Password") != user.Password) return false;

            return true;
        }

        public void ClickOnLoginButton() =>
            Helper.GetElementByXPath("/html/body/div/main/div/div[1]/section/form/div[5]/button").Click();

        public bool Login(User user)
        {
            AccessStoreWebsite();
            ClickOnLoginLink();
            FillLoginForm(user);
            if (!IsLoginFormFilled(user)) return false;
            ClickOnLoginButton();
            if (!ValidateLoggedUserGreeting(user)) return false;

            return true;
        }
    }
}
