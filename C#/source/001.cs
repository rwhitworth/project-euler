using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 count = 0;
            for (int i = 0; i < 1000; i++)
            {

                if (i % 3 == 0)
                {
                    count += i;
                }
                else
                if (i % 5 == 0)
                {
                    count += i;
                }
            }
            Console.WriteLine(count);
        }
    }
}
