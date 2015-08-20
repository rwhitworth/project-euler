using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _025
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInt b = 0;
            for (int i = 4780; i < 5000; i++)
            {
                b = ProjectEuler.ProjectEuler.FibIterative(i);
                if (b.ToString().Length == 1000)
                {
                    Console.WriteLine(i);
                    break;
                }
            }
        }
    }
}
