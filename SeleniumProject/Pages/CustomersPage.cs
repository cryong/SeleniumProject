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

        public void CreateNewCustomer(IWebDriver driver, Customer customer)
        {
            driver.FindElement(createNewCustomerButtonLocator).Click();
            // fill out name
            driver.FindElement(customerNameLocator).SendKeys(customer.Name);
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
            // check contact check box for simplicity
            driver.FindElement(isSameContactCheckBoxLocator).Click();
            // cick save button
            driver.FindElement(saveButtonLocator).Click();
        }

        public void UpdateCustomer(IWebDriver driver, Customer customer, Customer updatedCustomer)
        {
            Thread.Sleep(5000);
            IWebElement customerToUpdate = Search(driver, customer); // row
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
            //driver.SwitchTo().Frame(editCustomrIframe);
            // check contact check box for simplicity
            //driver.FindElement(isSameContactCheckBoxLocator).Click();
            // cick save button
            driver.FindElement(saveButtonLocator).Click();
        }

        public void PageLast(IWebDriver driver)
        {
            driver.FindElement(pageLastLocator).Click();
        }

        public IWebElement Search(IWebDriver driver, Customer customer)
        {
            Thread.Sleep(3000);
            PageLast(driver);
            // note: just assuming that last row will always be the item that we are looking for... not bothering with row iterations
            IWebElement row = driver.FindElement(By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row'][last()]"));
            Console.WriteLine("what ?" + row.FindElement(By.XPath("td[2]")).Text);
            if (row.FindElement(By.XPath("td[2]")).Text == customer.Name)
            {
                return row;
            }
            Console.WriteLine("Unable to locate customer");
            return null;
        }
    }
}
