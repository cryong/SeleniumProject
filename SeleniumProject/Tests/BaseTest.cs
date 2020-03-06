using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    public class BaseTest
    {
        public IWebDriver Driver { get; set; }
        private const string loginUrl = "http://horse-dev.azurewebsites.net/Account/Login?ReturnUrl=%2f";

        [OneTimeSetUp]
        public void SetUp()
        {
            ExcelLibHelpers.PopulateInCollection(@"..\..\..\Data\TestData.xlsx", "Login");
            // Open chrome
            Driver = new ChromeDriver();
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3); // implicit wait
            // Go to the URL
            Driver.Navigate().GoToUrl(loginUrl);

            // Maximise the browser
            Driver.Manage().Window.Maximize();

            LoginPage loginPage = new LoginPage();
            // Locate ID field and enter Hari
            // Locate Password field and enter 123123
            string userName = ExcelLibHelpers.ReadData(2, "Username");
            string passWord = ExcelLibHelpers.ReadData(2, "Password");
            HomePage homePage = loginPage.doLogin(Driver, ExcelLibHelpers.ReadData(2, "Username"), passWord);
            // Verify that the user has successfully logged into the web portal
            Assert.IsTrue(homePage.GetUserAccount(Driver).Text.Contains(userName), "Login Failed");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Driver.Quit();
        }

        public string CalculateCurrentTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        }

        public void ValidateURL(IWebDriver driver, string expectedUrl)
        {
            Assert.AreEqual(driver.Url, expectedUrl, "Currently on URL '" + driver.Url + "' but expected '" + expectedUrl + "'");
        }

        public int GenerateRandomNumber(int startRange, int endRange)
        {
            return new Random().Next(startRange, endRange);
        }
    }
}
