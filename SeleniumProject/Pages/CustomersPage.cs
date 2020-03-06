using System;
using System.Threading;
using OpenQA.Selenium;
using SeleniumProject.Data;
using SeleniumProject.Utilities;

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
        By saveButtonLocator = By.XPath("//input[@value='Save']");
        By isSameContactCheckBoxLocator = By.Id("IsSameContact");
        By createNewCustomerButtonLocator = By.XPath("//a[text()='Create New']");

        By editButtonLocator = By.XPath("td/a[@class='k-button k-button-icontext k-grid-Edit']");
        By deleteButtonLocator = By.XPath("td/a[@class='k-button k-button-icontext k-grid-Delete']");
        By contactDetailFrame = By.XPath("//iframe[@title='Edit Contact']");
        By editCustomerFrame = By.XPath("//iframe[@title='Edit Client']");
        By customerRowsLocator = By.XPath("//*[@id='clientsGrid']/div[2]/table/tbody/tr");
        By customerTableRefreshLocator = By.XPath("//a[@title='Refresh']");

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
            //Thread.Sleep(2000);
            SynchronizationHelper.WaitForVisibility(driver, contactDetailFrame, 10);
            IWebElement contactDetailForm = driver.FindElement(contactDetailFrame);
            // switching to Edit Contact Form iframe
            driver.SwitchTo().Frame(contactDetailForm);
            ClearAndEnter(driver.FindElement(contactFirstNameLocator), customer.CustomerContact.FirstName);
            ClearAndEnter(driver.FindElement(contactLastNameLocator), customer.CustomerContact.LastName);
            ClearAndEnter(driver.FindElement(contactPhoneNumberLocator), customer.CustomerContact.PhoneNumber);
            // click save contact button
            driver.FindElement(saveContactButtonLocator).Click();
            // switching iframe back
            driver.SwitchTo().DefaultContent();
            // cick save button
            SynchronizationHelper.WaitForClickability(driver, saveButtonLocator, 10);
            driver.FindElement(saveButtonLocator).Click();
        }

        public void UpdateCustomer(IWebDriver driver, string id, Customer updatedCustomer)
        {
            //Thread.Sleep(5000);
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
            //Thread.Sleep(2000);
            SynchronizationHelper.WaitForVisibility(driver, contactDetailFrame, 10);
            IWebElement editContactForm = driver.FindElement(contactDetailFrame);
            // switching to Edit Contact Form iframe
            driver.SwitchTo().Frame(editContactForm);
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
            //Thread.Sleep(2000);
            SynchronizationHelper.WaitForVisibility(driver, customerRowsLocator, 10);
            IWebElement row = SearchById(driver, id);
            SynchronizationHelper.WaitForClickability(driver, row.FindElement(deleteButtonLocator), 10);
            Thread.Sleep(10000);
            row.FindElement(deleteButtonLocator).Click();
            ClickOkForPopUp(driver);
        }

        public void PageLast(IWebDriver driver)
        {
            // need to wait until a row is displayed in the table before clicking page last button
            SynchronizationHelper.WaitForVisibility(driver, customerRowsLocator, 10);
            driver.FindElement(pageLastLocator).Click();
        }

        public IWebElement Search(IWebDriver driver, Customer customer)
        {
            //Thread.Sleep(3000);
            PageLast(driver);
            // note: just assuming that last row will always be the item that we are looking for
            SynchronizationHelper.WaitForVisibility(driver, By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row'][last()]"), 10);
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
            //Thread.Sleep(3000);
            PageLast(driver);
            // current page number
            //refreshTable(driver);
            int totalPageNumbers = int.Parse(driver.FindElement(By.ClassName("k-state-selected")).Text);
            int intId = int.Parse(id);
            for (var i = 0; i < totalPageNumbers; i++)
            {
                SynchronizationHelper.WaitForVisibility(driver, By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row']"), 10);
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

        public void refreshTable(IWebDriver driver)
        {
            SynchronizationHelper.WaitForVisibility(driver, customerRowsLocator, 10);
            driver.FindElement(customerTableRefreshLocator).Click();
        }
    }
}
