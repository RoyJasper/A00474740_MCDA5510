using System;
namespace Assignment1
{
    public class EmptyClass
    {

    //    public static void Main(String[] args)
   //     {
    //        EmptyClass ec = new EmptyClass();
    //        ec.foo();
    //    }


        public void foo()
        {


            //  double[] obj1 = new double[10];
            // double[] obj1 = { 1, 2 };

            /*           double max = 0;


                       foreach (double x in obj1)
                       {
                           if (max < x)
                           {
                               max = x;
                           }
                       }
                       Console.WriteLine("Max is:" + max);
                       

            //double[] obj1 = new double[10];
            double max = Double.MinValue;

            foreach (double x in obj1)
            {
                if (x > max)
                {
                    max = x;
                }
            }
            Console.WriteLine("Max is:" + max);
        }
        

            //double[] obj1 = new double[10];
            double sum = 0;

            foreach (double x in obj1)
            {
                double test = x * x * x;
                sum += test;
            }
            Console.WriteLine("sum is:" + sum);
        }

    */

            double[] obj1 = { 1, 2 };
            double[] obj2 = { 1, 2 };

            double result = 0;
            double num = 0;
            double firstTermSq = 0;
            double secondTermSq = 0;

            for (int i = 0; i < obj1.Length; i++)
            {

                num = obj1[i] * obj2[i];
                firstTermSq += obj1[i] * obj1[i];
                secondTermSq += obj2[i] * obj2[i];

            }

            double den = Math.Sqrt(firstTermSq * secondTermSq);

            result = num / den;

            Console.WriteLine("sum is:" + result);

        }

    }

}
