using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _10
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 total = 2;
            for (int i = 3; i < 2000000; i+=2)
            {
                if (ProjectEuler.ProjectEuler.Primetest(i))
                {
                    total += i;
                }
            }

            Console.WriteLine(total);
        }
    }
}
