using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _014
{
    class Program
    {
        static Int64 FindTerms(Int64 startnum)
        {
            Int64 tempnum = startnum;
            Int64 counter = 1;
            while (tempnum > 1)
            {
                if (tempnum % 2 == 0)
                {
                    tempnum = tempnum / 2;
                }
                else
                {
                    tempnum = (tempnum * 3) + 1;
                }
                counter++;
            }
            return counter;
        }

        static void Main(string[] args)
        {
            // n -> n/2 (n is even)
            // n -> 3n + 1 (n is odd)
            Int64 largest = 1;
            Int64 largestterms = 1;
            Int64 tempterms = 1;
            for (Int64 i = 2; i < 1000000; i++)
            {
                tempterms = FindTerms(i);
                if (tempterms > largestterms)
                {
                    largestterms = tempterms;
                    largest = i;
                }
            }
            Console.WriteLine(largest);
        }
    }
}
