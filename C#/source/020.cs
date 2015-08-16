using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _020
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInt b = 10;
            b = ProjectEuler.ProjectEuler.factoral(100);
            Console.WriteLine(ProjectEuler.ProjectEuler.SumString(b.ToString()));
        }
    }
}
