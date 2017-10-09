using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _97
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 bi = 1;
            //Console.WriteLine(power(2, 1000));

            for (BigInt i = 1; i <= 7830457; i++)
            //for (BigInt i = 1; i <= 3; i++)
            {
                bi *= 2;
                if (i % 2 == 0)
                {
                    while (bi > 1000000000000)
                    {
                        bi -= 1000000000000;
                    }
                }
            }
            Console.WriteLine(bi);
            bi *= 28433;
            Console.WriteLine(bi);
            bi += 1;
            Console.WriteLine(bi);
            Console.WriteLine(bi.ToString().Substring(0, 10));
            Console.WriteLine("---------------");

            bi = power(2, 7830457);
            Console.WriteLine(bi);
            bi *= 28433;
            Console.WriteLine(bi);
            bi += 1;
            Console.WriteLine(bi);
            Console.WriteLine("---");
            Console.WriteLine(bi.ToString().Substring(0, 10));
            Console.ReadLine();
            
        }

        public static Int64 power(Int64 basenum, Int64 pow)
        {
            Int64 total = basenum;
            for (Int64 i = 1; i < pow; i++)
            {
                //total = total * basenum;
                total *= basenum;
                if (total.ToString().Length > 10)
                {
                    String temp = total.ToString();
                    temp = temp.Substring(temp.Length - 10, 10);

                    total = Int64.Parse(temp);
                }
            }
            return total;
        }
    }
}
