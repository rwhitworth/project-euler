using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _13
{
    class Program
    {
        static BigInt[] a = new BigInt[100];

        static void PopulateArrayFromFile()
        {
            System.IO.StreamReader sr = new System.IO.StreamReader("13.txt");
            int counter = 0;
            while (!sr.EndOfStream)
            {
                a[counter] = BigInt.Parse(sr.ReadLine());
                counter++;
            }
        }

        static void Main(string[] args)
        {
            BigInt sum = 0;
            PopulateArrayFromFile();

            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i];
            }

            Console.WriteLine(sum.ToString().Substring(0, 10));
        }

    }
}
