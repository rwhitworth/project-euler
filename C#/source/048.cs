using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;
using System.Numerics;

namespace _48
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInteger bigint = 0;
            for (int ii = 1; ii <= 1000; ii++)
            {
                bigint += BigInteger.Pow(ii, ii);
            }

            char[] a = ProjectEuler.ProjectEuler.Reverse(bigint.ToString()).ToCharArray();
            Console.WriteLine(ProjectEuler.ProjectEuler.Reverse(String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}", a[0], a[1], a[2], a[3], a[4], a[5], a[6], a[7], a[8], a[9])));
        }
    }
}
