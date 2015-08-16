using System;
using ProjectEuler;

namespace _012
{
    class Program
    {
        static void Main(string[] args)
        {
            int bi = 0;
            for (int i = 1; i < 10000000; i++)
            {
                bi += i;
                if (ProjectEuler.ProjectEuler.NumberOfDivisors(bi) >= 500)
                { 
                    Console.WriteLine("{0}", bi);
                    break; 
                }
            }
        }
    }
}
