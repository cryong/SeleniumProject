using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace SeleniumProject.Pages
{
	public class BasePage
	{
		private IWebDriver driver;

		public BasePage(IWebDriver driver)
		{
			this.driver = driver;
		}

		public IWebDriver Driver { get => driver; set => driver = value; }
	}
}
