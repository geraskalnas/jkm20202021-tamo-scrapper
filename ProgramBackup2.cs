using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Program
    {
        static int tableWidth = 110;
    static void PrintLine()
    {
        Console.WriteLine(new string('-', tableWidth));
    }

    static void PrintRow(params string[] columns)
    {
        int width = (tableWidth - columns.Length) / columns.Length;
        string row = "|";

        foreach (string column in columns)
        {
            row += AlignCentre(column, width) + "|";
        }

        Console.WriteLine(row);
    }

    static string AlignCentre(string text, int width)
    {
        text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

        if (string.IsNullOrEmpty(text))
        {
            return new string(' ', width);
        }
        else
        {
            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
    }

//tiktai tikrinimui
public static double eval(string expression)  //https://stackoverflow.com/questions/6052640/is-there-an-eval-function-in-c
       {  
           System.Data.DataTable table = new System.Data.DataTable();  
           table.Columns.Add("expression", string.Empty.GetType(), expression);  
           System.Data.DataRow row = table.NewRow();  
           table.Rows.Add(row);  
           return double.Parse((string)row["expression"]);  
       }

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

            ///new
            bool scanGrades = true;
            string scanGradesRange = "1";//1 pusmetis
            string gradesExcelOutputFile = "grades.xlsx";

            bool readHomeWork = true;
            string readHomeWorkRange = "pending";//month, lweek, week, yesterday, today, tommorow, pending
            bool readClassWork = true;
            string readClassWorkRange = "pending";//month, lweek, week, yesterday, today, tommorow, pending
            string hCWorkTextOutputFile="hc.txt";
            
            bool readNewMessages = true;
            int maxNewMessages = 5;
            string newMessagesTextOutputFile = "new_msg.txt";
            bool readOldMessages = true;
            int maxOldMessages = 2; // reiks padaryt kad, -1 reikstu visas
            string oldMessagesTextOutputFile = "old_msg.txt";


            //...

            Marshal.ReleaseComObject(xlParams);

            List<Dalykas> dalykai = new List<Dalykas>();


            //initializing driver
            ChromeOptions chromeOptions = new ChromeOptions();
            if (!showChromeWindow)
            {
                chromeOptions.AddArguments("headless");
            }
            //chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
            chromeOptions.PageLoadStrategy = PageLoadStrategy.None;

            IWebDriver driver = new ChromeDriver(chromeOptions);


            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(maxWebpageLoadingTime);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(maxWebpageLoadingTime);
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(d => d.FindElement(By.Id("footer")).Displayed);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            IWebElement elem, elem2;
            IList<IWebElement> li, li2;


            //login
            driver.Url = "https://dienynas.tamo.lt/Prisijungimas/Login";
            elem = driver.FindElement(By.Id("UserName"));
            elem.Clear();
            elem.SendKeys(username);
            elem = driver.FindElement(By.Id("Password"));
            elem.Clear();
            elem.SendKeys(password);
            elem.SendKeys(Keys.Return);//<enter>

            driver.FindElement(By.TagName("body"));
            js.ExecuteScript("window.stop()");


            driver.Url = "https://dienynas.tamo.lt/PeriodoVertinimas/MokinioVertinimai";
            elem = driver.FindElement(By.Name("laikotarpis"));
            js.ExecuteScript("window.stop()");
            var sElem = new SelectElement(elem);
            sElem.SelectByText("1 pusmetis");


            li = driver.FindElements(By.CssSelector("#c_main > div.c_block.padLess.borderless > div > table > tbody > tr"));
            li2 = driver.FindElements(By.CssSelector("#slenkanti_dalis > table > tbody > tr > td > div"));
            js.ExecuteScript("window.stop()");
            //li - dalykai pavadinimai ir mokytoju vardai
            //li2 - pazymiai

            //keliaujama per dalykus
            foreach (var liZip in li.Zip(li2, (pam, paz) => new Tuple<IWebElement, IWebElement>(pam, paz))){
                string pam, mok;
                var lpaz = new List<Pazymys>();
                string[] unpack;

                try
                {
                    unpack = liZip.Item1.Text.Split('\n'); //dalykas ir mokytojas
                    pam = unpack[0].Trim();//dalykas
                    mok = unpack[1].Trim();//mokytojas
                    string[] paz = liZip.Item2.Text.Split(' ');//pazymiai string tipo masyve

                    for (int i = 0; i < paz.Length; i++)
                    {
                        //catch (FormatException)
                        lpaz.Add(new Pazymys(paz[i].Trim()));
                    }
                    dalykai.Add(new Dalykas(pam, mok, lpaz));
                }
                catch (IndexOutOfRangeException)//buna dalyku, kuriuose pazymiu nera
                {
                    pam = "";
                    mok = "";
                    string[] paz = {};
                }

                Console.WriteLine();
                

                
            }

            //foreach()


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

            /*
            bool dbg = false;
            while (dbg)
            {
                Console.WriteLine("{0}", eval(Console.ReadLine()));
            }*/

            driver.Url = "https://dienynas.tamo.lt/GoTo/Bendrauk";
            li = driver.FindElements(By.CssSelector("div[role=listitem]"));


            int j = 0;
            foreach(IWebElement tempElem in li)
            {
                Console.WriteLine("{0}. {1}", ++j, elem.Text);
                elem.Click();
                break;
            }

            //visos zinutes pagal css div[role=listitem]

            //class: message-person
            //class: message-date
            //class: row?


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
