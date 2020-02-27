﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace SeleniumProject.Pages
{
	public class LoginPage : BasePage
	{
		By userNameLocator = By.Id("UserName");
		By passwordLocator = By.Id("Password");
		By loginButtonLocator = By.XPath("//input[@type='submit']");

		public void EnterUserName(IWebDriver driver, string userName)
		{
			driver.FindElement(userNameLocator).SendKeys(userName);
		}

		public void EnterPassword(IWebDriver driver, string password)
		{
			driver.FindElement(passwordLocator).SendKeys(password);
		}

		public HomePage Login(IWebDriver driver)
		{
			driver.FindElement(loginButtonLocator).Click();
			return new HomePage();
		}

	}
}
