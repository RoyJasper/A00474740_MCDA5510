using System;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.Text.RegularExpressions;
using CsvHelper.TypeConversion;
using System.Diagnostics;
using File = System.IO.File;

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
        public static CsvWriter? csvwriter;

        private object? output;
        public static StreamWriter? StreamCSVwritter;
        public static string? LogFilepath;
        public static string? csvPath;
        public static int SkippedRows = 0, ValidRows = 0;

        public static void Main(String[] args)
        {
            try
            {
                var watch = Stopwatch.StartNew();
                //CSV full data file

                csvPath = @"..\..\..\Output\FullData.csv";
                if (System.IO.File.Exists(csvPath))
                {
                    System.IO.File.Delete(csvPath);
                    // Console.WriteLine("File deleted");
                }
                FileStream fs = System.IO.File.Create(csvPath);
                fs.Close();
                // Log file
                LogFilepath = @"..\..\..\Logs\Logfile.log";
                if (System.IO.File.Exists(LogFilepath))
                {
                    System.IO.File.Delete(LogFilepath);
                    // Console.WriteLine("Log File newely created");
                }
                FileStream logstream = System.IO.File.Create(LogFilepath);
                logstream.Close();

                //CSV Writter
                StreamCSVwritter = new StreamWriter(csvPath);
                csvwriter = new CsvWriter(StreamCSVwritter, CultureInfo.InvariantCulture);
                csvwriter.WriteHeader<Customer>();
                csvwriter.WriteField("Date");
                csvwriter.NextRecord();

                DirWalker fm = new DirWalker();

                // fm.walk(@"..\..\..\Sample Data");
                // String folder_path =
                Console.WriteLine("Enter the folder path: ");
                String fol_path = Console.ReadLine();
                if (fol_path != null)
                {
                    Console.WriteLine("Appending all CSV files to a single file...");
                    fm.walk(fol_path);
                }
                else
                {
                    Console.WriteLine("Enter valid path.\n");
                    return;
                }
                watch.Stop();
                Console.WriteLine(@"The processed file is stored in Output\FullData.csv");
                //  Console.WriteLine("\nProgram execution time: {0} Milliseconds \nValid Rows: {1} \nSkipped Rows: {2}", watch.ElapsedMilliseconds, ValidRows, SkippedRows);
                //Writting to log file the execution time and no. of records.
                if (ValidRows != 0)
                    using (StreamWriter sw = File.AppendText(LogFilepath))
                        sw.WriteLine("\nProgram execution time: {0} Seconds \nValid Rows: {1} \nSkipped Rows: {2}", watch.ElapsedMilliseconds / 1000, ValidRows, SkippedRows);
                else
                    Console.WriteLine("The Folder doesn't contain any csv files. ");
                
            }catch(Exception e)
            {
               // Console.WriteLine(e.GetType());
                Console.WriteLine(e.GetType().ToString().Split(".")[e.GetType().ToString().Split(".").Length-1].Replace("Exception",""));    
            }
            
        }

        public void walk(String path)
        {
            string exfile = null;
            
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
                        //Console.WriteLine("File:" + filepath);
                        exfile = filepath;
                        ReadCSV(filepath);
                    }
                }
            }
            catch (CsvHelper.HeaderValidationException ex) { Console.WriteLine("Invalid file format. Please check the file: "+exfile); }
        }

        public void ReadCSV(string filepath)
        {
            // int i = 0;
            try
            {
                //  int i = 0;
                string status = null;
                var reader = new StreamReader(filepath);
                var csvreader = new CsvReader(reader, CultureInfo.InvariantCulture);
                //csvreader.Configuration.MissingFieldFound = null;

                var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
                {

                };

                var output = csvreader.GetRecords<Customer>().ToList();

                foreach (var cus in output)
                {

                    status = RecordChecker(cus);
                    // status = RecordChecker(output[i]);

                    if (status == "OK")
                    {
                        ValidRows++;
                        WriteCSV(cus, filepath);
                        //  WriteCSV(output[i], filepath);
                    }
                    else
                    {
                        SkippedRows++;
                        LogBadData(status, cus, filepath);
                        // LogBadData(status, output[i], filepath);
                    }


                }
            }
            catch (CsvHelper.MissingFieldException ex)
            {
                SkippedRows++;
                //Console.WriteLine("Occured in write \n" + ex.ToString());
                String bad_record = (ex.ToString().Split("RawRecord:")[1]).Split("at")[0].Trim();
                LogBadData("Few records", bad_record, filepath);
            }

            //***Data Check
            //List<String> strings = new List<String>(filepath.Split(@"\"));
            //Console.WriteLine("read complete");
        }

        private void LogBadData(string status, string bad_record, string filepath)
        {
            using (StreamWriter sw = System.IO.File.AppendText(LogFilepath))
            {
                List<String> BadDataFile = new List<String>(filepath.Split(@"Sample Data"));

                sw.WriteLine("Record: {0} -->> \t Reason: {1} \t File Path : {2}",bad_record, status, BadDataFile[BadDataFile.Count - 1]);

                /*   sw.WriteLine("First_Name:  \t Last_Name:  \t Street_Number:  \t Street_Name:  \t City:  \t Province:  \t Poastal_Code:  \t Country:  \t Phone:  \t Email:  || \t Reason:  \t File Path:");
                   sw.WriteLine( cus.First_Name +"\t"+ cus.Last_Name + "\t" + cus.Street_Number + "\t" + cus.Street_Name + "\t" +
                       cus.City + "\t" + cus.Province + "\t" + cus.Poastal_Code + "\t" + cus.Country + "\t" + cus.Phone + "\t" + cus.Email
                    + "\t" + BadDataFile[BadDataFile.Count - 1] + "\t" + status); */
            }
        }

        private void LogBadData(string status, Customer cus, string filepath)
        {
            //Writting to LOG file
            using (StreamWriter sw = System.IO.File.AppendText(LogFilepath))
            {
                List<String> BadDataFile = new List<String>(filepath.Split(@"Sample Data"));

                 sw.WriteLine("First_Name: {0} \t Last_Name: {1} \t Street_Number: {2} \t Street_Name: {3} \t City: {4} \t Province: {5} " +
                  "\t Poastal_Code: {6} \t Country: {7} \t Phone: {8} \t Email: {9} -->> \t Reason: {11} \t File Path : {10}",
                  cus.First_Name, cus.Last_Name, cus.Street_Number, cus.Street_Name, cus.City, cus.Province, cus.Poastal_Code, cus.Country, cus.Phone, cus.Email
                  , BadDataFile[BadDataFile.Count - 1], status); 

                     
                  // CSV type to analyze 
                   /*
                  sw.WriteLine(" {0} ,  {1} ,  {2} ,  {3} ,  {4} ,  {5} ,  {6} ,  {7} ,  {8} ,  {9} ,  {11} ,    {10}",
                      cus.First_Name, cus.Last_Name, cus.Street_Number, cus.Street_Name, cus.City, cus.Province, cus.Poastal_Code, cus.Country, cus.Phone, cus.Email
  ,                    BadDataFile[BadDataFile.Count - 1], status); */
                   
            }

        }

        public void WriteCSV(Customer cus, string filepath)
        {
            // Writing to main CSV file.
            List<String> strings = new List<String>(filepath.Split(@"\"));
            string Date = strings[strings.Count - 4] + @"/" + strings[strings.Count - 3] + @"/" + strings[strings.Count - 2];

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
            //Validating the records.
            string returnString =null,strRegex = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            Regex re = new Regex(strRegex, RegexOptions.IgnoreCase);
           
            if (cus.First_Name == "")
                returnString+= " Empty First Name ";
            if (cus.Street_Number == "")
                returnString += " Empty Street Number ";
            if (cus.Street_Name == "")
                returnString += " Empty Street Name ";
            if (cus.City == "")
                returnString += " Empty City ";
            if (cus.Province == "")
                returnString += " Empty Province ";
            if (cus.Poastal_Code == "")
                returnString += " Empty Postal code ";
            if (cus.Country == "")
                returnString += " Empty County ";
            if (cus.Phone == "")
                returnString += " Empty Phone ";
            if (cus.Email == "")
                returnString += " Empty Email. ";
            if(returnString!=null) 
                return returnString;
            else
            {
                    return "OK";
            }
        }
    }
}