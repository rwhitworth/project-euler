using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectEuler;

namespace _8
{
    class Program
    {
        static void Main(string[] args)
        {
            String str =    "73167176531330624919225119674426574742355349194934" +
                            "96983520312774506326239578318016984801869478851843" +
                            "85861560789112949495459501737958331952853208805511" +
                            "12540698747158523863050715693290963295227443043557" +
                            "66896648950445244523161731856403098711121722383113" +
                            "62229893423380308135336276614282806444486645238749" +
                            "30358907296290491560440772390713810515859307960866" +
                            "70172427121883998797908792274921901699720888093776" +
                            "65727333001053367881220235421809751254540594752243" +
                            "52584907711670556013604839586446706324415722155397" +
                            "53697817977846174064955149290862569321978468622482" +
                            "83972241375657056057490261407972968652414535100474" +
                            "82166370484403199890008895243450658541227588666881" +
                            "16427171479924442928230863465674813919123162824586" +
                            "17866458359124566529476545682848912883142607690042" +
                            "24219022671055626321111109370544217506941658960408" +
                            "07198403850962455444362981230987879927244284909188" +
                            "84580156166097919133875499200524063689912560717606" +
                            "05886116467109405077541002256983155200055935729725" +
                            "71636269561882670428252483600823257530420752963450";

            int largest = 0;
            string tempstr = "";

            for (int i = 0; i < str.Length-5; i++)
            {
                tempstr = String.Format("{0}{1}{2}{3}{4}", str[i] , str[i+1] , str[i+2] , str[i+3] , str[i+4]);
                if (int.Parse(tempstr) > largest)
                {
                    if (!tempstr.Contains("0"))
                    {
                        largest = int.Parse(tempstr);
                        //Console.WriteLine(int.Parse(str[i].ToString()) * int.Parse(str[i + 1].ToString()) * int.Parse(str[i + 2].ToString()) * int.Parse(str[i + 3].ToString()) * int.Parse(str[i + 4].ToString()));
                    }
                }
            }
            tempstr = largest.ToString();
            int ii = 0;
            Console.WriteLine(int.Parse(tempstr[ii].ToString()) * int.Parse(tempstr[ii + 1].ToString()) * int.Parse(tempstr[ii + 2].ToString()) * int.Parse(tempstr[ii + 3].ToString()) * int.Parse(tempstr[ii + 4].ToString()));
            Console.ReadLine();
        }
    }
}
