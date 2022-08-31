using System;
using System.Collections.Generic;


namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Parametrai
    {
        //public string Parametras { get; set; }
        public string isvestiesVieta { get; set; }
        public string slapyvardis { get; set; }
        public string slaptazodis { get; set; }
        public bool arRodytiChromeLanga { get; set; }
        //public string narsykle { get; set; }
        public bool nuotolinisDriver { get; set; }
        public string nuotolinioDriverAdresas { get; set; }
        //public string laikmatisElementuiIeskotiS { get; set; }
        //public string atsitiktinisUzdelsimas { get; set; }
        //public string uzdelsimoTrukmeNuoMS { get; set; }
        //public string uzdelsimoTrukmeIkiMS { get; set; }
        //public string Pazymiai { get; set; }
        public bool arIeskotiPazymiu { get; set; }
        public string pazymiuLaikotarpis { get; set; }
        public string pazymiuIsvestis { get; set; }
        //public string Pranesimai { get; set; }
        public bool arIeskotiPranesimu { get; set; }
        public int naujuPranesimuSkaicius { get; set; }
        public string naujuPranesimuIsvestis { get; set; }
        public int senuPranesimuSkaicius { get; set; }
        public string senuPranesimuIsvestis { get; set; }
        //public string Pamokos { get; set; }
        public bool arIeskotiDarbu { get; set; }
        public int namuDarbuSkaicius { get; set; }
        public string namuDarbuIsvestis { get; set; }
        public int klasesDarbuSkaicius { get; set; }
        public string klasesDarbuIsvestis { get; set; }
        //public string darbuAtgalDalykuPamokuSkaicius { get; set; }

        public Parametrai()
        {

            //Parametras = "Reiksme";
            isvestiesVieta = @"C:\Users\Nojus\source\repos\NojusSajauskas_JKM_baigiamasis_2020_2021\isvestis\";
            slapyvardis = "slapyvardis";
            slaptazodis = "slaptazodis";
            arRodytiChromeLanga = true;
            //narsykle = "chrome"; //apsiriboju kol kas tik chrome
            nuotolinisDriver = false;
            nuotolinioDriverAdresas = "http://192.168.0.10:9515";
            //laikmatisElementuiIeskotiS = "5";
            //atsitiktinisUzdelsimas = "0";
            //uzdelsimoTrukmeNuoMS = "1500";
            //uzdelsimoTrukmeIkiMS = "3000";
            //Pazymiai = "";
            arIeskotiPazymiu = true;
            pazymiuLaikotarpis = "1pusmetis";
            pazymiuIsvestis = "pazymiai.xlsx";
            //Pamokos = "";
            arIeskotiDarbu = false;
            namuDarbuSkaicius = 3;
            namuDarbuIsvestis = "nd.txt";
            klasesDarbuSkaicius = 2;
            klasesDarbuIsvestis = "kd.txt";
            //darbuAtgalDalykuPamokuSkaicius = "10";
            //Pranesimai = "";
            arIeskotiPranesimu = true;
            naujuPranesimuSkaicius = 1;
            naujuPranesimuIsvestis = "nZin.txt";
            senuPranesimuSkaicius = 3;
            senuPranesimuIsvestis = "sZin.txt";

        }

    }
}
