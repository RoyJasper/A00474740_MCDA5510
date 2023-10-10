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
using System.Formats.Asn1;
using LINQtoCSV;
using static System.Net.WebRequestMethods;
using Microsoft.VisualBasic;
using System.Diagnostics;
using File = System.IO.File;
using System.Data;

namespace Assignment1
{
    public class Customer
    {
        [Name("First Name")]
        public string First_Name { get; set; }
        [Name("Last Name")]
        public string Last_Name { get; set; }
        [Name("Street Number")]
        public string Street_Number { get; set; }
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
        [Optional]
        public string Field { get; set; }
    }
    public class DirWalker
    {
        public static CsvWriter? csvwriter ;

        private object? output;
        public static StreamWriter? StreamCSVwritter;
        public static string? LogFilepath;
        public static string? csvPath;
        public static int SkippedRows = 0, ValidRows =0;

        public static void Main(String[] args)
        {
            var watch = Stopwatch.StartNew();
            //CSV full data file
            csvPath = @"..\..\..\Output\FullData.csv";
            if (System.IO.File.Exists(csvPath))
            {
               Console.WriteLine("File deleted");
            }
            FileStream fs = System.IO.File.Create(csvPath);
            fs.Close();
            // Log file
            LogFilepath = @"..\..\..\Logs\Logfile.txt";
            if (System.IO.File.Exists(LogFilepath))
            {
                System.IO.File.Delete(LogFilepath);
              Console.WriteLine("Log File newely created");
            }
            FileStream logstream = System.IO.File.Create(LogFilepath);
            logstream.Close();

            StreamCSVwritter = new StreamWriter(csvPath);
            csvwriter = new CsvWriter(StreamCSVwritter, CultureInfo.InvariantCulture);
            csvwriter.WriteHeader<Customer>();
            csvwriter.WriteField("Date");
            csvwriter.NextRecord();

            DirWalker fm = new DirWalker();
            fm.walk(@"..\..\..\Sample Data");
            watch.Stop();
            //Console.WriteLine("Program execution time: {0} Milliseconds {1} {2}", watch.ElapsedMilliseconds, SkippedRows, ValidRows);
            //Writting to log file the execution time and no. of records.
            using (StreamWriter sw = File.AppendText(LogFilepath))
                sw.WriteLine("\nProgram execution time: {0} Milliseconds \nValid Rows: {1} \nSkipped Rows: {2}", watch.ElapsedMilliseconds,  ValidRows, SkippedRows);
            
        }

