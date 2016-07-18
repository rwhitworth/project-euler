using System;
using System.Numerics;
using PE;

namespace _387
{
    class Program
    {
        static BigInteger total = BigInteger.Zero;
        // static BigInteger LIMIT = BigInteger.Parse("100000000000000");
        static BigInteger LIMIT = BigInteger.Parse("10000");

        static void Main(String[] args)
        {
            for (BigInteger i = 11; i <= LIMIT; i += 2)
            {
                string s = i.ToString();
                if (PE.Utilities.is_prime(i) && PE.Utilities.isRightTruncatableNivemNumber(i))
                {
                    if (int.Parse(s[s.Length - 1].ToString()) % 2 != 0)
                    {
                        BigInteger rtt = BigInteger.Parse(s.Substring(0, s.Length - 1));
                        if (PE.Utilities.is_prime(rtt / PE.Utilities.sumDigits(s.Substring(0, s.Length - 1))))
                            total += i;
                    }
                }
            }
            Console.WriteLine("{0}", total.ToString());
        }   
    }
}
