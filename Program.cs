using System; //konsole
using System.Collections.Generic; //sarasai ir t. t.
using System.Runtime.InteropServices; //excel uzdarymas
using Microsoft.Office.Interop;
using Excel =Microsoft.Office.Interop.Excel; //excel
using System.IO; //failu irasymas;
using OpenQA.Selenium;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Program
    {
        //PRIES NAUDOJANT!
        //turi buti idiegta chrome/chromium ir chromedriver.exe (sis failas yra projekto aplanke ir ji galima atsisiuti is https://chromedriver.chromium.org/) aplanko vieta turi buti irasyta i aplinkos PATH kintamaji
        //nustatymai yra tekstiniame faile duom.yml , jei jo nera - paleidus programa jis bus sukurtas su numatytomis reiksmemis
        //dauguma atveju nebutina, bet naudojant startRemoteChromedriverLinux.sh , jo eiluciu uzbaigimai turi buti pakeisti \r\n -> \n , tai galima padaryti su dos2unix
        //
        //Naudojamos c sharp bibliotekos: openqa.selenium, excel interop, yamldotnet. 
        //Failai:
        /*
        Program.cs - paleidykle
        duom.yaml - konfiguracija (gali buti bin/Debug aplanke)
        planas.txt - issikelti dalykai //nesvarbu programos veikimui
        scrible.txt - dabar nereikalingi funkciju kodai ir t.t. //nesvarbu programos veikimui
        Spausdinimas.cs - 3 lenteliu spausdinimo funkcijos is interneto (ju nekuriau, tik pritaikiau sios programos veikimui)
        Dienynas.cs - funkcijos veiksmams su dienynu (daugiausiai kodo)
        Dalykas.cs - saugo pazymiu klases
        Pazymys.cs - vieno pazymio enum, data...
        Darbas.cs - saugo dienyno klases/namu darbo info (dalykas, mokytojas, tema, uzduotis), leidzia tai spausdint i faila
        zinute.cs - saugo zinutes tema, data, siunteja, turini, leidzia spausdint i faila 
        startRemoteChromedriverLinux.sh ir startRemoteChromedriverWindows.bat - chromedriver paleidimo programos, jei chrome ir chromedriver idiegta nuotoliniame kompiuteryje
        chromedriver.exe - chromedriver
        */
        //Paleidimas:
        //kai si programa ir narsykle paleidziama ant sio kompiuterio:
        /*
        1. Idiegti chrome
        2. prideti chromedriver.exe vieta i PATH (as esu idejes chromedriver.exe i sio projekto aplanka, todel galima i PATH prideti sio projekto aplanka)
        3. nustatyti duom.yaml
        4. Paspausti Start
        */

        //kai chromedriver ir chrome yra kitame kompiuteryje
        /*
        1.Idiegti chrome, paruosti chromedriver i kompiuteri 2
        2.ant kompiuterio 1 nustatyti duom.yaml
            2.1. nustatyti nuotolinisDriver: false
            2.2. nustatyti nuotolinioDriverAdresas: http://<kito-kompiuterio-ip>:9515
        3. ant 2 kompiuterio paleisti
            3.1 jei sistema windows - startRemoteChromedriverWindows.bat
                3.1.1. irasyti 1 kompiuterio ip i baltaji sarasa, kuris yra bat faile
            3.2 jei sistema ubuntu
                3.2.1. irasyti 1 kompiuterio ip i baltaji sarasa, kuris yra bat faile
            $ sudo apt-get update && sudo apt-get install dos2unix chromium chromium-chromedriver
            $ dos2unix startRemoteChromedriverLinux.sh
            $ chmod +x startRemoteChromedriverLinux.sh
            $ ./startRemoteChromedriverLinux.sh
        3. Nustatyti duom.yaml
        4. Ant 1 kompiuterio paspausti Start
        */




        static Parametrai Ikelti(string ivestis)
        {
            //https://github.com/aaubry/YamlDotNet
            //https://dotnetfiddle.net/CQ7ZKi
            string yml;

            if (!File.Exists(ivestis))//jei failo nera irasomi numatytieji
            {
                var parametrai = new Parametrai();

                var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
                yml = serializer.Serialize(parametrai);

                TextWriter fw = new StreamWriter(@ivestis, false, System.Text.Encoding.GetEncoding(1257));
                fw.Write(yml);
                fw.Close();

                return parametrai;
            }
            
            TextReader f = new StreamReader(@ivestis, System.Text.Encoding.GetEncoding(1257));
            yml = f.ReadToEnd();
            f.Close();

            var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

            return deserializer.Deserialize<Parametrai>(yml); //grazina "parametrai" klases objekta

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Gaunami nustatymai");

            var parametrai = Ikelti("duom.yaml");
            
            Excel.Application xlApp = new Excel.Application();
            xlApp.Visible = false;
            xlApp.DisplayAlerts = false;

            Excel.Workbook xlFile;

            var dalykai = new List<Dalykas>();
            //var zinutes = new List<Zinute>();

            Console.WriteLine("Paleidziama chrome");
            var dienynas = new Dienynas(parametrai.arRodytiChromeLanga, parametrai.nuotolinisDriver, new Uri(parametrai.nuotolinioDriverAdresas));

            //Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(maxWebpageLoadingTime);
            dienynas.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


            Console.WriteLine("Prisijungiama prie dienyno");
            dienynas.Prisijungti(parametrai.slapyvardis, parametrai.slaptazodis);


            Console.WriteLine("Gaunami pazymiai");
            if (parametrai.arIeskotiPazymiu)
            {
                dienynas.SkanuotiDalykuPazymius(ref dalykai);//BUG: nera semestro pasirinkimo

                xlFile = xlApp.Workbooks.Open(@parametrai.isvestiesVieta+parametrai.pazymiuIsvestis);

                //pazymiu excel failo lapas irasymui
                Excel._Worksheet pazVisi = xlFile.Sheets[1];

                //j - ordinate (keliaujama per dalykus), i  - abcise (keliaujama per pazymius)
                for(int j = 0; j < dalykai.Count; j++)
                {
                    pazVisi.Cells[j+1, 1].Value2 = dalykai[j].pavadinimas;//dalyko pavadinimas, pvz. matematika

                    for(int i = 0; i < dalykai[j].pazymiai.Count; i++)
                    {
                        pazVisi.Cells[j + 1, i + 2].Value2 = dalykai[j].pazymiai[i].ToString();//dalyko pazymys
                    }

                }
                xlFile.Save();

                Marshal.ReleaseComObject(pazVisi);
                Marshal.ReleaseComObject(xlFile);
                xlApp.Quit();


            }


            Console.WriteLine("Gaunami klases/namu darbai");
            if (parametrai.arIeskotiDarbu)
            {
                TextWriter kdFailas = new StreamWriter(@parametrai.isvestiesVieta + parametrai.klasesDarbuIsvestis, false, System.Text.Encoding.GetEncoding(1257));
                TextWriter ndFailas = new StreamWriter(@parametrai.isvestiesVieta + parametrai.namuDarbuIsvestis, false, System.Text.Encoding.GetEncoding(1257));

                dienynas.SkanuotiDarbus(ref kdFailas, ref ndFailas, parametrai.klasesDarbuSkaicius, parametrai.namuDarbuSkaicius, 2);

                kdFailas.Close();
                ndFailas.Close();
            }

            Console.WriteLine("Gaunamos zinutes");
            //dienynas.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            if (parametrai.arIeskotiPranesimu)
            {

                TextWriter nzFailas = new StreamWriter(@parametrai.naujuPranesimuIsvestis, false, System.Text.Encoding.GetEncoding(1257));
                TextWriter szFailas = new StreamWriter(@parametrai.senuPranesimuIsvestis,  false, System.Text.Encoding.GetEncoding(1257));


                int neskZinSkc = dienynas.GautiNeskaitytuZinuciuSkaiciu(2);//patikrint ar ju is vis yra

                neskZinSkc = dienynas.GautiNeskaitytuZinuciuSkaiciu(2);

                dienynas.GautiZinutes(ref nzFailas, ref szFailas, Math.Min(neskZinSkc, parametrai.naujuPranesimuSkaicius), parametrai.senuPranesimuSkaicius, 5);

                nzFailas.Close();
                szFailas.Close();
            }


            Console.WriteLine("Programa baige darba. Paspauskite <enter>");
            Console.ReadLine();


            GC.Collect();
            GC.WaitForPendingFinalizers();

            //xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
