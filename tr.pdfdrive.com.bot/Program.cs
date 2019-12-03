using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace tr.pdfdrive.com.bot
{
    public class MyClass
    {

    }
    internal class Program
    {
        private static HtmlDocument GetHtmlDocument(string url, string fileName)
        {
            var web = new HtmlWeb();
            if (!File.Exists(fileName))
            {
                var tempDocument = web.Load(url);
                tempDocument.Save(fileName, Encoding.UTF8);
            }
            var document = new HtmlDocument();
            document.Load(fileName);
            return document;
        }
        static void Main(string[] args)
        {
            var query = "orhan";
            var fileName = query + ".txt";
            var rootLink = "https://tr.pdfdrive.com";
            var url = rootLink + "/search?q={0}";
            var link = string.Format(url, query);
            var document = GetHtmlDocument(link, fileName);
            

            var list = document.DocumentNode.SelectNodes("//div[@class='files-new']//li");
            foreach (var item in list.Take(1))
            {
                var kitapLink = item.SelectSingleNode(".//div[@class='file-left']//a").Attributes["href"].Value;
                Console.WriteLine(kitapLink);
                Console.WriteLine(item.SelectSingleNode(".//div[@class='file-left']//img").Attributes["src"].Value);
                Console.WriteLine(item.SelectSingleNode(".//div[@class='file-right']//a").InnerText.Trim());
                Console.WriteLine(item.SelectSingleNode(".//div[@class='file-info']").InnerText.Trim());
                var temp1 = GetHtmlDocument(rootLink + kitapLink, kitapLink.Replace("/", ""));
                Console.WriteLine(temp1.DocumentNode.SelectSingleNode("//a[@id='download-button-link']").Attributes["href"].Value);
                var temp2 = GetHtmlDocument(rootLink + kitapLink, kitapLink.Replace("/", ""));
                var temp3 = temp2.DocumentNode.SelectSingleNode("//a[@id='download-button-link']").Attributes["href"].Value;
                Console.WriteLine(temp3);
                IWebDriver webDriver = new ChromeDriver();
                WebDriverWait wait = new WebDriverWait(webDriver,TimeSpan.FromSeconds(40));
                webDriver.Navigate().GoToUrl(rootLink + temp3);
                wait.Until<bool>(d => {
                    try
                    {
                        Console.WriteLine(webDriver.PageSource);

                        return d.FindElement(By.XPath("//a[@class='btn btn-success btn-responsive']")) == null;
                    }
                    catch (Exception)
                    {

                        return true;
                    }                                       
                });
            }
        }
    }
}
