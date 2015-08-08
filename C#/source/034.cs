using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PE;

namespace _034
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 sum = 0;

            for (int i = 3; i <= 100000; i++)
            {
                String s = i.ToString();
                Int64 answer = 0;
                foreach (var item in s)
                {
                    answer += PE.Utilities.factorial(Int64.Parse(item.ToString()));
                }
                if (answer == i)
                {
                    sum += answer;
                }
            }

            Console.WriteLine("{0}", sum);
        }
    }
}
