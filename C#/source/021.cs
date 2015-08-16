using System;
using ProjectEuler;

namespace _021
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Collections.ArrayList results = new System.Collections.ArrayList(1000);
            BigInt sum = 0;

            for (int i = 1; i < 10000; i++)
            {
                Int64[] a1 = ProjectEuler.ProjectEuler.ProperDivisors(i);
                Int64[] a2 = ProjectEuler.ProjectEuler.ProperDivisors(Int64.Parse(ProjectEuler.ProjectEuler.SumArrayOfNumbers(a1).ToString()));
                Int64 sum1 = 0;
                Int64 sum2 = 0;
                sum1 = (Int64)ProjectEuler.ProjectEuler.SumArrayOfNumbers(a1);
                sum2 = (Int64)ProjectEuler.ProjectEuler.SumArrayOfNumbers(a2);
                if ((sum2 == i) && (sum1 != sum2))
                {
                    sum += i;
                }
            }

            Console.WriteLine(sum);
        }
    }
}
