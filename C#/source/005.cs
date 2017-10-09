using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _5
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 num = 0;
            bool div = false;

            for (num = 210; num < 10000000000; num+=2)
            {
                div = true;
                for (int i = 3; i <= 20; i++)
                {
                    if (num % i != 0) { div = false; break; }
                }
                if (div)
                {
                    Console.WriteLine(num);
                    Console.ReadLine();
                    break;
                }
                if (num % 1000 == 0) { Console.Write("."); }
            }
        }
    }
}
