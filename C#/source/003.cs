using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PE;

namespace _003
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 prime = 600851475143;
            Int64 counter = 0;
            Int64 temp = 0;
            Int64 highest = 0;

            while (PE.PrimeData.PreGenPrimesList[counter] < prime)
            {
                temp = prime / PE.PrimeData.PreGenPrimesList[counter];
                if (temp * PE.PrimeData.PreGenPrimesList[counter] == prime)
                {
                    highest = PE.PrimeData.PreGenPrimesList[counter];
                }
                counter++;
                if (counter >= PE.PrimeData.PreGenPrimesList.Length)
                {
                    break;
                }
            }
            Console.WriteLine("{0}", highest);
        }
    }
}
