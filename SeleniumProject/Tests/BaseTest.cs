using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    class BaseTest
    {
        public IWebDriver Driver { get; set; }
        private const string loginUrl = "http://horse-dev.azurewebsites.net/Account/Login?ReturnUrl=%2f";

        [OneTimeSetUp]
        public void SetUp()
        {
            ExcelLibHelpers.PopulateInCollection(@"..\..\..\Data\TestData.xlsx", "Login");
            // Open chrome
            Driver = new ChromeDriver();
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            // Go to the URL
            Driver.Navigate().GoToUrl(loginUrl);

            // Maximise the browser
            Driver.Manage().Window.Maximize();

            LoginPage loginPage = new LoginPage();
            // Locate ID field and enter Hari
            // Locate Password field and enter 123123
            string userName = ExcelLibHelpers.ReadData(2, "Username");
            string passWord = ExcelLibHelpers.ReadData(2, "Password");
            //Console.WriteLine(Path.Combine(Assembly.GetExecutingAssembly().CodeBase, @"Data\TestData.xlsx"));
            //Console.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\TestData.xlsx"));
            HomePage homePage = loginPage.doLogin(Driver, ExcelLibHelpers.ReadData(2, "Username"), passWord);
            if (!homePage.GetUserAccount(Driver).Text.Contains(userName))
            {
                Console.WriteLine("Login Failed");
                throw new WebDriverException("Login Failed");
            }
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
            if (driver.Url != expectedUrl)
            {
                Assert.Fail("Currently on URL '" + driver.Url + "' but expected '" + expectedUrl + "'");
            }
        }

        public int GenerateRandomNumber(int startRange, int endRange)
        {
            return new Random().Next(startRange, endRange);
        }
    }
}