        public void walk(String path)
        {
            try
            {
                string[] list = Directory.GetDirectories(path);
               // Console.WriteLine("\n ------------------ \nPassed Path " + path);
                if (list == null) return;

                foreach (string dirpath in list)
                {
                    if (Directory.Exists(dirpath))
                    {
                        walk(dirpath);
                       // Console.WriteLine("Dir:" + dirpath);
                    }
                }
                string[] fileList = Directory.GetFiles(path);
                foreach (string filepath in fileList)
                {
                    if (filepath.EndsWith(".csv"))
                    {
                     //   Console.WriteLine("File:" + filepath);
                        ReadCSV(filepath);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }

        public void ReadCSV(string filepath)
        {
            string status=null;
            var reader = new StreamReader(filepath);
            var csvreader = new CsvReader(reader, CultureInfo.InvariantCulture);
            //csvreader.Configuration.MissingFieldFound = null;
            var output = csvreader.GetRecords<Customer>().ToList();
            
            foreach (var cus in output)
            {
                status = RecordChecker(cus);
                if(status == "OK")
                {
                    ValidRows++;
                    WriteCSV(cus, filepath);
                }
                else
                {
                    SkippedRows++;
                    LogBadData(status,cus, filepath);
                }
            }
            //***Data Check
            //List<String> strings = new List<String>(filepath.Split(@"\"));
            //Console.WriteLine("");
        }

        private void LogBadData(string  status, Customer cus, string filepath)
        {
            using (StreamWriter sw = System.IO.File.AppendText(LogFilepath))
            {
                List<String> BadDataFile = new List<String>(filepath.Split(@"Sample Data"));

                sw.WriteLine("First_Name: {0} \t Last_Name: {1} \t Street_Number: {2} \t Street_Name: {3} \t City: {4} \t Province: {5} " +
                "\t Poastal_Code: {6} \t Country: {7} \t Phone: {8} \t Email: {9} -->> \t Reason: {11} \t File Path : {10}",
                cus.First_Name, cus.Last_Name, cus.Street_Number, cus.Street_Name, cus.City, cus.Province, cus.Poastal_Code, cus.Country, cus.Phone, cus.Email
                , BadDataFile[BadDataFile.Count - 1], status);

             /*   sw.WriteLine("First_Name:  \t Last_Name:  \t Street_Number:  \t Street_Name:  \t City:  \t Province:  \t Poastal_Code:  \t Country:  \t Phone:  \t Email:  || \t Reason:  \t File Path:");
                sw.WriteLine( cus.First_Name +"\t"+ cus.Last_Name + "\t" + cus.Street_Number + "\t" + cus.Street_Name + "\t" +
                    cus.City + "\t" + cus.Province + "\t" + cus.Poastal_Code + "\t" + cus.Country + "\t" + cus.Phone + "\t" + cus.Email
                 + "\t" + BadDataFile[BadDataFile.Count - 1] + "\t" + status); */
            }

        }

        private void displayData(Customer cus)
        {
            Console.WriteLine("\n First_Name:{0} \n Last_Name:{1} \n Street_Number:{2} \n Street_Name:{3} \n City:{4} \n Province:{5} " +
                "\n Poastal_Code:{6} " +
                "\n Country:{7} \n Phone:{8} \n Email:{9} ", 
                cus.First_Name , cus.Last_Name,cus.Street_Number,cus.Street_Name,cus.City,cus.Province,cus.Poastal_Code, cus.Country,
                cus.Phone,cus.Email);
           
        }

        public void WriteCSV(Customer cus, string filepath)
        {
            //  csvwriter.WriteField()
            List<String> strings = new List<String>(filepath.Split(@"\"));
            string Date = strings[strings.Count - 4] + "/" + strings[strings.Count - 3] + "/" + strings[strings.Count - 2];
           
            //DateTime DateColum = DateTime.ParseExact(Date, "yyyy/MM/dd", CultureInfo.InvariantCulture);
           
            csvwriter.WriteField(cus.First_Name);
            csvwriter.WriteField(cus.Last_Name);
            csvwriter.WriteField(cus.Street_Number);
            csvwriter.WriteField(cus.Street_Name);
            csvwriter.WriteField(cus.City);
            csvwriter.WriteField(cus.Province);
            csvwriter.WriteField(cus.Poastal_Code);
            csvwriter.WriteField(cus.Country);
            csvwriter.WriteField(cus.Phone);
            csvwriter.WriteField(cus.Email);
            csvwriter.WriteField(Date);
            csvwriter.NextRecord();
        }

        public static string RecordChecker(Customer cus)
        {
           string strRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            Regex re = new Regex(strRegex, RegexOptions.IgnoreCase);
            if (cus.First_Name == "")
                return "Empty First Name";
            if ( cus.Street_Number == "")
                return "Empty Last name";
            if (cus.Street_Name == "")
                return "Empty Street Name";
            if (cus.City == "")
                return "Empty City";
            if (cus.Province == "")
                return "Empty Province";
            if (cus.Poastal_Code == "")
                return "Null Postal code ";
            if (cus.Country == "")
                return "Null County";
            if (cus.Phone == "")
                return "Null Phone";
            if (cus.Email == "")
                return "Null Email";
            else
               {  
               if (re.IsMatch(cus.Email))
                    return "OK";
                else
                   return "Invalid Email";
                 }
        }
    }
}