using System;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes; 
using System.Text.RegularExpressions;
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Metadata;

namespace Assignment1
{
    public class Customer
    {
        [Name("First Name")]
        public string First_Name { get; set; }
        [Name("Last Name")]
        public string Last_Name { get; set; }
        [Name("Street Number")]
        public int Street_Number { get; set; }
        [Name("Street")]
        public string Street_Name { get; set; }
        [Name("City")]
        public string City { get; set; }
        [Name("Province")]
        public string Province { get; set; }
        [Name("Postal Code")]
        public string Poastal_Code { get; set; }
        [Name("Country")]
        public string Country { get; set; }
        [Name("Phone Number")]
        public string Phone { get; set; }
        [Name("email Address")]
        public string Email { get; set; }
    }
    public class DirWalker
    {

        public static void Main(String[] args)
        {
            DirWalker fm = new DirWalker();
            fm.walk(@"..\..\..\Sample Data");
            string csvPath = @"..\..\..\Output";
            StreamWriter writter = new StreamWriter(csvPath);
            CsvWriter csvwriter = new CsvWriter(writter, CultureInfo.InvariantCulture);

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
                    // if (re.IsMatch(filepath))		filepath	"..\\..\\..\\Sample Data\\2017\\11\\19\\CustomerData0.csv"	string

                    if (filepath.EndsWith(".csv"))
                    {
                        Console.WriteLine("File:" + filepath);
                        ReadCSV(filepath);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public void ReadCSV(string filepath)
        {
            var reader = new StreamReader(filepath);
            var csvreader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var output = csvreader.GetRecords<Customer>().ToList();
            List<String> strings = new List<String>(filepath.Split(@"\"));
            Console.WriteLine(strings[strings.Count - 1]+" Has "+ output.Count);
            Console.WriteLine("");
        }

        public void WriteCSV(string filepath)
        {

        }
    }
}
