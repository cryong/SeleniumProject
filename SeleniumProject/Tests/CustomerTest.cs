using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumProject.Data;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    [TestFixture]
    [Parallelizable]
    class CustomerTest : BaseTest
    {
        private static readonly string customersUrl = "https://horse-dev.azurewebsites.net/Client";

        [SetUp]
        public void TestSetUp()
        {
            // load data from spreadsheet for each test
            ExcelLibHelpers.PopulateInCollection(@"..\..\..\Data\TestData.xlsx", "Customer");
        }

        [Test]
        public void TestAddCustomer()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                CustomersPage customersPage = homePage.GoToCustomersPage(Driver);
                Contact contactObject = new Contact(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactFirstName"), 
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactLastName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactPhoneNumber"));
                Customer customerObject = new Customer(ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "CustomerName"), contactObject, contactObject);

                customersPage.CreateNewCustomer(Driver, customerObject);

                // go back to the summary page
                // creating a new customer does not change the landing page
                // have to explicitly go back to the summary page...
                Driver.Navigate().GoToUrl(customersUrl);

                // verify
                if (PerformValidation(Driver, customersPage, customerObject) == null)
                {
                    Console.WriteLine("Test Failed because the customer was not created");
                    Assert.Fail("Unable to create new customer - TestAddCustomer failed");
                }

            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected error has occurred - TestAddCustomer failed", e);
            }

        }

        [Test]
        public void TestUpdateCustomer()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                CustomersPage customersPage = homePage.GoToCustomersPage(Driver);
                Contact contactObject = new Contact(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactFirstName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactLastName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactPhoneNumber"));
                Customer customerObject = new Customer(ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "CustomerName"), contactObject, contactObject);

                customersPage.CreateNewCustomer(Driver, customerObject);
                // go back to the summary page
                Driver.Navigate().GoToUrl(customersUrl);

                // verify
                IWebElement customerElementRow = PerformValidation(Driver, customersPage, customerObject);
                if (customerElementRow == null)
                {
                    Console.WriteLine("Test Failed because the customer was not created");
                    Assert.Fail("Unable to create new customer - TestUpdateCustomer failed");
                }

                Contact updatedContactObject = new Contact(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactFirstName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactLastName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactPhoneNumber"));
                Customer updatedCustomerObject = new Customer(ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "CustomerName"), updatedContactObject, updatedContactObject);

                string id = customerElementRow.FindElement(By.XPath("td[1]")).Text;
                customersPage.UpdateCustomer(Driver, id, updatedCustomerObject);
                if (PerformValidation(Driver, customersPage, id) == null)
                {
                    Console.WriteLine("Test Failed because the customer was not updated");
                    Assert.Fail("Unable to update customer - TestUpdateCustomer failed");
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected error has occurred - TestUpdateCustomer failed ", e);
            }

        }

        [Test]
        public void TestDeleteCustomer()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                CustomersPage customersPage = homePage.GoToCustomersPage(Driver);
                Contact contactObject = new Contact(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactFirstName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactLastName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactPhoneNumber"));
                Customer customerObject = new Customer(ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "CustomerName"), contactObject, contactObject);

                customersPage.CreateNewCustomer(Driver, customerObject);
                // go back to the summary page
                Driver.Navigate().GoToUrl(customersUrl);

                // verify
                IWebElement customerElementRow = PerformValidation(Driver, customersPage, customerObject);
                if (customerElementRow == null)
                {
                    Console.WriteLine("Test Failed because the customer was not created");
                    Assert.Fail("Unable to create new customer - TestDeleteCustomer failed");
                }

                string id = customerElementRow.FindElement(By.XPath("td[1]")).Text;
                customersPage.DeleteCustomer(Driver, id);

                if (PerformValidation(Driver, customersPage, id) != null)
                {
                    Console.WriteLine("Test Failed because the customer was not deleted");
                    Assert.Fail("Unable to delete customer - TestDeleteCustomer failed");
                }
            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected error has occurred - TestDeleteCustomer failed " + e.Message);
            }
        }

        private IWebElement PerformValidation(IWebDriver driver, CustomersPage customerPage, Customer customer)
        {
            // wait 1 second first
            Thread.Sleep(1000);

            ValidateURL(driver, customersUrl);
            return customerPage.Search(driver, customer);
        }

        private IWebElement PerformValidation(IWebDriver driver, CustomersPage customerPage, string id)
        {
            // wait 1 second first
            Thread.Sleep(1000);

            ValidateURL(driver, customersUrl);
            return customerPage.SearchById(driver, id);
        }
    }
}
