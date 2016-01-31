using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace _018
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] array;
            int array_count = 0;
            int[] temp_array;
            string input;
            int input_count = 0;
            Random r = new Random();

            System.IO.StreamReader sr = new StreamReader("018.txt");
            array = new int[15][];

            while (sr.EndOfStream == false)
            {
                input = sr.ReadLine();
                var input_split = input.Split(' ');
                input_count = input_split.Length;
                temp_array = new int[input_count];
                for (int i = 0; i < input_count; i++)
                {
                    temp_array[i] = int.Parse(input_split[i]);
                }
                array[array_count] = temp_array;
                array_count++;
            }

            // Calculate from bottom up, the max that each decision could be for each number

            for (int j = 1; j < array_count; j++)
            {
                int t = array[array_count - j - 1].Length;
                for (int i = 0; i < array[array_count - j - 1].Length; i++)
                {
                    if (array[array_count - j][i] >= array[array_count - j][i + 1])
                    {
                        array[array_count - j - 1][i] = array[array_count - j][i] + array[array_count - j - 1][i];
                    }
                    else
                    {
                        array[array_count - j - 1][i] = array[array_count - j][i + 1] + array[array_count - j - 1][i];
                    }
                }
            }

            Console.WriteLine("{0}", array[0][0].ToString());
        }
    }
}
