using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.BDD.Tests.User
{
    public class RegisterUserPage : BaseUserPage
    {
        public RegisterUserPage(SeleniumHelper helper) : base(helper)
        {
        }

        public void ClickOnRegisterLink() =>
            Helper.ClickOnLinkByText("Register");

        public void FillRegisterForm(User user)
        {
            Helper.FillTextBoxById("Input_Email", user.Email);
            Helper.FillTextBoxById("Input_Password", user.Password);
            Helper.FillTextBoxById("Input_ConfirmPassword", user.Password);
        }

        public bool IsRegisterFormFilled(User user)
        {
            if (Helper.GetTextBoxValueById("Input_Email") != user.Email) return false;
            if (Helper.GetTextBoxValueById("Input_Password") != user.Password) return false;
            if (Helper.GetTextBoxValueById("Input_ConfirmPassword") != user.Password) return false;

            return true;
        }

        public void ClickOnRegisterButton() =>
            Helper.GetElementByXPath("/html/body/div/main/div/div[1]/form/button").Click();
    }
}
