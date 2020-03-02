using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumProject.Data;
using SeleniumProject.Pages;

namespace SeleniumProject.Tests
{
    [TestFixture]
    class CustomerTest : BaseTest
    {
        private static readonly string customersUrl = "https://horse-dev.azurewebsites.net/Client";

        [Test]
        public void TestAddCustomer()
        {
            HomePage homePage = new HomePage();
            // Locate Administration Menu and Click
            // Locate Time & Materials Menu item and Click
            try
            {
                CustomersPage customersPage = homePage.GoToCustomersPage(driver);
                string name = "customerName";
                Contact contactObject = new Contact("firstName", "lastName", "12345678");
                Customer customerObject = new Customer(name, contactObject, contactObject);

                customersPage.CreateNewCustomer(driver, customerObject);

                // go back to the summary page
                driver.Navigate().GoToUrl(customersUrl);

                // verify
                if (customersPage.Search(driver, customerObject) == null)
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
                CustomersPage customersPage = homePage.GoToCustomersPage(driver);
                string name = "customerName";
                Contact contactObject = new Contact("firstName", "lastName", "12345678");
                Customer customerObject = new Customer(name, contactObject, contactObject);

                customersPage.CreateNewCustomer(driver, customerObject);

                // go back to the summary page
                driver.Navigate().GoToUrl(customersUrl);

                // verify
                if (customersPage.Search(driver, customerObject) == null)
                {
                    Console.WriteLine("Test Failed because the customer was not created");
                    Assert.Fail("Unable to create new customer - TestUpdateCustomer failed");
                }

                Contact updatedContactObject = new Contact("firstName2", "lastName2", "23456789");
                Customer updatedCustomerObject = new Customer(name, contactObject, contactObject);

                customersPage.UpdateCustomer(driver, customerObject, updatedCustomerObject);
                if (customersPage.Search(driver, updatedCustomerObject) == null)
                {
                    Console.WriteLine("Test Failed because the customer was not updated");
                    Assert.Fail("Unable to update customer - TestUpdateCustomer failed");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Assert.Fail("Unexpected error has occurred - TestUpdateCustomer failed ", e);
            }

        }
    }
}
