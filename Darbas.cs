using System.IO;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Darbas
    {
        public string Dalykas { get; }
        public string Mokytojas { get; set; }
        public string Tema { get; set; }
        public string Uzduotis { get; }


        public Darbas(string dalykas, string uzduotis)
        {
            Dalykas = dalykas;
            Uzduotis = uzduotis;
        }

        public Darbas(string dalykas, string mokytojas, string tema, string uzduotis)
        {
            Dalykas = dalykas;
            Mokytojas = mokytojas;
            Tema = tema;
            Uzduotis = uzduotis;
        }

        public void IFaila(TextWriter f)
        {
            var sp = new Spausdinimas(200);

            f.WriteLine(sp.PrintLine());
            f.WriteLine(sp.PrintRow(Dalykas, Tema));
            f.WriteLine(sp.PrintRow(Mokytojas));
            f.WriteLine(sp.PrintLine());
            f.WriteLine(sp.PrintRow(Uzduotis));
            f.WriteLine(sp.PrintLine());

        }
    }
}
