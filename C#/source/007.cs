using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _7
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 prime = 0;
            prime = ProjectEuler.ProjectEuler.PrimeNum(10001);
            Console.WriteLine("Prime 10001\t{0}", prime);
            Console.ReadLine();
        }
    }
}
