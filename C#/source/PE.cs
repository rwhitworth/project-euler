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
            if (prime == 2)
            {
                // yes, 2 is a prime number.
                return true;
            }
            if (prime % 2 == 0)
            {
                // all even numbers, other than 2, are not prime
                return false;
            }
            if (prime < 2)
            {
                // all numbers less than 2 are not prime.  Including 1.
                return false;
            }
            if ((prime % 5 == 0) && (prime != 5))
            {
                // anything devided by 5 with a remainder of zero is not prime.
                // at one point had a problem with returning true on numbers like 25.
                // probably a bug in the way I'm handling the sqrt/ceil (?)
                return false;
            }
            for (Int64 i = 3; i < sq; i+=2)
            {
                if (prime % i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        static public String StringReverse(String s)
        {
            char[] a = s.ToCharArray();
            Array.Reverse(a);
            return new String(a);
        }
    }
}
