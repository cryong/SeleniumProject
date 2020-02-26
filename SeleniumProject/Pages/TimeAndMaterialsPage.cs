using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;

namespace SeleniumProject.Pages
{
	public class TimeAndMaterialsPage : BasePage
	{
		public TimeAndMaterialsPage(IWebDriver driver) : base(driver)
		{
		}

		public void PageUp()
		{
			Driver.FindElement(By.XPath("//a[@title='Go to the next page']")).Click();
		}

		public void PageDown()
		{
			Driver.FindElement(By.XPath("//a[@title='Go to the previous page']")).Click();
		}

		public void PageLast()
		{
			Driver.FindElement(By.XPath("//a[@title='Go to the last page']")).Click();
		}

		public void CreateNewTimeAndMaterial(string code, string description, string price)
		{
			Driver.FindElement(By.XPath("//a[text()='Create New']")).Click();
			Driver.FindElement(By.Id("Code")).SendKeys("test123");
			Driver.FindElement(By.Id("Description")).SendKeys(description);
			Driver.FindElement(By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[4]/div/span[1]/span/input[1]")).SendKeys(price);
			Driver.FindElement(By.Id("SaveButton")).Click();
		}

		public void UpdateTimeAndMaterial(string code, string description, string price, string newCode, string newDescription, string newPrice)
		{
			IWebElement itemToUpdate = Search(code, description, price); // row
			itemToUpdate.FindElement(By.XPath("td[5]/a[@class='k-button k-button-icontext k-grid-Edit']")).Click();
			IWebElement codeTextField = Driver.FindElement(By.Id("Code"));
			codeTextField.Clear();
			codeTextField.SendKeys(newCode);
			IWebElement descriptionTextField = Driver.FindElement(By.Id("Description"));
			descriptionTextField.Clear();
			descriptionTextField.SendKeys(newDescription);
			IWebElement priceTextField = Driver.FindElement(By.XPath("//*[@id=\"TimeMaterialEditForm\"]/div/div[4]/div/span[1]/span/input[1]"));
			priceTextField.Clear();
			IWebElement priceTextField2 = Driver.FindElement(By.Id("Price"));
			priceTextField2.Clear();
			priceTextField.SendKeys(newPrice);
			//Thread.Sleep(10000);
			Driver.FindElement(By.Id("SaveButton")).Click();

		}

		public void DeleteTimeAndMaterial(string code, string description, string price)
		{
			IWebElement itemToDelete = Search(code, description, price); // row
			itemToDelete.FindElement(By.XPath("td[5]/a[@class='k-button k-button-icontext k-grid-Delete']")).Click();
			// check that confirmation pop up is visible
			//string popupText = driver.SwitchTo().Alert().Text;
			//if (popupText == "Are you sure you want to delete this record?")
			//{
			//	Console.WriteLine("Test Passed - Delete confirmation popup dialog visible");
			//}
			// click OK button
			Driver.SwitchTo().Alert().Accept();
		}

		public IWebElement Search(string code, string description, string price)
		{
			//IWebElement tableElement = Driver.FindElement(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table"));
			int totalNumberOfPages = Convert.ToInt32(Driver.FindElement(By.XPath("//a[@title='Go to the last page']")).GetAttribute("data-page"));
			Console.WriteLine("number of pages : " + totalNumberOfPages);
			List<IWebElement> tableRowElements = null;
			PageLast();
			//Console.WriteLine("code : " + code + " description : " + description + " price: " + price);
			// note: it takes too long to iterate through all pages so just limit it to about 5 pages.. should be sufficient.
			for (int i = totalNumberOfPages; i > totalNumberOfPages - 5; i--)
			{
				tableRowElements = new List<IWebElement>(Driver.FindElements(By.XPath("//*[@id=\"tmsGrid\"]/div[3]/table/tbody/tr[@role='row']")));
				if (tableRowElements.Count == 0)
				{
					// just go to previous page if there are no rows on the current page
					PageDown();
					continue;
				}
				foreach (var row in tableRowElements)
				{
					if (row.FindElement(By.XPath("td[1]")).Text == code && row.FindElement(By.XPath("td[3]")).Text == description)
						//&&
						//row.FindElement(By.XPath("td[4]")).Text.Contains(price))
					{
						return row;
					}
				}
				PageDown();
			}
			Console.WriteLine("Unable to locate the item");
			return null;
		}

	}
}
