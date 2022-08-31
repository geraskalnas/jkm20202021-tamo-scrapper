using System.IO;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Zinute
    {
        public string Tema { get; }
        public string Siuntejas { get; }
        public string Data { get; }
        public string  Turinys { get; set; }

        public Zinute(string tema, string siuntejas, string data)
        {
            Tema = tema;
            Siuntejas = siuntejas;
            Data = data;
        }

        public void IFaila(TextWriter f)
        {
            var sp = new Spausdinimas(200);

            f.WriteLine(sp.PrintLine());
            f.WriteLine(sp.PrintRow(Siuntejas, Tema));
            f.WriteLine(sp.PrintLine());
            if (Turinys.Length > 0)
            {
                string[] eils = Turinys.Split('\n');
                foreach (var eil in eils)
                {
                    f.WriteLine(sp.PrintRow(eil));
                }
                f.WriteLine(sp.PrintLine());
            }
        }
    }
}
