using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumProject.Pages;

namespace SeleniumProject
{
	class Program
	{
		static void Main(string[] args)
		{
			// Open chrome
			IWebDriver driver = new ChromeDriver();
			// Go to the URL
			driver.Navigate().GoToUrl("http://horse-dev.azurewebsites.net/Account/Login?ReturnUrl=%2f");

			// Maximise the browser
			driver.Manage().Window.Maximize();

			LoginPage loginPage = new LoginPage(driver);
			// Locate ID field and enter Hari
			loginPage.EnterUserName("hari");
			// Locate Password field and enter 123123
			loginPage.EnterPassword("123123");
			// Locate Log in Button and Click
			loginPage.Login();
			// Verify the login was successful
			if (driver.FindElement(By.XPath("//*[@id=\"logoutForm\"]/ul/li/a")).Text == "Hello hari!")
			{
				Console.WriteLine("Login Successful");
			}
			else
			{
				Console.WriteLine("Login Failed");
				driver.Quit();
			}

			HomePage homePage = new HomePage(driver);

			// Locate Administration Menu and Click
			homePage.ClickAdministrationMenuLink();
			// Locate Time & Materials Menu item and Click
			homePage.ClickTimeAndMaterialsMenuLink();
			// Verify that you are on Time & Materials Page by checking the URL http://horse-dev.azurewebsites.net/TimeMaterial
			if (driver.Url != "http://horse-dev.azurewebsites.net/TimeMaterial")
			{
				Console.WriteLine("Test Failed because the current page is not Time & Materials");
				driver.Quit();
			}
			// Create new Time and Material item
			// Locate TypeCode dropdown field and Select Time
			// Locate Code textfield and enter test123
			// Locate Description textfield and enter current timestamp as the description for uniqueness
			// Locate Price per unit field and enter 10.00
			// Locate and click Save Button
			String timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
			Console.WriteLine("Timestamp is " + timeStamp);
			TimeAndMaterialsPage timeAndMaterialsPage = new TimeAndMaterialsPage(driver);
			string code = "test123";
			string price = "10";
			timeAndMaterialsPage.CreateNewTimeAndMaterial(code, timeStamp, price);
			// Verify that you are back on the sumary page but wait for 1 second for synchronization
			Thread.Sleep(1000);
			if (driver.Url != "http://horse-dev.azurewebsites.net/TimeMaterial")
			{
				Console.WriteLine("Test Failed because item was not created");
				driver.Quit();
			}
			// Verify that the item was added
			IWebElement newItemElement = PerformVerification(timeAndMaterialsPage, code , timeStamp, price);
			//driver.Quit();

			// Update the item that was added just now
			string newTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
			Console.WriteLine("New timestamp is " + newTimeStamp);
			string newCode = code + 1;
			string newPrice = price + 2;
			timeAndMaterialsPage.UpdateTimeAndMaterial(code, timeStamp, price, newCode, newTimeStamp, newPrice);

			//verify that the item was updated
			IWebElement updatedItemElement = PerformVerification(timeAndMaterialsPage, newCode, newTimeStamp, newPrice);

			// Now perform delete by clicking Delete button
			timeAndMaterialsPage.DeleteTimeAndMaterial(newCode, newTimeStamp, newPrice);

			// verify that deletion was successful
			IWebElement deletedItemElement = PerformVerification(timeAndMaterialsPage, newCode, newTimeStamp, newPrice);
			if (deletedItemElement != null)
			{
				Console.WriteLine("Test Failed - Delete failed");
				driver.Quit();
			}

			Console.WriteLine("Test Passed - Delete successful");
			driver.Quit();
		}

		static IWebElement PerformVerification(TimeAndMaterialsPage page, string code, string timeStamp, string price)
		{
			// wait 1 second first
			Thread.Sleep(1000);

			// Verify that the item was added,edited, or deleted by searching for it in the table
			return page.Search(code, timeStamp, price);
		}
	}
}
