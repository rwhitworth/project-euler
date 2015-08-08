using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PE;

namespace _037
{
    class Program
    {
        static void Main(string[] args)
        {
            Int64 sum = 0;
            for (int i = 11; i < 1000000; i += 2)
            {
                if (PE.Utilities.is_prime(i))
                {
                    String str = i.ToString();
                    Int32 len = str.Length;
                    Boolean p = true;

                    for (int j = 0; j < len; j++)
                    {
                        String left = str.Substring(0, j+1);
                        String right = str.Substring(j, len - j);
                        if (!PE.Utilities.is_prime(Int64.Parse(left)))
                        {
                            p = false;
                            break;
                        }
                        if (!PE.Utilities.is_prime(Int64.Parse(right)))
                        {
                            p = false;
                            break;
                        }
                    }

                    if (p)
                    {
                        //Console.WriteLine("{0}", i);
                        sum += i;
                    }
                }
            }
            Console.WriteLine("{0}", sum);
        }
    }
}
