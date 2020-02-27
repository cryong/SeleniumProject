using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumProject.Pages;

namespace SeleniumProject
{
    class Program
    {
        private static readonly string loginUrl = "http://horse-dev.azurewebsites.net/Account/Login?ReturnUrl=%2f";
        private static readonly string timeAndMaterialUrl = "http://horse-dev.azurewebsites.net/TimeMaterial";

        static void Main(string[] args)
        {
            // Open chrome
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3);
            // Go to the URL
            driver.Navigate().GoToUrl(loginUrl);

            // Maximise the browser
            driver.Manage().Window.Maximize();

            try
            {
                LoginPage loginPage = new LoginPage();
                // Locate ID field and enter Hari
                string userName = "hari";
                loginPage.EnterUserName(driver, userName);
                // Locate Password field and enter 123123
                loginPage.EnterPassword(driver, "123123");
                // Locate Log in Button and Click
                HomePage homePage = loginPage.Login(driver);
                // Verify the login was successful
                Thread.Sleep(3000);

                if (!homePage.GetUserAccount(driver).Text.Contains(userName))
                {
                    Console.WriteLine("Login Failed");
                    throw new WebDriverException("Login Failed");
                }
                // Locate Administration Menu and Click
                homePage.ClickAdministrationMenuLink(driver);
                // Locate Time & Materials Menu item and Click
                TimeAndMaterialsPage timeAndMaterialsPage = homePage.ClickTimeAndMaterialsMenuLink(driver);
                // Verify that you are on Time & Materials Page by checking the URL
                if (driver.Url != timeAndMaterialUrl)
                {
                    Console.WriteLine("Test Failed because the current page is not Time & Materials");
                    throw new WebDriverException("Unable to load Time & Materials page");
                }

                // Create new Time and Material item
                // Locate TypeCode dropdown field and Select Time
                // Locate Code textfield and enter test123
                // Locate Description textfield and enter current timestamp as the description for uniqueness
                // Locate Price per unit field and enter 10.00
                // Locate and click Save Button
                String timeStamp = calculateCurrentTimeStamp();
                Console.WriteLine("Timestamp is " + timeStamp);
                string code = "test123";
                string price = "10";
                timeAndMaterialsPage.CreateNewTimeAndMaterial(driver, code, timeStamp, price);

                // Verify that the item was added
                PerformVerification(driver, timeAndMaterialsPage, code, timeStamp, price);

                // Update the item that was added just now
                string newTimeStamp = calculateCurrentTimeStamp();
                Console.WriteLine("New timestamp is " + newTimeStamp);
                string newCode = "1234";
                string newPrice = "20";
                timeAndMaterialsPage.UpdateTimeAndMaterial(driver, code, timeStamp, price, newCode, newTimeStamp, newPrice);

                //verify that the item was updated
                IWebElement updatedItemElement = PerformVerification(driver, timeAndMaterialsPage, newCode, newTimeStamp, newPrice);

                // Now perform delete by clicking Delete button
                timeAndMaterialsPage.DeleteTimeAndMaterial(driver, newCode, newTimeStamp, newPrice);

                // verify that deletion was successful
                IWebElement deletedItemElement = PerformVerification(driver, timeAndMaterialsPage, newCode, newTimeStamp, newPrice);
                if (deletedItemElement != null)
                {
                    Console.WriteLine("Test Failed - Delete failed");
                    driver.Quit();
                }

                Console.WriteLine("Test Passed - Delete successful");
            }
            catch (WebDriverException e)
            {
                Console.WriteLine("Exception occurred : ", e);
            }
            finally
            {
                driver.Quit();
            }

        }

        private static IWebElement PerformVerification(IWebDriver driver, TimeAndMaterialsPage page, string code, string timeStamp, string price)
        {
            // wait 1 second first
            Thread.Sleep(1000);

            if (driver.Url != timeAndMaterialUrl)
            {
                throw new WebDriverException("Something unexpected has occurred while performing add/update/delete");
            }
            // Verify that the item was added,edited, or deleted by searching for it in the table
            return page.Search(driver, code, timeStamp, price);
        }

        private static string calculateCurrentTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(); ;
        }
    }
}
