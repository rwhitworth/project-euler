using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PE
{
    public class Utilities
    {
        static public Int64 factorial(Int64 i)
        {
            Int64[] fact = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600, 6227020800 };
            //if (i == 0) { return 1; }
            //if (i == 1) { return 1; }
            //if (i == 2) { return 2; }
            //if (i == 3) { return 6; }
            //if (i == 4) { return 24; }

            if (i > 20)
            {
                throw new Exception("factorial > 20, unsupported");
            }

            if (fact.Length > i)
            {
                return fact[i];
            }

            return factorial(i - 1) * i;
        }
        static public Boolean is_prime(Int64 prime)
        {
            var sq = (Int64)Math.Ceiling(Math.Sqrt(prime)) + 1;
            for (Int64 i = 3; i < sq; i+=2)
            {
                if (prime % i == 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
