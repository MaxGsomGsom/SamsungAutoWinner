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
        static void Wait(int sec) => Thread.Sleep(TimeSpan.FromSeconds(sec));

        static void Main(string[] args)
        {
            var opts = new ChromeOptions();
            opts.AddArgument(@"user-data-dir=Default");

            using (var driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), opts))
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://www.samsungrewards.com/rewards/#/main");

                Console.WriteLine("Login to Samsung and press any key");
                Console.ReadKey(true);
                Console.WriteLine("Press q to win points, w to win galaxy watch");

                string reward;
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

                int played = 0, won = 0, points = 0;

                do
                {
                    try
                    {
                        driver.FindElementByXPath(reward).Click();
                        Wait(1);

                        new Actions(driver).MoveToElement(driver.FindElementByXPath("//label[@for='catalog_terms']"), 5, 5).Click().Perform();
                        Wait(1);

                        driver.FindElementByXPath("//button[text()='Play Now']").Click();
                        Wait(1);

                        driver.FindElementByXPath("//button[text()='Confirm']").Click();
                        Wait(12);

                        if (driver.FindElementsByXPath("//h3[text()='Sorry!']").Count == 0)
                            Console.WriteLine($"!!! You won {++won} times !!!");

                        driver.FindElementByXPath("//button[text()='Done']").Click();
                        Wait(1);

                        points = int.Parse(driver.FindElementByXPath(
                            "//div[@class='mid_content_container']/div/h3[2]/span[1]").Text.Replace(",", ""));
                        Console.WriteLine($"You played {++played} times, points {points}");
                    }
                    catch
                    {
                        driver.Navigate().GoToUrl("https://www.samsungrewards.com/rewards/#/catalog/chance_to_win");
                        Wait(1);
                    }
                }
                while (points > 0);

                Console.WriteLine("Stopped. Press any key");
                Console.ReadKey(true);
            }
        }
    }
}
