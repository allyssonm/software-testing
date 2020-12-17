using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace NerdStore.BDD.Tests.Config
{
    public class SeleniumHelper : IDisposable
    {
        public IWebDriver WebDriver;
        public readonly ConfigurationHelper Configuration;
        public WebDriverWait Wait;

        public SeleniumHelper(Browser browser, ConfigurationHelper configuration, bool headless = true)
        {
            Configuration = configuration;
            WebDriver = WebDriverFactory.CreateWebDriver(browser, Configuration.WebDrivers, headless);
            WebDriver.Manage().Window.Maximize();
            Wait = new WebDriverWait(WebDriver, TimeSpan.FromSeconds(30));
        }

        public string GetUrl() => WebDriver.Url;

        public void GoToUrl(string url) => WebDriver.Navigate().GoToUrl(url);

        public void ClickOnLinkByText(string linkText) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.LinkText(linkText)))
                .Click();

        public bool ValidateUrlContent(string content) =>
            Wait.Until(ExpectedConditions.UrlContains(content));

        public void ClickOnButtonById(string buttonId) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(buttonId))).Click();

        public void ClickByXPath(string xPath) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath))).Click();

        public IWebElement GetElementByClass(string cssClass) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(cssClass)));

        public IWebElement GetElementByXPath(string xPath) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));

        public void FillTextBoxById(string fieldId, string fieldValue) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(fieldId))).SendKeys(fieldValue);

        public string GetTextElementByClass(string className) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.ClassName(className))).Text;

        public string GetTextElementById(string id) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id))).Text;

        public string GetTextBoxValueById(string id) =>
            Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(id)))
                .GetAttribute("value");

        public IEnumerable<IWebElement> GetListByClass(string className) =>
            Wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName(className)));

        public bool CheckIfElementExistsById(string id) => ElementExists(By.Id(id));

        public void TakeScreenshot(string name) =>
            SaveScreenShot(WebDriver.TakeScreenshot(), $"{DateTime.UtcNow.ToFileTime()}_{name}.png");

        private void SaveScreenShot(Screenshot screenshot, string fileName) =>
            screenshot.SaveAsFile($"{Configuration.FolderPicture}{fileName}", ScreenshotImageFormat.Png);

        public void FillDropDownById(string fieldId, string fieldValue)
        {
            var field = Wait.Until(ExpectedConditions.ElementIsVisible(By.Id(fieldId)));
            var selectElement = new SelectElement(field);
            selectElement.SelectByValue(fieldValue);
        }

        public void NavigateBack(int times = 1)
        {
            for (var i = 0; i < times; i++)
            {
                WebDriver.Navigate().Back();
            }
        }

        private bool ElementExists(By by)
        {
            try
            {
                WebDriver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void Dispose()
        {
            WebDriver.Quit();
            WebDriver.Dispose();
        }
    }
}
