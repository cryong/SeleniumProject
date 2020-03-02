using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace SeleniumProject.Pages
{
	public class HomePage : BasePage
	{
		By administrationMenuLocator = By.XPath("//a[contains(text(), 'Administration')]");
		By timeAndMaterialMenuLocator = By.XPath("//a[@href='/TimeMaterial']");
		By userAccountLocator = By.XPath("//*[@id=\"logoutForm\"]/ul/li/a");
		By customerMenuLocator = By.XPath("//a[@href='/Client']");

		public void ClickAdministrationMenuLink(IWebDriver driver)
		{
			driver.FindElement(administrationMenuLocator).Click();
		}
		public TimeAndMaterialsPage ClickTimeAndMaterialsMenuLink(IWebDriver driver)
		{
			driver.FindElement(timeAndMaterialMenuLocator).Click();
			return new TimeAndMaterialsPage();
		}

		public IWebElement GetUserAccount(IWebDriver driver)
		{
			return driver.FindElement(userAccountLocator);
		}

		public TimeAndMaterialsPage GoToTimeAndMaterialsPage(IWebDriver driver)
		{
			ClickAdministrationMenuLink(driver);
			return ClickTimeAndMaterialsMenuLink(driver);
		}

		public CustomersPage GoToCustomersPage(IWebDriver driver)
		{
			ClickAdministrationMenuLink(driver);
			driver.FindElement(customerMenuLocator).Click();
			return new CustomersPage();
		}
	}
}
