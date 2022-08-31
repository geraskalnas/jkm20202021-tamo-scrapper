using System;
using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sukurti COM objektus
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlFile = xlApp.Workbooks.Open(@"C:\Users\Nojus\source\repos\NojusSajauskas_JKM_baigiamasis_2020_2021\Duomenys.xlsx");

            //loading params
            Excel._Worksheet xlParams = xlFile.Sheets[1];
            //[row, column] from 1
            string username = xlParams.Cells[3, 2].value2;
            string password = xlParams.Cells[4, 2].value2;

            bool showChromeWindow = xlParams.Cells[6, 2].value2>0?true:false;
            int maxWebpageLoadingTime = int.Parse(xlParams.Cells[7, 2].value2.ToString());

            //...

            Marshal.ReleaseComObject(xlParams);




            //initializing driver
            ChromeOptions chromeOptions = new ChromeOptions();
            if (!showChromeWindow)
            {
                chromeOptions.AddArguments("headless");
            }

            IWebDriver driver = new ChromeDriver(chromeOptions);

            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(maxWebpageLoadingTime);

            driver.Url = "https://dienynas.tamo.lt/Prisijungimas/Login";

            //IJavaScriptExecutor js = (IJavaScriptExecutor)driver; //Driver is the WebDriver object
            //js.ExecuteScript("document.getElementsByTagName('img')[2].setAttribute('src', 'Hello')");




            //login
            IWebElement elem;
            elem = driver.FindElement(By.Id("UserName"));
            elem.Clear();
            elem.SendKeys(username);
            elem = driver.FindElement(By.Id("Password"));
            elem.Clear();
            elem.SendKeys(password);
            elem.SendKeys(Keys.Return);//<enter>



            //check unread messages count
            int neskaitytosZinutes;
            try
            {
                elem = driver.FindElement(By.Id("unread_msg_count_badge"));
                neskaitytosZinutes = int.Parse(elem.Text);
            }
            catch (NoSuchElementException)
            {
                neskaitytosZinutes = 0;
            }

            driver.Close();


            Console.ReadKey();
            //IŠVALYMAI
            GC.Collect();
            GC.WaitForPendingFinalizers();



            
            //uždaryti
            xlFile.Close();
            Marshal.ReleaseComObject(xlFile);


            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
