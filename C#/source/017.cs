using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _017
{
    class Program
    {
        static String[] ones =      { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
        static String[] tens =      { "", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
        static String[] teens =     { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

        static String number2string(Int32 number)
        {
            // only works for numbers 1 - 1000
            String num = number.ToString();

            if ((number < 0) || (number > 1000))
                return "";

            if (number < 10)
                return ones[number];

            if (num.Length == 2)
            {
                if (num[1] == '0')
                    return tens[Int32.Parse(num[0].ToString())];
                else if (num[0] == '1')
                {
                    return teens[Int32.Parse(num[1].ToString())];
                }
                else
                {
                    return tens[Int32.Parse(num[0].ToString())] + ones[Int32.Parse(num[1].ToString())];
                }
            }

            if (num.Length == 3)
            {
                if (num[1] == '0' && num[2] == '0')
                {
                    return ones[Int32.Parse(num[0].ToString())] + "hundred";
                }

                if (num[2] == '0')
                    return ones[Int32.Parse(num[0].ToString())] + "hundred and " + tens[Int32.Parse(num[1].ToString())];
                else if (num[1] == '1')
                {
                    return ones[Int32.Parse(num[0].ToString())] + "hundred and " + teens[Int32.Parse(num[2].ToString())];
                }
                else
                {
                    return ones[Int32.Parse(num[0].ToString())] + "hundred and " + tens[Int32.Parse(num[1].ToString())] + ones[Int32.Parse(num[2].ToString())];
                }
            }

            if (num.Length == 4)
            {
                if (number == 1000)
                    return "one thousand";
                else
                    return ""; // because, really, this only supports 1 - 1000
            }

            return "";
        }

        static void Main(string[] args)
        {
            Int64 total_count = 0;

            for (int i = 1; i <= 1000; i++)
            {
                total_count += number2string(i).Replace(" ", "").Length;
            }

            Console.WriteLine("{0}", total_count);
        }
    }
}
