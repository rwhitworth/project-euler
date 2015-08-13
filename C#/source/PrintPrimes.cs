using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace PrintPrimes
{
    class PrintPrimes
    {
        static void Main(string[] args)
        {
            Boolean first = true;
            Int32 counter = 0;
            const Int32 highestPrime = 1000000;

            print("//start of auto-generated list of prime numbers\nusing System;\nnamespace PE\n{\n\tpublic class PrimeData\n\t{\n");
            print("\t\tstatic public Int64[] PreGenPrimesList = { \n\t\t");

            for (Int32 i = 0; i < highestPrime; i++)
            {
                if (PE.Utilities.is_prime(i))
                {
                    if ((first == false && counter != 1) || (i == 3))
                    {
                        print(", ");
                    }
                    print("{0}", i);

                    if (counter == 5)
                    {
                        print(", \n\t\t");
                        counter = 0;
                    }

                    counter++;
                    first = false;
                }
            }
            print(" };\n\t}\n}\n//end of auto-generated list of prime numbers\n");

            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        static void print(String s)
        {
            // Console.Write(s);

            var outputfile = System.IO.File.AppendText("primes.cs");
            outputfile.Write(s);
            outputfile.Close();
        }
        static void print(String s, String v)
        {
            print(String.Format(s, v));
        }
        static void print(String s, Int32 v)
        {
            print(String.Format(s, v));
        }
        static void print(String s, Int64 v)
        {
            print(String.Format(s, v));
        }
    }
}
