using System;
using System.Numerics;
using PE;

namespace _387
{
    class Program
    {
        static BigInteger total = BigInteger.Zero;
        static Int64 LIMIT = Int64.Parse("100000000000000");
        # static BigInteger LIMIT = BigInteger.Parse("10000");

        static void Main(String[] args)
        {
            for (Int64 i = 11; i <= LIMIT; i += 2)
            {
                string s = i.ToString();
                if (PE.Utilities.is_prime(i) && PE.Utilities.isRightTruncatableNivemNumber(i))
                {
                    if (int.Parse(s[s.Length - 1].ToString()) % 2 != 0)
                    {
                        Int64 rtt = Int64.Parse(s.Substring(0, s.Length - 1));
			// sumDigits needs an Int64 capable version for this to work
                        if (PE.Utilities.is_prime(rtt / PE.Utilities.sumDigits(s.Substring(0, s.Length - 1))))
                            total += i;
                    }
                }
            }
            Console.WriteLine("{0}", total.ToString());
        }   
    }
}
