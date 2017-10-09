using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _6
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 SumOfSquares = 0;
            Int64 SquareOfSums = 0;
            for (int i = 1; i < 101; i++)
            {
                SumOfSquares += i * i;
                SquareOfSums += i;
            }
            SquareOfSums = SquareOfSums * SquareOfSums;
            Console.WriteLine("SumOfSquares:\t\t{0}", SumOfSquares);
            Console.WriteLine("SquareOfSums:\t\t{0}", SquareOfSums);
            Console.WriteLine("Difference:\t\t{0}", SquareOfSums - SumOfSquares);
            Console.ReadLine();
        }
    }
}
