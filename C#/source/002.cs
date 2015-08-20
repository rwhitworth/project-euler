using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _2
{
    class Program
    {
        static void Main(string[] args)
        {
            int i = 0;
            BigInt result = 0;
            BigInt count = 0;
            for (i = 1; i < 35; i++)
            {
                result = ProjectEuler.ProjectEuler.Fib(i);
                if ((result % 2 == 0) && (result < 4000000))
                {
                    count += result;
                }
            }
            Console.WriteLine(count.ToString());
        }
    }
}
