using System; //konsole
using System.Collections.Generic; //sarasai ir t. t.
using System.IO;//failai
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Dienynas
    {
        public IWebDriver Driver
        {
            get; set;
        }

        public IJavaScriptExecutor Js
        {
            get; set;
        }

        /*public TimeSpan ImplicitWait
        {set
            {
                Driver.Manage().Timeouts().ImplicitWait = value;
                //this.ImplicitWait = value;
            }
        }*/

        public Dienynas() { }

        public Dienynas(bool showWindow = true, bool remote = false, Uri remoteUrl = null)
        {
            //initializing Driver
            /*var browserName="chrome";
            switch (browserName)
            {
                case "firefox": //problematiska //firefox turi kitokias ypatybes nei chrome, todel puslapiu uzkrovimui reikia rasyt viska is naujo
                case "gecko":
                    FirefoxOptions firefoxOptions = new FirefoxOptions();
                    if (!showWindow)
                    {
                        firefoxOptions.AddArguments("headless");
                    }
                    //chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
                    firefoxOptions.PageLoadStrategy = PageLoadStrategy.None;
                    if (remote)
                    {
                        return new RemoteWebDriver(remoteUrl, firefoxOptions);
                    }
                    return new FirefoxDriver(firefoxOptions);*/
            //case "chrome":
            //default:
            ChromeOptions chromeOptions = new ChromeOptions();
            if (!showWindow)
            {
                chromeOptions.AddArguments("headless");
            }
            //chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
            chromeOptions.PageLoadStrategy = PageLoadStrategy.None;
            if (remote)
            {
                try
                {
                    Driver = new RemoteWebDriver(remoteUrl, chromeOptions);
                }
                catch (OpenQA.Selenium.WebDriverException e)
                {
                    Console.WriteLine("OpenQA.Selenium.WebDriverException: {0}", e);
                    Thread.CurrentThread.Abort();
                }
            }
            else
            {
                Driver = new ChromeDriver(chromeOptions);
            }
            Js = (IJavaScriptExecutor)Driver;
            //  break;
            //}
        }
        ~Dienynas()
        {
            Driver.Url = "https://dienynas.tamo.lt/Atsijungti";
            Thread.Sleep(2000);
            Driver.FindElement(By.TagName("body"));
            Js.ExecuteScript("window.stop()");
            Driver.Quit();
        }
        public void Prisijungti(string slapyvardis, string slaptazodis)
        {
            IWebElement elem;
            Driver.Url = "https://dienynas.tamo.lt/Prisijungimas/Login";
            Thread.Sleep(2000);
            elem = Driver.FindElement(By.Id("UserName"));
            elem.Clear();
            elem.SendKeys(slapyvardis);
            elem = Driver.FindElement(By.Id("Password"));
            elem.Clear();
            elem.SendKeys(slaptazodis);
            elem.SendKeys(Keys.Return);//<enter>

            Driver.FindElement(By.TagName("body"));
            Js.ExecuteScript("window.stop()");
        }


        //PAZYMIAI


        public void SkanuotiDalykuPazymius(ref List<Dalykas> dalykai)
        {
            //var iw = Driver.Manage().Timeouts().ImplicitWait;
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(20);

            //pasirenkamas pusmetis
            IWebElement elem;
            Driver.Url = "https://dienynas.tamo.lt/PeriodoVertinimas/MokinioVertinimai";
            Thread.Sleep(2000);
            elem = Driver.FindElement(By.Name("laikotarpis"));
            //Js.ExecuteScript("window.stop()");
            var sElem = new SelectElement(elem);
            sElem.SelectByText("1 pusmetis");

            //susirandama pzymiu lentele
            IList<IWebElement> li, li2, li3;
            //li-dalyku pavadinimai ir mokytojai
            li = Driver.FindElements(By.CssSelector("#c_main > div.c_block.padLess.borderless > div > table > tbody > tr"));
            //li2-dalyku pazymiai
            li2 = Driver.FindElements(By.CssSelector("#slenkanti_dalis > table > tbody > tr"));

            //nustatomas maksimalus elemento ieskojimo laikas
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);



            //keliaujama per dalykus
            for (int i = 0; i < li.Count; i++)
            {

                string pam, mok;
                var lpaz = new List<Pazymys>();
                string[] unpack;

                try
                {
                    unpack = li[i].Text.Split('\n'); //dalykas ir mokytojas
                    pam = unpack[0].Trim();//dalykas
                    mok = unpack[1].Trim();//mokytojas

                    li3 = li2[i].FindElements(By.CssSelector("td > div"));

                    string pazInfo, data;

                    //li3 - dalyko pazymiai
                    foreach (var paz in li3)//heliaujama per dalyko pazymius
                    {
                        //paz pavyzdys
                        /*
                        <div data-toggle="tooltip" data-original-title="<div><b> 2021-02-11</b></div><div>Teorinis darbas</div>" style="text-align:center;cursor:pointer;font-size:15px;display:inline-block;vertical-align:top;margin-right:10px;font-weight:normal;">
                                    <span class=" ">
                                        10
                                    </span>
                            </div>*/

                        pazInfo = paz.GetAttribute("data-original-title");
                        data = "";
                        for (int k = 0; k < pazInfo.Length; k++)// hmmm, html skanavimas tokiu metodu nera gera ideja, gali buti BUG
                        {
                            if ("1234567890-".Contains(pazInfo[k].ToString()))
                            {
                                data += pazInfo[k];
                            }
                        }
                        lpaz.Add(new Pazymys(paz.Text.Trim()));

                    }
                    dalykai.Add(new Dalykas(pam, mok, lpaz));


                    lpaz.ForEach(Console.WriteLine);
                    Console.WriteLine("---");
                }
                catch (IndexOutOfRangeException)//tuscia lenteles skiltis
                {
                    //done
                }
            }

            //Driver.Manage().Timeouts().ImplicitWait = iw;
        }



        //KLASES/NAMU DARBAI


        public void SkanuotiDarbus(ref TextWriter kdFailas, ref TextWriter ndFailas, int klasesN, int namuN, int sek)
        {
            var iw = Driver.Manage().Timeouts().ImplicitWait;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);


            IList<IWebElement> li;
            Driver.Url = "https://dienynas.tamo.lt/Pamoka/Sarasas";
            Thread.Sleep(2000);
            li = Driver.FindElements(By.CssSelector("#duomenys > div.row > div.col-md-12 > div.row"));

            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sek);

            string dalykas, mokytojas, tema, uzduotis = "", uzduotis2 = "";

            Darbas d;
            for (int i = 0; (i < li.Count && (klasesN > 0 || namuN > 0)); i++)//tesiama, kol yra reikalingu skaitytu/neskaitytu zinuciu
            {
                dalykas = li[i].FindElement(By.CssSelector("div > div:nth-child(1)")).Text.Replace("\n", "").Replace("\r", "");
                mokytojas = li[i].FindElement(By.CssSelector("div > div:nth-child(2)")).Text.Replace("\n", "").Replace("\r", "");
                tema = li[i].FindElement(By.CssSelector("div > div:nth-child(3)")).Text.Replace("\n", "").Replace("\r", "");
                try//kartais uzduotys yra, vienos arba abieju nera, nors dalykas, mokytojas ir tema visada parasoma
                {
                    uzduotis2 = li[i].FindElement(By.CssSelector("message-div > div:nth-child(4)")).Text.Replace("\n", "").Replace("\r", "");//klases d
                    if (uzduotis2.Contains("Klas"))//uzduotis1 turi saugoti klases darba, o uzduotis2 namu
                    {
                        uzduotis = uzduotis2;
                        uzduotis2 = li[i].FindElement(By.CssSelector("message-div > div:nth-child(5)")).Text.Replace("\n", "").Replace("\r", "");//namu d
                    }
                }
                catch { }

                d = new Darbas(dalykas, mokytojas, tema, uzduotis);

                Console.WriteLine("{0}. Dalykas: {1}; Mokytojas: {2}; Tema: {3}; Uzduotis (klases): {4}, Uzduotis(namu): {5}", i + 1, dalykas, mokytojas, tema, uzduotis, uzduotis2);

                if (klasesN > 0)
                {
                    d.IFaila(kdFailas);
                    klasesN--;
                }
                else if (namuN > 0)
                {
                    d.IFaila(ndFailas);
                    namuN--;
                }

            }

            Driver.Manage().Timeouts().ImplicitWait = iw;
        }


        //ZINUTES


        public int GautiNeskaitytuZinuciuSkaiciu(int sek)
        {
            //var iw = Driver.Manage().Timeouts().ImplicitWait;
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sek);

            IWebElement elem;
            int neskaitytosZinutes;
            try//BUG: per ilgai
            {
                elem = Driver.FindElement(By.Id("unread_msg_count_badge"));
                neskaitytosZinutes = int.Parse(elem.Text);
            }
            catch (NoSuchElementException)
            {
                neskaitytosZinutes = 0;
            }

            //Driver.Manage().Timeouts().ImplicitWait = iw;
            return neskaitytosZinutes;
        }

        static bool ArNaujaZinute(IWebElement msg)
        {
            if (msg.FindElements(By.ClassName("not-read")).Count > 0) { return true; }
            return false;
        }

        public void GautiZinutes(ref TextWriter nzFailas, ref TextWriter szFailas, int naujuN, int senuN, int sek)
        {
            var iw = Driver.Manage().Timeouts().ImplicitWait;
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0.1);


            IList<IWebElement> li;
            Driver.Url = "https://dienynas.tamo.lt/GoTo/Bendrauk";
            Thread.Sleep(7000);
            li = Driver.FindElements(By.CssSelector("div[role=listitem]"));

            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sek);

            string siuntejas, tema, data;
            bool arNauja;
            Zinute z;
            for (int i = 0; (i < li.Count && (naujuN > 0 || senuN > 0)); i++)//tesiama, kol yra reikalingu skaitytu/neskaitytu zinuciu
            {
                siuntejas = li[i].FindElement(By.ClassName("message-sender")).Text.Trim();
                tema = li[i].FindElement(By.ClassName("message-subject")).Text.Trim();
                data = li[i].FindElement(By.ClassName("message-date")).Text.Trim(); //BUG: problemele //si elementa randa tik narsykle

                arNauja = ArNaujaZinute(li[i]);
                z = new Zinute(tema, siuntejas, data);

                Console.WriteLine("{0}. Siuntejas: {1}; tema: {2}; nauja: {3}; data: {4}", i + 1, siuntejas, tema, arNauja, data);

                if (arNauja && naujuN > 0)
                {
                    li[i].Click();
                    Thread.Sleep(3000);
                    z.Turinys = Driver.FindElement(By.CssSelector("div[class=message-body]")).Text;
                    z.IFaila(nzFailas);
                    Driver.Navigate().Back();
                    naujuN--;
                }
                else if (!arNauja && senuN > 0)
                {
                    li[i].Click();
                    Thread.Sleep(3000);
                    z.Turinys = Driver.FindElement(By.CssSelector("div[class=message-body]")).Text;
                    z.IFaila(szFailas);
                    Driver.Navigate().Back();
                    senuN--;
                }

                Thread.Sleep(3000);

                //https://stackoverflow.com/questions/759966/what-is-the-best-way-to-modify-a-list-in-a-foreach-loop
                li = Driver.FindElements(By.CssSelector("div[role=listitem]"));//nelabai gera ideja, bet kito metodo nezinau
            }

            Driver.Manage().Timeouts().ImplicitWait = iw;
        }
    }
}
