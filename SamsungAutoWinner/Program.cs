using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace SamsungAutoWinner
{
    class Program
    {
        static void Main(string[] args)
        {
            var opts = new ChromeOptions();
            opts.AddArgument(@"user-data-dir=Default");

            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), opts))
            {
                driver.Manage().Window.Maximize();
                driver.Url = "https://www.samsungrewards.com/rewards/#/main";

                Console.WriteLine("Login to Samsung and press any key");
                Console.ReadKey(true);
                Console.WriteLine("Press q to win points, w to win galaxy watch");

                string reward = null;
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Q:
                        reward = "//img[@alt='10 Million Point Giveaway*']/..";
                        break;
                    case ConsoleKey.W:
                        reward = "//img[@alt='Win a Galaxy Watch Active.*']/..";
                        break;
                    default:
                        return;
                }

                int played = 0, won = 0;

                while (played < 1000)
                {
                    try
                    {
                        driver.FindElementByXPath(reward).Click();
                        Thread.Sleep(1000);
                        new Actions(driver).MoveToElement(driver.FindElementByXPath("//label[@for='catalog_terms']"), 5, 5).Click().Perform();
                        Thread.Sleep(1000);
                        driver.FindElementByXPath("//button[text()='Play Now']").Click();
                        Thread.Sleep(1000);
                        driver.FindElementByXPath("//button[text()='Confirm']").Click();
                        Thread.Sleep(12000);
                        if (driver.FindElementsByXPath("//h3[text()='Sorry!']").Count == 0)
                            Console.WriteLine($"!!! You won {++won} times !!!");
                        driver.FindElementByXPath("//button[text()='Done']").Click();
                        Thread.Sleep(1000);
                        Console.WriteLine($"You played {++played} times");
                    }
                    catch
                    {
                        try
                        {
                            driver.Url = "https://www.samsungrewards.com/rewards/#/catalog/chance_to_win";
                            Thread.Sleep(5000);
                        }
                        catch
                        {
                            break;
                        }
                    }
                }

                Console.WriteLine("Stopped. Press any key");
                Console.ReadKey(true);
            }
        }
    }
}
