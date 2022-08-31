using System;

namespace NojusSajauskas_JKM_baigiamasis_2020_2021
{
    class Spausdinimas
    {
        //Pagal: https://stackoverflow.com/questions/856845/how-to-best-way-to-draw-table-in-console-app-c

        int tableWidth = 110;

        public Spausdinimas() { }

        public Spausdinimas(int dydis)
        {
            this.tableWidth = dydis;
        }
        public string PrintLine()
        {
            return (new string('-', this.tableWidth));
        }

        public string PrintRow(params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            return row;
        }
        public string PrintRow(ref string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            return row;
        }

        public string AlignCentre(string text, int width)
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
    }
}