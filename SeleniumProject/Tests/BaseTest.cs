using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumProject.Pages;

namespace SeleniumProject.Tests
{
    class BaseTest
    {
        public IWebDriver driver { get; set; }
        private const string loginUrl = "http://horse-dev.azurewebsites.net/Account/Login?ReturnUrl=%2f";

        [OneTimeSetUp]
        public void SetUp()
        {
            // Open chrome
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            // Go to the URL
            driver.Navigate().GoToUrl(loginUrl);

            // Maximise the browser
            driver.Manage().Window.Maximize();

            LoginPage loginPage = new LoginPage();
            // Locate ID field and enter Hari
            // Locate Password field and enter 123123
            string userName = "hari";
            HomePage homePage = loginPage.doLogin(driver, userName, "123123");
            if (!homePage.GetUserAccount(driver).Text.Contains(userName))
            {
                Console.WriteLine("Login Failed");
                throw new WebDriverException("Login Failed");
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        public static string CalculateCurrentTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }

        public static void ValidateURL(IWebDriver driver, string expectedUrl)
        {
            if (driver.Url != expectedUrl)
            {
                Assert.Fail("Currently on URL '" + driver.Url + "' but expected '" + expectedUrl + "'");
            }
        }
    }
}
