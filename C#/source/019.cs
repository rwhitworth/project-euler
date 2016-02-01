using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _019
{
    class Program
    {
        static void Main(string[] args)
        {
            int total_count = 0;
            System.DateTime dt = new DateTime(1901, 1, 1);
            while (dt.Year < 2001)
            {
                dt = dt.AddMonths(1);
                if (dt.Day == 1 && dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    total_count++;
                }
            }
            Console.WriteLine("{0}", total_count);
        }
    }
}
