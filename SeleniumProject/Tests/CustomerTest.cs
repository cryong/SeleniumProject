﻿using System;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumProject.Data;
using SeleniumProject.Pages;
using SeleniumProject.Utilities;

namespace SeleniumProject.Tests
{
    [TestFixture]
    //[Parallelizable]
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
            CustomersPage customersPage = null;
            Customer customerObject = null;
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                customersPage = homePage.GoToCustomersPage(Driver);
                Contact contactObject = new Contact(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactFirstName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactLastName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactPhoneNumber"));
                customerObject = new Customer(ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "CustomerName"), contactObject, contactObject);

                customersPage.CreateNewCustomer(Driver, customerObject);

                // go back to the summary page
                // creating a new customer does not change the landing page
                // have to explicitly go back to the summary page...
                //Driver.Navigate().Back();
                //customersPage.refreshTable(Driver);
                Driver.Navigate().GoToUrl(customersUrl);

            }
            catch (Exception e)
            {
                Assert.Fail("Unexpected error has occurred - TestAddCustomer failed " + e.Message);
            }
            // verify
            Assert.That(PerformValidation(Driver, customersPage, customerObject), Is.Not.Null, "Unable to create new customer - TestAddCustomer failed");

        }

        [Test]
        public void TestUpdateCustomer()
        {
            HomePage homePage = new HomePage();
            CustomersPage customersPage = null;
            string id = null;
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                customersPage = homePage.GoToCustomersPage(Driver);
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
                Assert.That(customerElementRow, Is.Not.Null, "Unable to create new customer - TestUpdateCustomer failed");
                id = customerElementRow.FindElement(By.XPath("td[1]")).Text;

                Contact updatedContactObject = new Contact(
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactFirstName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactLastName"),
                    ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "ContactPhoneNumber"));

                Customer updatedCustomerObject = new Customer(ExcelLibHelpers.ReadData(GenerateRandomNumber(2, 7), "CustomerName"), updatedContactObject, updatedContactObject);
                customersPage.UpdateCustomer(Driver, id, updatedCustomerObject);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.Fail("Unexpected error has occurred - TestUpdateCustomer failed " + e.Message);
            }

            Assert.That(PerformValidation(Driver, customersPage, id), Is.Not.Null, "Unable to update customer - TestUpdateCustomer failed");

        }

        [Test]
        public void TestDeleteCustomer()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            IWebElement customerRow = null;

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
                customerRow = PerformValidation(Driver, customersPage, customerObject);
                Assert.That(customerRow, Is.Not.Null, "Unable to create new customer - TestDeleteCustomer failed");
                string id = customerRow.FindElement(By.XPath("td[1]")).Text;
                customersPage.DeleteCustomer(Driver, id);
                Driver.Navigate().Refresh();
                //customersPage.refreshTable(Driver);
                customerRow = PerformValidation(Driver, customersPage, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.Fail("Unexpected error has occurred - TestDeleteCustomer failed " + e.Message);
            }

            Assert.That(customerRow, Is.Null, "Unable to delete customer - TestDeleteCustomer failed");
        }

        private IWebElement PerformValidation(IWebDriver driver, CustomersPage customerPage, Customer customer)
        {
            ValidateURL(driver, customersUrl);
            return customerPage.Search(driver, customer);
        }

        private IWebElement PerformValidation(IWebDriver driver, CustomersPage customerPage, string id)
        {
            ValidateURL(driver, customersUrl);
            IWebElement row = customerPage.SearchById(driver, id);
            return row;
        }
    }
}
