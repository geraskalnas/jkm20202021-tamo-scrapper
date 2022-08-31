using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Pazymys
    {
        public enum GalimiPazymiai
        {
            p1 = 1, p2, p3, p4, p5, p6, p7, p8, p9, p10, isk = 0, neisk = -1
        }
        public GalimiPazymiai pazymys;
        public Pazymys()
        {

        }
        public Pazymys(int pazymys)
        {
            this.pazymys = (GalimiPazymiai)pazymys;
        }
        public Pazymys(GalimiPazymiai pazymys)
        {
            this.pazymys = pazymys;
        }
        public Pazymys(string pazymys)
        {
            GalimiPazymiai t;
            if (pazymys.StartsWith("ne") && pazymys.EndsWith("sk"))//neisk //i su nosine
            {
                t = GalimiPazymiai.neisk;
            }
            else if (pazymys.EndsWith("sk"))//isk
            {
                t = GalimiPazymiai.isk;
            }
            else
            {
                t = (GalimiPazymiai)int.Parse(pazymys);
            }
            this.pazymys = t;
        }

        public override string ToString()
        {
            switch ((int)this.pazymys)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    return ((int)this.pazymys).ToString();
                case 0:
                    return "iskaita";
                case -1:
                default:
                    return "neiskaita";

            }
        }
    }
}
