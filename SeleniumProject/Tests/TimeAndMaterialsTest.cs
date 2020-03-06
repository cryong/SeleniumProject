using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumProject.Data;
using SeleniumProject.Pages;
using SeleniumProject.Tests;
using SeleniumProject.Utilities;

namespace SeleniumProject
{
    [TestFixture]
    //[Parallelizable]
    class TimeAndMaterialsTest : BaseTest
    {
        private static readonly string timeAndMaterialUrl = "http://horse-dev.azurewebsites.net/TimeMaterial";

        [SetUp]
        public void TestSetUp()
        {
            // load data from spreadsheet for each test
            ExcelLibHelpers.PopulateInCollection(@"..\..\..\Data\TestData.xlsx", "TimeAndMaterial");
        }

        [Test]
        public void TestDragAndDRop()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            TimeAndMaterialsPage timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(Driver);
            timeAndMaterialsPage.dragAndDrop(Driver);
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
            TimeAndMaterialsPage timeAndMaterialsPage = null;
            TimeAndMaterial timeAndMaterialObject = null;
            try
            {
                timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(Driver);
                // Verify that you are on Time & Materials Page by checking the URL
                ValidateURL(Driver, timeAndMaterialUrl);
                // Create new Time and Material item
                // Locate TypeCode dropdown field and Select Time
                // Locate Code textfield and enter test123
                // Locate Description textfield and enter current timestamp as the description for uniqueness
                // Locate Price per unit field and enter 10.00;
                // Locate and click Save Button
                timeAndMaterialObject = CreateNewTimeAndMaterial(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Code"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Price"));
                timeAndMaterialsPage.CreateNewTimeAndMaterial(Driver, timeAndMaterialObject);
                // Verify that the item was added
            }
            catch (WebDriverException e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.Fail("Unexpected exception occurred -  TestAddTimeAndMaterial failed", e);
            }
            Assert.That(PerformVerification(Driver, timeAndMaterialsPage, timeAndMaterialObject), Is.Not.Null, "Time and Material was not added - TestAddTimeAndMaterial failed");
        }

        [Test]
        public void TestUpdateTimeAndMaterial()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            TimeAndMaterialsPage timeAndMaterialsPage = null;
            TimeAndMaterial timeAndMaterialObject = null;
            TimeAndMaterial updatedTimeAndMaterialObject = null;
            try
            {
                timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(Driver);
                // Verify that you are on Time & Materials Page by checking the URL
                ValidateURL(Driver, timeAndMaterialUrl);

                // Create new Time and Material item
                // Locate TypeCode dropdown field and Select Time
                // Locate Code textfield and enter test123
                // Locate Description textfield and enter current timestamp as the description for uniqueness
                // Locate Price per unit field and enter 10.00
                // Locate and click Save Button
                timeAndMaterialObject = CreateNewTimeAndMaterial(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Code"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Price"));
                timeAndMaterialsPage.CreateNewTimeAndMaterial(Driver, timeAndMaterialObject);

                Assert.That(PerformVerification(Driver, timeAndMaterialsPage, timeAndMaterialObject), Is.Not.Null, "Time and Material was not added - TestUpdateTimeAndMaterial failed");

                // Update the item that was added just now
                updatedTimeAndMaterialObject = CreateNewTimeAndMaterial(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Code"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Price"));
                timeAndMaterialsPage.UpdateTimeAndMaterial(Driver, timeAndMaterialObject, updatedTimeAndMaterialObject);

                //verify that the item was updated
            }
            catch (WebDriverException e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.Fail("Unexpected exception occurred -  TestUpdateTimeAndMaterial failed" + e.Message);
            }
            Assert.That(PerformVerification(Driver, timeAndMaterialsPage, updatedTimeAndMaterialObject), Is.Not.Null, "Time and Material was not updated - TestUpdateTimeAndMaterial failed");
        }

        [Test]
        public void TestDeleteTimeAndMaterial()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            TimeAndMaterialsPage timeAndMaterialsPage = null;
            TimeAndMaterial timeAndMaterialObject = null;
            try
            {
                timeAndMaterialsPage = homePage.GoToTimeAndMaterialsPage(Driver);
                // Verify that you are on Time & Materials Page by checking the URL
                ValidateURL(Driver, timeAndMaterialUrl);

                timeAndMaterialObject = CreateNewTimeAndMaterial(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Code"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 6), "Price"));
                timeAndMaterialsPage.CreateNewTimeAndMaterial(Driver, timeAndMaterialObject);
                // Verify that the item was added
                PerformVerification(Driver, timeAndMaterialsPage, timeAndMaterialObject);
                Assert.That(PerformVerification(Driver, timeAndMaterialsPage, timeAndMaterialObject), Is.Not.Null, "Time and Material was not created - TestDeleteTimeAndMaterial failed");
                // Now perform delete by clicking Delete button
                timeAndMaterialsPage.DeleteTimeAndMaterial(Driver, timeAndMaterialObject);
            }
            catch (WebDriverException e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.Fail("Unexpected exception occurred -  TestDeleteTimeAndMaterial failed" + e.Message);
            }
            // verify that deletion was successful
            Assert.That(PerformVerification(Driver, timeAndMaterialsPage, timeAndMaterialObject), Is.Null, "Time and Material was not deleted - TestDeleteTimeAndMaterial failed");
        }

        private IWebElement PerformVerification(IWebDriver driver, TimeAndMaterialsPage page, string code, string timeStamp, string price)
        {
            ValidateURL(driver, timeAndMaterialUrl);
            // Verify that the item was added,edited, or deleted by searching for it in the table
            return page.Search(driver, code, timeStamp, price);
        }

        private IWebElement PerformVerification(IWebDriver driver, TimeAndMaterialsPage page, TimeAndMaterial timeAndMaterial)
        {
            return PerformVerification(driver, page, timeAndMaterial.Code, timeAndMaterial.Description, timeAndMaterial.Price);
        }
    }
}
