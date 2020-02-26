using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace SeleniumProject.Pages
{
	public class HomePage : BasePage
	{
		public HomePage(IWebDriver driver) : base(driver)
		{
		}

		public void ClickAdministrationMenuLink()
		{
			Driver.FindElement(By.XPath("//a[contains(text(), 'Administration')]")).Click();
		}
		public void ClickTimeAndMaterialsMenuLink()
		{
			Driver.FindElement(By.XPath("//a[@href='/TimeMaterial']")).Click();
		}
	}
}
