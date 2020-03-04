using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using SeleniumProject.Data;

namespace SeleniumProject.Pages
{
    public class CustomersPage : BasePage
    {

        By pageDownLocator = By.XPath("//a[@title='Go to the previous page']");
        By pageLastLocator = By.XPath("//a[@title='Go to the last page']");
        By customerNameLocator = By.Id("Name");
        By editContactButtonLocator = By.Id("EditContactButton");
        By contactFirstNameLocator = By.XPath("//input[@id='FirstName']");
        By contactLastNameLocator = By.XPath("//input[@id='LastName']");
        By contactPhoneNumberLocator = By.XPath("//input[@id='Phone']");
        By saveContactButtonLocator = By.XPath("//input[@value='Save Contact']");
        By sameContactCheckBoxLocator = By.Id("IsSameContact");
        By saveButtonLocator = By.XPath("//input[@value='Save']");
        By isSameContactCheckBoxLocator = By.Id("IsSameContact");
        By createNewCustomerButtonLocator = By.XPath("//a[text()='Create New']");

        By editButtonLocator = By.XPath("td/a[@class='k-button k-button-icontext k-grid-Edit']");
        By deleteButtonLocator = By.XPath("td/a[@class='k-button k-button-icontext k-grid-Delete']");
        By contactDetailFrame = By.XPath("//iframe[@title='Edit Contact']");
        By editCustomerFrame = By.XPath("//iframe[@title='Edit Client']");

        public void PageDown(IWebDriver driver)
        {
            driver.FindElement(pageDownLocator).Click();
        }

        public void CreateNewCustomer(IWebDriver driver, Customer customer)
        {
            driver.FindElement(createNewCustomerButtonLocator).Click();
            // fill out name
            driver.FindElement(customerNameLocator).SendKeys(customer.Name);
            // check contact check box for simplicity
            driver.FindElement(isSameContactCheckBoxLocator).Click();
            // click editContactButtonLocator
            driver.FindElement(editContactButtonLocator).Click();
            // fill out contact first name and last name and phone number
            Thread.Sleep(2000);
            IWebElement iframeXpath = driver.FindElement(contactDetailFrame);
            // switching to Edit Contact Form iframe
            driver.SwitchTo().Frame(iframeXpath);
            driver.FindElement(contactFirstNameLocator).SendKeys(customer.CustomerContact.FirstName);
            driver.FindElement(contactLastNameLocator).SendKeys(customer.CustomerContact.LastName);
            driver.FindElement(contactPhoneNumberLocator).SendKeys(customer.CustomerContact.PhoneNumber);
            // click save contact button
            driver.FindElement(saveContactButtonLocator).Click();
            // switching iframe back
            driver.SwitchTo().DefaultContent();
            // cick save button
            driver.FindElement(saveButtonLocator).Click();
        }

        public void UpdateCustomer(IWebDriver driver, string id, Customer updatedCustomer)
        {
            Thread.Sleep(5000);
            IWebElement customerToUpdate = SearchById(driver, id); // row
            // fill out name
            customerToUpdate.FindElement(editButtonLocator).Click();
            // new client edit iframe
            IWebElement editCustomrIframe = driver.FindElement(editCustomerFrame);
            driver.SwitchTo().Frame(editCustomrIframe);
            ClearAndEnter(driver.FindElement(customerNameLocator), updatedCustomer.Name);
            // click editContactButtonLocator
            driver.FindElement(editContactButtonLocator).Click();
            // fill out contact first name and last name and phone number
            Thread.Sleep(2000);
            IWebElement editContactFrame = driver.FindElement(contactDetailFrame);
            // switching to Edit Contact Form iframe
            driver.SwitchTo().Frame(editContactFrame);
            ClearAndEnter(driver.FindElement(contactFirstNameLocator), updatedCustomer.CustomerContact.FirstName);
            ClearAndEnter(driver.FindElement(contactLastNameLocator), updatedCustomer.CustomerContact.LastName);
            ClearAndEnter(driver.FindElement(contactPhoneNumberLocator), updatedCustomer.CustomerContact.PhoneNumber);
            // click save contact button
            driver.FindElement(saveContactButtonLocator).Click();
            // switching iframe back
            driver.SwitchTo().ParentFrame();
            // cick save button
            driver.FindElement(saveButtonLocator).Click();
        }

        public void DeleteCustomer(IWebDriver driver, string id)
        {
            Thread.Sleep(2000);
            SearchById(driver, id).FindElement(deleteButtonLocator).Click();
            driver.SwitchTo().Alert().Accept();
        }

        public void PageLast(IWebDriver driver)
        {
            driver.FindElement(pageLastLocator).Click();
        }

        public IWebElement Search(IWebDriver driver, Customer customer)
        {
            Thread.Sleep(3000);
            PageLast(driver);
            // note: just assuming that last row will always be the item that we are looking for
            IWebElement row = driver.FindElement(By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row'][last()]"));
            if (row.FindElement(By.XPath("td[2]")).Text == customer.Name)
            {
                return row;
            }
            Console.WriteLine("Unable to locate customer");
            return null;
        }

        public IWebElement SearchById(IWebDriver driver, string id)
        {
            Thread.Sleep(3000);
            PageLast(driver);
            // current page number 
            int totalPageNumbers = int.Parse(driver.FindElement(By.ClassName("k-state-selected")).Text);
            int intId = int.Parse(id);
            for (var i = 0; i < totalPageNumbers; i++)
            {
                var initialRows = driver.FindElements(By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row']"));
                int firstRowId = int.Parse(initialRows[0].FindElement(By.XPath("td[1]")).Text);
                int lastRowId = int.Parse(initialRows[initialRows.Count - 1].FindElement(By.XPath("td[1]")).Text);
                if (firstRowId > intId && lastRowId > intId)
                {
                    // hasn't reached the right page
                    // page down and iterate rows
                    PageDown(driver);
                    continue;
                }
                if (firstRowId < intId && lastRowId < intId)
                {
                    // already have gone past
                    // fail and return
                    return null;
                }
                foreach (var possibleRow in initialRows)
                {
                    if (int.Parse(possibleRow.FindElement(By.XPath("td[1]")).Text) == intId)
                    {
                        return possibleRow;
                    }
                }
                //var rows = driver.FindElements(By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row']/td[1][text()='" + id + "']/parent::tr"));
                //if (rows.Count > 1)
                //{
                //    return rows[0];
                //}
                //PageDown(driver);
            }
            return null;
        }
    }
}
