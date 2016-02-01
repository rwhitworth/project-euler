using System;
using PE;

namespace _022
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] names;
            Int64 total_score = 0;
            char[] split_character = new char[1];
            split_character[0] = ',';
            System.IO.StreamReader sr = new System.IO.StreamReader("022.txt");
            names = sr.ReadLine().Replace("\"", "").Split(split_character);
            sr.Close();

            Array.Sort(names);

            for (int i = 0; i < names.Length; i++)
            {
                total_score += (i + 1) * PE.Utilities.StringNameScore(names[i]);
            }

            Console.WriteLine("{0}", total_score);
        }
    }
}
