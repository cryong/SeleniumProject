using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;

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

		public void PageDown(IWebDriver driver)
		{
			driver.FindElement(pageDownLocator).Click();
		}

		public void PageLast(IWebDriver driver)
		{
			driver.FindElement(pageLastLocator).Click();
		}

		public void CreateNewTimeAndMaterial(IWebDriver driver, string code, string description, string price)
		{
			driver.FindElement(createNewButtonLocator).Click();
			driver.FindElement(codeLocator).SendKeys(code);
			driver.FindElement(descriptionLocator).SendKeys(description);
			driver.FindElement(priceLocator).SendKeys(price);
			driver.FindElement(saveButtonLocator).Click();
		}

		public void UpdateTimeAndMaterial(IWebDriver driver, string code, string description, string price, string newCode, string newDescription, string newPrice)
		{
			IWebElement itemToUpdate = Search(driver, code, description, price); // row
			itemToUpdate.FindElement(editButtonLocator).Click();
			IWebElement codeTextField = driver.FindElement(codeLocator);
			codeTextField.Clear();
			codeTextField.SendKeys(newCode);
			IWebElement descriptionTextField = driver.FindElement(descriptionLocator);
			descriptionTextField.Clear();
			descriptionTextField.SendKeys(newDescription);
			IWebElement priceTextField = driver.FindElement(priceLocator);
			priceTextField.Clear();
			IWebElement priceTextField2 = driver.FindElement(By.Id("Price"));
			priceTextField2.Clear();
			priceTextField.SendKeys(newPrice);
			driver.FindElement(saveButtonLocator).Click();

		}

		public void DeleteTimeAndMaterial(IWebDriver driver, string code, string description, string price)
		{
			IWebElement itemToDelete = Search(driver, code, description, price); // row
			itemToDelete.FindElement(deleteButtonLocator).Click();
			// click OK button
			driver.SwitchTo().Alert().Accept();
		}

		public IWebElement Search(IWebDriver driver, string code, string description, string price)
		{
			PageLast(driver);
			// note: just assuming that last row will always be the item that we are looking for... not bothering with row iterations
			IWebElement row = driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[@role='row'][last()]"));
			if (row.FindElement(By.XPath("td[1]")).Text == code && row.FindElement(By.XPath("td[3]")).Text == description)
			{
				return row;
			}

			Console.WriteLine("Unable to locate the item");
			return null;
		}

	}
}
