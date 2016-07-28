using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _028
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 total = 1;
            const Int64 num_squares = 1001; // 1001 == 1001 x 1001

            for (int i = 3; i <= num_squares; i+=2)
            {
                total += 4 * i * i - (i * 6) + 6;
            }

            Console.WriteLine("{0}", total);
        }
    }
}
