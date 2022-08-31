using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Dalykas
    {
        public string pavadinimas;
        public string mokytojas;
        public List<Pazymys> pazymiai;

        public Dalykas()
        {

        }
        public Dalykas(string pavadinimas)
        {
            this.pavadinimas = pavadinimas;
        }
        public Dalykas(string pavadinimas, List<Pazymys> pazymiai)
        {
            this.pavadinimas = pavadinimas;
            this.pazymiai = pazymiai;
        }
        public Dalykas(string pavadinimas, string mokytojas)
        {
            this.pavadinimas = pavadinimas;
            this.mokytojas = mokytojas;
        }
        public Dalykas(string pavadinimas, string mokytojas, List<Pazymys> pazymiai)
        {
            this.pavadinimas = pavadinimas;
            this.mokytojas = mokytojas;
            this.pazymiai = pazymiai;
        }
    }
}
