using System;
using System.IO;

namespace Assignment1
{
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
                Console.WriteLine("\n -------------------------------- \nPassed Path " + path);
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

                     Console.WriteLine("File:" + filepath);
                 } 
            }catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}
