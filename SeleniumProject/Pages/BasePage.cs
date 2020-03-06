using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumProject.Pages
{
	public class BasePage
	{
		public void ClearAndEnter(IWebElement element, string value)
		{
			element.Clear();
			element.SendKeys(value);
		}

		public void ClickOkForPopUp(IWebDriver driver)
		{
			driver.SwitchTo().Alert().Accept();
		}
	}
}
