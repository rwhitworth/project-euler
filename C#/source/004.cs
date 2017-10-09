using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _4
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 largest = 0;
            for (int x = 99; x < 1000; x++)
            {
                for (int y = 99; y < 1000; y++)
                {
                    if (ProjectEuler.ProjectEuler.isPalindrome(x * y))
                    {
                        if (x * y > largest)
                        {
                            largest = x * y;
                        }
                    }
                }
            }
            Console.WriteLine("Largest: {0}\nDone", largest);
            Console.ReadLine();
        }
    }
}
