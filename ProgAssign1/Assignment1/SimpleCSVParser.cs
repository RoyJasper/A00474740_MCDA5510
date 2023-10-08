using System;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Assignment1
{
    public class SimpleCSVParser
    {


 //       public static void Main(String[] args)
 //       {
 //           SimpleCSVParser parser = new SimpleCSVParser();
 //           parser.parse(@"/Users/dpenny/Projects/Assignment1/Assignment1/sampleFile.csv");
 //       }


        public void parse(String fileName)
        {
            try { 
            using (TextFieldParser parser = new TextFieldParser(fileName))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                    while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();
                    foreach (string field in fields)
                    {
                        Console.WriteLine(field);
                    }
                }
            }
        
        }catch(IOException ioe){
                Console.WriteLine(ioe.StackTrace);
         }

    }


    }
}
