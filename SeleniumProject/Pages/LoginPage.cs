using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumProject.Pages
{
	public class LoginPage : BasePage
	{

		public LoginPage(IWebDriver driver) : base(driver)
		{
		}

		public void EnterUserName(string userName)
		{
			Driver.FindElement(By.Id("UserName")).SendKeys(userName);
		}

		public void EnterPassword(string password)
		{
			Driver.FindElement(By.Id("Password")).SendKeys(password);
		}

		public void Login()
		{
			Driver.FindElement(By.XPath("//input[@type='submit']")).Click();
		}

	}
}
