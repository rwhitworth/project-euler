using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PE;
using ProjectEuler;

namespace _145
{
    class Program
    {
        static void Main(string[] args)
        {
            int total = 0;
            // int upper_limit = 1000;

            int upper_limit = 1000000000;

            DateTime start = DateTime.Now;

            for (int i = 1; i < upper_limit; i++)
            {
                if (i % 2 == 1)
                {
                    int rev = PE.Utilities.ReverseInt(i);
                    if ((i % 2) + (rev % 2) == 1)
                    {
                        if (PE.Utilities.isReversibleNumber(i, rev))
                            total+=2;
                    }
                }
            }

            DateTime stop = DateTime.Now;

            Console.WriteLine("{0}", total);
            Console.WriteLine("{0}", Convert.ToInt32((stop - start).TotalSeconds));
            Console.ReadLine();
        }
    }
}
