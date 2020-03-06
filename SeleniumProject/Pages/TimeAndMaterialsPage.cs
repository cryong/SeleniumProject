using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using SeleniumProject.Data;
using SeleniumProject.Utilities;

namespace SeleniumProject.Pages
{
    public class TimeAndMaterialsPage : BasePage
    {
        By pageDownLocator = By.XPath("//a[@title='Go to the previous page']");
        By pageLastLocator = By.XPath("//a[@title='Go to the last page']");
        By codeLocator = By.Id("Code");
        By descriptionLocator = By.Id("Description");
        By priceLocator = By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[4]/div/span[1]/span/input[1]");
        By saveButtonLocator = By.Id("SaveButton");
        By editButtonLocator = By.XPath("td/a[@class='k-button k-button-icontext k-grid-Edit']");
        By deleteButtonLocator = By.XPath("td/a[@class='k-button k-button-icontext k-grid-Delete']");
        By createNewButtonLocator = By.XPath("//a[text()='Create New']");
        By codeColumnLocator = By.XPath("//th[@data-field='Code' and @data-role='sortable']/a");
        By dragDropTargetLocator = By.XPath("//div[@data-role='droptarget']");
        By timeAndMaterialsRowsLocator = By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[@role='row']");

        public void PageDown(IWebDriver driver)
        {
            driver.FindElement(pageDownLocator).Click();
        }

        public void PageLast(IWebDriver driver)
        {
            // need to wait until a row is displayed in the table before clicking page last button
            //SynchronizationHelper.WaitForVisibility(driver, pageLastLocator, 10);
            //SynchronizationHelper.WaitForClickability(driver, pageLastLocator, 10);
            SynchronizationHelper.WaitForElementToBeHidden(driver, By.Id("loader"), 10);
            IWebElement pageLast = driver.FindElement(pageLastLocator);
            // rare situation where page last button is disabled because there are no table rows
            // this can happen when the user deletes a row from the table and the page only had a single record
            // proceeding to click page down instead
            pageLast.Click();
        }

        public void CreateNewTimeAndMaterial(IWebDriver driver, string code, string description, string price)
        {
            driver.FindElement(createNewButtonLocator).Click();
            driver.FindElement(codeLocator).SendKeys(code);
            driver.FindElement(descriptionLocator).SendKeys(description);
            driver.FindElement(priceLocator).SendKeys(price);
            driver.FindElement(saveButtonLocator).Click();
        }

        public void CreateNewTimeAndMaterial(IWebDriver driver, TimeAndMaterial timeAndMaterial)
        {
            CreateNewTimeAndMaterial(driver, timeAndMaterial.Code, timeAndMaterial.Description, timeAndMaterial.Price);
        }

        public void UpdateTimeAndMaterial(IWebDriver driver, string code, string description, string price, string newCode, string newDescription, string newPrice)
        {
            IWebElement itemToUpdate = Search(driver, code, description, price); // row
            itemToUpdate.FindElement(editButtonLocator).Click();
            ClearAndEnter(driver.FindElement(codeLocator), newCode);
            IWebElement descriptionTextField = driver.FindElement(descriptionLocator);
            ClearAndEnter(driver.FindElement(descriptionLocator), newDescription);
            IWebElement priceTextField = driver.FindElement(priceLocator);
            priceTextField.Clear();
            // FIXME investigate why this needs to be done to update the field value...
            IWebElement priceTextField2 = driver.FindElement(By.Id("Price"));
            priceTextField2.Clear();
            priceTextField.SendKeys(newPrice);
            driver.FindElement(saveButtonLocator).Click();
        }

        public void UpdateTimeAndMaterial(IWebDriver driver, TimeAndMaterial currentTimeAndMaterial, TimeAndMaterial newTimeAndMaterial)
        {
            UpdateTimeAndMaterial(driver,
                currentTimeAndMaterial.Code,
                currentTimeAndMaterial.Description,
                currentTimeAndMaterial.Price,
                newTimeAndMaterial.Code,
                newTimeAndMaterial.Description,
                newTimeAndMaterial.Price);
        }

        public void DeleteTimeAndMaterial(IWebDriver driver, string code, string description, string price)
        {
            SynchronizationHelper.WaitForVisibility(driver, timeAndMaterialsRowsLocator, 10);
            IWebElement itemToDelete = Search(driver, code, description, price); // row
            SynchronizationHelper.WaitForClickability(driver, itemToDelete.FindElement(deleteButtonLocator), 10);
            itemToDelete.FindElement(deleteButtonLocator).Click();
            // click OK button
            ClickOkForPopUp(driver);
        }

        public void DeleteTimeAndMaterial(IWebDriver driver, TimeAndMaterial timeAndMaterial)
        {
            DeleteTimeAndMaterial(driver, timeAndMaterial.Code, timeAndMaterial.Description, timeAndMaterial.Price);
        }

        public IWebElement Search(IWebDriver driver, string code, string description, string price)
        {
            PageLast(driver);
            // note: just assuming that last row will always be the item that we are looking for... not bothering with row iterations
            SynchronizationHelper.WaitForVisibility(driver, By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[@role='row'][last()]"), 10);
            IWebElement row = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[@role='row'][last()]"));
            if (row.FindElement(By.XPath("td[1]")).Text == code && row.FindElement(By.XPath("td[3]")).Text == description)
            {
                return row;
            }

            Console.WriteLine("Unable to locate the item");
            return null;
        }

        public IWebElement Search(IWebDriver driver, TimeAndMaterial timeAndMaterial)
        {
            return Search(driver, timeAndMaterial.Code, timeAndMaterial.Description, timeAndMaterial.Price);
        }

        public void dragAndDrop(IWebDriver driver)
        {
            Actions builder = new Actions(driver);

            builder.ClickAndHold(driver.FindElement(codeColumnLocator))
                .MoveToElement(driver.FindElement(dragDropTargetLocator)).Release()
                .Build().Perform();
        }

        public IWebElement SearchById(IWebDriver driver, string id)
        {
            SynchronizationHelper.WaitForElementToBeHidden(driver, By.Id("loader"), 10);
            PageLast(driver);
            // current page number
            int totalPageNumbers = int.Parse(driver.FindElement(By.ClassName("k-state-selected")).Text);
            int intId = int.Parse(id);
            for (var i = 0; i < totalPageNumbers; i++)
            {
                SynchronizationHelper.WaitForVisibility(driver, By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row']"), 10);
                var initialRows = driver.FindElements(By.XPath("//*[@id=\"clientsGrid\"]/div[2]/table/tbody/tr[@role='row']"));
                Console.WriteLine("rows : " + initialRows.Count);
                if (initialRows.Count == 0)
                {
                    PageDown(driver);
                    continue;
                }
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
            SynchronizationHelper.WaitForVisibility(driver, timeAndMaterialsRowsLocator, 10);
            driver.FindElement(timeAndMaterialsRowsLocator).Click();
        }

    }
}
