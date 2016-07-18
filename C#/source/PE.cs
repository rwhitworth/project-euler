using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace PE
{
    public class Utilities
    {
        static public BigInteger sqrt(BigInteger n)
        {
            // calibrate 'guess' parameter based on input
            // multiple test runs with items 3 digits in length to hundreds of digits in length
            // shows 50 to be the correct magic number for all input values
            Int64 len = n.ToString().Length;
            BigInteger guess = 0;
            guess = 50 * (n.ToString().Length / 2);

            BigInteger temp = guess;
            BigInteger res = 0;
            
            // stop attempting to correct the guess by 5000 runs...
            // a 2000 digit number takes roughly 3300 iterations.
            // so 4999 should be enough.. right?
            for (int i = 0; i < 5000; i++)
            {
                res = sqrt(n, temp);
                if (res == temp)
                {
                    // Console.WriteLine("Break! {0}", i);
                    break;
                }
                temp = res;
                if (i >= 4999)
                {
                    throw new Exception("sqrt() ran almost 5000 iterations without calibrating... failing.");
                }
            }

            return temp;
        }
        static public BigInteger sqrt(BigInteger n, BigInteger guess)
        {
            // Newton's Method
            // https://en.wikipedia.org/wiki/Newton%27s_method#Square_root_of_a_number

            if (guess < 1)
                return n;

            BigInteger top = (guess * guess) - n;
            BigInteger bottom = 2 * guess;
            return guess - (top / bottom);
        }
        static public Int64 factorial(Int64 i)
        {
            Int64[] fact = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600, 6227020800 };

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
        static public BigInteger factorial(BigInteger i)
        {
            BigInteger[] fact = { 1, 1, 2, 6, 24, 120, 720, 5040, 40320, 362880, 3628800, 39916800, 479001600, 6227020800 };

            if (i > 20)
            {
                throw new Exception("factorial > 20, unsupported");
            }

            if (fact.Length > i)
            {
                return fact[Int32.Parse(i.ToString())];
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
        static public Boolean is_prime(BigInteger prime)
        {
            var sq = sqrt(prime) + 1;
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
            for (BigInteger i = 3; i < sq; i += 2)
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
        static public Int64 StringNameScore(String s)
        {
            // StringNameScore
            // Problem 22
            // StringNameScore("COLIN") == 53
            Int64 total = 0;
            s = s.ToUpper();
            for (int i = 0; i < s.Length; i++)
            {
                total += s[i] - 64; // A = 1 = 65 - 64, B = 2 = 66 - 64, etc 
            }
            return total;
        }
        public static Int64[] ProperDivisors(Int64 num)
        {
            // Problem 21
            // ProperDivisors(220) == 1, 2, 4, 5, 10, 11, 20, 22, 44, 55, 110
            // ProperDivisors(284) == 1, 2, 4, 71, 142
            System.Collections.ArrayList a = new ArrayList();

            a.Add(Int64.Parse("1"));
            for (Int64 k = 2; k < ((num / 2) + 1); k++)
            {
                if (num % k == 0)
                {
                    a.Add(k);
                }

            }

            Int64[] result = new Int64[a.Count];
            for (int i = 0; i < a.Count; i++)
            {
                result[i] = (Int64)a[i];
            }
            return result;
        }
        public static BigInteger[] ProperDivisors(BigInteger num)
        {
            // Problem 21
            // ProperDivisors(220) == 1, 2, 4, 5, 10, 11, 20, 22, 44, 55, 110
            // ProperDivisors(284) == 1, 2, 4, 71, 142
            System.Collections.ArrayList a = new ArrayList();

            a.Add(BigInteger.Parse("1"));
            for (BigInteger k = 2; k < ((num / 2) + 1); k++)
            {
                if (num % k == 0)
                {
                    a.Add(k);
                }

            }

            BigInteger[] result = new BigInteger[a.Count];
            for (int i = 0; i < a.Count; i++)
            {
                result[i] = (BigInteger)a[i];
            }
            return result;
        }
        public static bool sum_abundant(BigInteger num)
        {
            var a = ProperDivisors(num);
            BigInteger total = 0;
            foreach (var item in a)
            {
                total += item;
            }
            if (total > num)
                return true;
            return false;
        }
        public static bool PerfectNumber(BigInteger num)
        {
            var a = ProperDivisors(num);
            BigInteger total = 0;
            foreach (var item in a)
            {
                total += item;
            }
            if (total == num)
                return true;
            return false;
        }

        public static bool isNivenNumber(BigInteger num, String nivenString = "")
        {
            // Hashad Number, aka Niven Number
            // #387
            if (nivenString.Length == 0)
            {
                nivenString = num.ToString();
            }
            BigInteger counter = 0;
            if (num <= 0)
            {
                return false;
            }
            foreach (var item in nivenString)
            {
                counter += int.Parse(item.ToString());
            }
            if (num % counter == 0)
            {
                return true;
            }
            return false;
        }
        public static bool isRightTruncatableNivemNumber(BigInteger num)
        {
            // TODO: Is this function working?  Need more test cases to prove it works.
            // #387
            String nivenString = num.ToString();
            String new_string = nivenString.Substring(0, nivenString.Length - 1);
            BigInteger new_num = BigInteger.Parse(new_string);
            if (num <= 0)
            {
                return false;
            }
            while (new_string.Length > 1)
            {
                if (!isNivenNumber(BigInteger.Parse(new_string), new_string))
                {
                    return false;
                }
                new_string = new_string.Substring(0, new_string.Length - 1);
            }
            return true;
        }
        public static BigInteger sumDigits(String num)
        {
            BigInteger total = 0;
            for (int i = 0; i < num.Length; i++)
            {
                total += BigInteger.Parse(num[i].ToString());
            }
            return total;
        }
        public static BigInteger sumDigits(BigInteger num)
        {
            return sumDigits(num.ToString());
        }
    }
}
