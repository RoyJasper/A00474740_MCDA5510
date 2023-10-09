﻿using System;
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
    }
    public class DirWalker
    {
        public CsvWriter? csvwriter ;
        private object? output;
        public static string? LogFilepath;

        public static void Main(String[] args)
        {
            
            //CSV full data file
            string csvPath = @"..\..\..\Output\FullData.csv";
            if (System.IO.File.Exists(csvPath))
            {
                System.IO.File.Delete(csvPath);
               Console.WriteLine("File newely created");
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

            StreamWriter writter = new StreamWriter(csvPath);
            CsvWriter csvwriter = new CsvWriter(writter, CultureInfo.InvariantCulture);

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
            int status;
            var reader = new StreamReader(filepath);
            var csvreader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var output = csvreader.GetRecords<Customer>().ToList();
            //Console.WriteLine(output.ToArray());
            foreach (var cus in output)
            {
                //Console.WriteLine(cus.Email, cus.Phone);
                status = RecordChecker(cus);
                if(status == 0)
                {
                    //Console.WriteLine("Good record :");
                   // displayData(cus);
                }
                else if (status == 1)
                {
                    Console.WriteLine("Bad record ");
                    displayData(cus);
                    LogBadData(cus, filepath);
                }
            }
            //***Data Check

            List<String> strings = new List<String>(filepath.Split(@"\"));
           // Console.WriteLine(strings[strings.Count-1]+" Has "+ output.Count);
            Console.WriteLine("");
          // csvwriter.WriteRecords(output);
        }

        private void LogBadData(Customer cus, string filepath)
        {
            using (StreamWriter sw = System.IO.File.AppendText(LogFilepath))
            {
                List<String> BadDataFile = new List<String>(filepath.Split(@"Sample Data"));

                sw.WriteLine("\t First_Name:{0} \t Last_Name:{1} \t Street_Number:{2} \t Street_Name:{3} \t City:{4} \t Province:{5} " +
                "\t Poastal_Code:{6} \t Country:{7} \t Phone:{8} \t Email:{9} , File Path : {10}",
                cus.First_Name, cus.Last_Name, cus.Street_Number, cus.Street_Name, cus.City, cus.Province, cus.Poastal_Code, cus.Country, cus.Phone, cus.Email
                , BadDataFile[BadDataFile.Count - 1]);
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

        public void WriteCSV(string filepath)
        { 
            //  csvwriter.WriteField()
            var csvFileDescription = new CsvFileDescription 
            {
                FirstLineHasColumnNames = true,
                SeparatorChar = ','
            };
           // var csvContext = new CsvHelper.CsvContext();
         //   csvContext.Write(output,filepath,csvFileDescription);
        }

        public static int RecordChecker(Customer cus)
        {
           string strRegex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            Regex re = new Regex(strRegex, RegexOptions.IgnoreCase);
            if (cus.First_Name == "" || cus.Street_Number == "" || cus.Street_Name == "" || cus.City == "" || cus.Province == ""
                || cus.Poastal_Code == "" || cus.Country == "" || cus.Phone == "" || cus.Email == "")
                return 1;
              else
                 {  
               if (re.IsMatch(cus.Email))
                    return 0;
                else
                   return 1;
                 }
        }
    }
}
