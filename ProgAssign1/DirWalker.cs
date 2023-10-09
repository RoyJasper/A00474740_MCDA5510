using System;
using CsvHelper;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;

namespace Assignment1
{
    public class Customer
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int Street_Number { get; set; }
        public string Street_Name { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Poastal_Code { get; set; }
        public string Country { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
    }
    public class DirWalker
    {
        public static void Main(String[] args)
        {
            DirWalker fm = new DirWalker();
            fm.walk(@"..\..\..\Sample Data");
        }

        public void walk(String path)
        {
            try
            {
                string[] list = Directory.GetDirectories(path);
                Console.WriteLine("\n ------------------ \nPassed Path " + path);
                if (list == null) return;

                foreach (string dirpath in list)
                {
                    if (Directory.Exists(dirpath))
                    {
                        walk(dirpath);
                        Console.WriteLine("Dir:" + dirpath);
                    }
                }
                string[] fileList = Directory.GetFiles(path);
                foreach (string filepath in fileList)
                {
                    // string strRegex = @"*.csv";
                    // Regex re = new Regex(strRegex, RegexOptions.IgnoreCase);
                    // if (re.IsMatch(filepath))
                    if (filepath.EndsWith(".csv"))
                    {
                        Console.WriteLine("File:" + filepath);
                        ReadCSV(filepath);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        private void ReadCSV(string filepath)
        {
            var reader = new StreamReader(filepath);
            var csvreader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var output = csvreader.GetRecords<dynamic>().ToList();
            Console.WriteLine("");

        }
    }
}
