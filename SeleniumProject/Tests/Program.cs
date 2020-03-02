using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumProject.Data;
using SeleniumProject.Pages;
using SeleniumProject.Tests;

namespace SeleniumProject
{
    [TestFixture]
    class Program : BaseTest
    {
        private static readonly string timeAndMaterialUrl = "http://horse-dev.azurewebsites.net/TimeMaterial";

        [Test]
        public void TestDragAndDRop()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            TimeAndMaterialsPage timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(driver);
            timeAndMaterialsPage.dragAndDrop(driver);
        }

        public TimeAndMaterial CreateNewTimeAndMaterial(string code, string price)
        {
            String timeStamp = CalculateCurrentTimeStamp();
            Console.WriteLine("Timestamp is " + timeStamp);
            TimeAndMaterial timeAndMaterialObject = new TimeAndMaterial(code, timeStamp, price);
            return timeAndMaterialObject;
        }

        [Test]
        public void TestAddTimeAndMaterial()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                TimeAndMaterialsPage timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(driver);
                // Verify that you are on Time & Materials Page by checking the URL
                ValidateURL(driver, timeAndMaterialUrl);
                // Create new Time and Material item
                // Locate TypeCode dropdown field and Select Time
                // Locate Code textfield and enter test123
                // Locate Description textfield and enter current timestamp as the description for uniqueness
                // Locate Price per unit field and enter 10.00;
                // Locate and click Save Button
                TimeAndMaterial timeAndMaterialObject = CreateNewTimeAndMaterial("test123", "10");
                timeAndMaterialsPage.CreateNewTimeAndMaterial(driver, timeAndMaterialObject);
                // Verify that the item was added
                if (PerformVerification(driver, timeAndMaterialsPage, timeAndMaterialObject) == null){
                    Assert.Fail("Time and Material was not added - TestAddTimeAndMaterial failed");
                }
            }
            catch (WebDriverException e)
            {
                Assert.Fail("Unexpected exception occurred -  TestAddTimeAndMaterial failed", e);
            }
        }

        [Test]
        public void TestUpdateTimeAndMaterial()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                TimeAndMaterialsPage timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(driver);
                // Verify that you are on Time & Materials Page by checking the URL
                ValidateURL(driver, timeAndMaterialUrl);

                // Create new Time and Material item
                // Locate TypeCode dropdown field and Select Time
                // Locate Code textfield and enter test123
                // Locate Description textfield and enter current timestamp as the description for uniqueness
                // Locate Price per unit field and enter 10.00
                // Locate and click Save Button
                TimeAndMaterial timeAndMaterialObject = CreateNewTimeAndMaterial("test123", "10");
                timeAndMaterialsPage.CreateNewTimeAndMaterial(driver, timeAndMaterialObject);

                // Update the item that was added just now
                TimeAndMaterial updatedTimeAndMaterialObject = CreateNewTimeAndMaterial("test1234", "11");
                timeAndMaterialsPage.UpdateTimeAndMaterial(driver, timeAndMaterialObject, updatedTimeAndMaterialObject);

                //verify that the item was updated
                if (PerformVerification(driver, timeAndMaterialsPage, updatedTimeAndMaterialObject) == null)
                {
                    Assert.Fail("Time and Material was not updated - TestUpdateTimeAndMaterial failed");
                }
            }
            catch (WebDriverException e)
            {
                Assert.Fail("Unexpected exception occurred -  TestUpdateTimeAndMaterial failed", e);
            }
        }

        [Test]
        public void TestDeleteTimeAndMaterial()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                TimeAndMaterialsPage timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(driver);
                // Verify that you are on Time & Materials Page by checking the URL
                ValidateURL(driver, timeAndMaterialUrl);

                TimeAndMaterial timeAndMaterialObject = CreateNewTimeAndMaterial("test123", "10");
                timeAndMaterialsPage.CreateNewTimeAndMaterial(driver, timeAndMaterialObject);
                // Verify that the item was added
                PerformVerification(driver, timeAndMaterialsPage, timeAndMaterialObject);

                // Now perform delete by clicking Delete button
                timeAndMaterialsPage.DeleteTimeAndMaterial(driver, timeAndMaterialObject);

                // verify that deletion was successful
                IWebElement deletedItemElement = PerformVerification(driver, timeAndMaterialsPage, timeAndMaterialObject);
                if (deletedItemElement != null)
                {
                    Assert.Fail("Test Failed - Delete failed");
                }

            }
            catch (WebDriverException e)
            {
                Assert.Fail("Unexpected exception occurred -  TestDeleteTimeAndMaterial failed", e);
            }
        }

        private static IWebElement PerformVerification(IWebDriver driver, TimeAndMaterialsPage page, string code, string timeStamp, string price)
        {
            // wait 1 second first
            Thread.Sleep(1000);

            ValidateURL(driver, timeAndMaterialUrl);
            // Verify that the item was added,edited, or deleted by searching for it in the table
            return page.Search(driver, code, timeStamp, price);
        }

        private static IWebElement PerformVerification(IWebDriver driver, TimeAndMaterialsPage page, TimeAndMaterial timeAndMaterial)
        {
            return PerformVerification(driver, page, timeAndMaterial.Code, timeAndMaterial.Description, timeAndMaterial.Price);
        }
    }
}
