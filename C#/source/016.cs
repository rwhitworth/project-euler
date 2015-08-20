using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _016
{
    class Program
    {
        static void Main(string[] args)
        {
            BigInt b = new BigInt();
            b = ProjectEuler.ProjectEuler.power(2, 1000);
            char[] array = b.ToString().ToCharArray();
            b = 0;
            for (int i = 0; i < array.Length; i++)
            {
                b += int.Parse(array[i].ToString());
            }
            Console.WriteLine(b.ToString());
        }
    }
}
