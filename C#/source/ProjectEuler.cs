using System;
using System.Collections;

namespace ProjectEuler
{
    public class ProjectEuler
    {
        public static BigInt Fib(BigInt num)
        {
            // Why the static table for some numbers?  Speed!
            if (num < 2) { return num; }
            if (num == 10) { return 55; }
            if (num == 20) { return 6765; }
            if (num == 21) { return 10946; }
            if (num == 40) { return 102334155; }
            if (num == 41) { return 165580141; }
            if (num == 50) { return 12586269025; }
            if (num == 70) { return 190392490709135; }
            if (num == 71) { return 308061521170129; }
            if (num == 80) { return 23416728348467685; }
            if (num == 90) { return 2880067194370816120; }
            if (num == 91) { return 4660046610375530309; }

            else
            {
                return Fib(num - 1) + Fib(num - 2);
            }
        }
        public static BigInt FibIterative(BigInt num)
        {
            BigInt a = 0;
            BigInt b = 1;
            BigInt c = 0;
            if (num < 3) { return 1; }
            for (BigInt i = 0; i < num; i++)
            {
                c = a;
                a = b;
                b = c + b;
            }
            return a;
        }

        public static BigInt sqrt(BigInt n)
        {
            // calibrate 'guess' parameter based on input
            // multiple test runs with items 3 digits in length to hundreds of digits in length
            // shows 50 to be the correct magic number for all input values
            Int64 len = n.ToString().Length;
            BigInt guess = 0;
            guess = 50 * (n.ToString().Length / 2);

            BigInt temp = guess;
            BigInt res = 0;

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
        public static BigInt sqrt(BigInt n, BigInt guess)
        {
            // Newton's Method
            // https://en.wikipedia.org/wiki/Newton%27s_method#Square_root_of_a_number
            if (guess <= 0)
            {
                return 0;
            }
            BigInt top = (guess * guess) - n;
            BigInt bottom = 2 * guess;
            return guess - (top / bottom);
        }

        public static Int64 PrimeNum(int num)
        {
            int prime_count = 1;
            Int64 starting_num = 3;
            if (num == 1) { return 2; }
            
            for (Int64 i = starting_num; i < 100000000; i+=2)
            {
                if (QPrimetest(i))
                {
                    if (Primetest(i))
                    {
                        prime_count++;
                        if (prime_count == num)
                        {
                            return i;
                        }
                    }
                }
            }
            return 0;
        }
        public static bool QPrimetest(Int64 num)
        {
            if (num % 2 == 0) { return false; }
            if (num <= 17) { return true; }
            if (num % 3 == 0) { return false; }
            if (num % 5 == 0) { return false; }
            if (num % 7 == 0) { return false; }
            if (num % 11 == 0) { return false; }
            if (num % 13 == 0) { return false; }
            if (num % 17 == 0) { return false; }
            return true;
        }
        public static bool Primetest(Int64 num)
        {
            if (num == 2) { return true; }
            if (num == 3) { return true; }
            Double sqrt = System.Math.Sqrt(num);
            sqrt = System.Math.Ceiling(sqrt);
            Int64 sqrt64 = Int64.Parse(sqrt.ToString());
            for (Int64 i = 3; i <= sqrt64; i+=2)
            {
                if (num % i == 0) { return false; }
            }
            return true;
        }
        public static bool isPrime(BigInt num)
        {
            if (num == 2) { return true; }
            if (num == 3) { return true; }
            Int64 sq_root = Int64.Parse(sqrt(num).ToString()) + 1;
            for (Int64 i = 3; i <= sq_root; i += 2)
            {
                if (num % i == 0) { return false; }
            }
            return true;
        }
        public static Int64 GCD(Int64 a, Int64 b)
        {
            Int64 c = 0;
            if (a < b) { c = a; a = b; b = c; c = 0; }
            c = a % b;
            if (c == 0) { return b; }
            a = b;
            b = c;
            return GCD(a, b);
        }
        public static bool isPalindrome(Int64 num)
        {
            String pal = num.ToString();
            String pal1 = Reverse(pal);
            if (pal == pal1)
            {
                return true;
            }
            return false;
        }
        public static String Reverse(String str)
        {
            char[] array = str.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
        public static BigInt power(BigInt basenum, Int64 pow)
        {
            BigInt total = basenum;
            for (Int64 i = 1; i < pow; i++)
            {
                total = total * basenum;
            }
            return total;
        }
        public static BigInt factoral(BigInt fac)
        {
            // Problem 20
            // Note: This is NOT a factorial done correctly...
            BigInt result = 1;
            for (BigInt i = 1; i < fac; i++)
            {
                result *= i;
            }
            return result;
        }
        public static BigInt factorial(BigInt fac)
        {
            // Problem 34 - Factorial done correctly
            BigInt result = 1;
            for (BigInt i = 1; i <= fac; i++)
            {
                result *= i;
            }
            return result;
        }
        public static BigInt FactorialSumOfDigits(BigInt fac)
        {
            String s = fac.ToString();
            BigInt sum = 0;
            for (int i = 0; i < s.Length; i++)
            {
                String ss = s.Substring(i, 1);
                sum += factorial(BigInt.Parse(s.Substring(i, 1)));
                // s.Substring((int)i, 1)
            }
            return sum;
        }
        public static BigInt SumString(String str)
        {
            char[] array = str.ToCharArray();
            BigInt b = 0;
            for (int i = 0; i < array.Length; i++)
            {
                b += int.Parse(array[i].ToString());
            }
            return b;
        }
        public static int NumberOfDivisors(int number)
        {
            int nod = 0;
            int sqrt = (int)Math.Sqrt(number);

            for (int i = 1; i <= sqrt; i++)
            {
                if (number % i == 0)
                {
                    nod += 2;
                }
            }
            //Correction if the number is a perfect square
            if (sqrt * sqrt == number)
            {
                nod--;
            }

            return nod;
        }
        public static Int64[] ProperDivisors(Int64 num)
        {
            // Problem 21
            // ProperDivisors(220) == 1, 2, 4, 5, 10, 11, 20, 22, 44, 55, 110
            // ProperDivisors(284) == 1, 2, 4, 71, 142
            System.Collections.ArrayList a = new ArrayList();

            a.Add(Int64.Parse("1"));
            for (Int64 k = 2; k < ((num / 2)+1); k++)
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
        public static BigInt SumArrayOfNumbers(Int64[] a)
        {
            BigInt result = 0;
            for (Int64 i = 0; i < a.Length; i++)
            {
                result += a[i];
            }
            return result;
        }
        public static bool isNivenNumber(BigInt num)
        {
            // Hashad Number, aka Niven Number
            String nivenString = num.ToString();
            BigInt counter = 0;
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
        public static bool isRightTruncatableNivemNumber(BigInt num)
        {
            // TODO: Is this function working?  Need more test cases to prove it works.
            // #387
            String nivenString = num.ToString();
            String new_string = nivenString.Substring(0, nivenString.Length - 1);
            BigInt new_num = BigInt.Parse(new_string);
            if (num <= 0)
            {
                return false;
            }
            while(new_string.Length > 1)
            {
                if (!isNivenNumber(BigInt.Parse(new_string)))
                {
                    return false;
                }
                new_string = new_string.Substring(0, new_string.Length - 1);
            }
            return true;
        }
        public static bool isStrongNivenNumber(BigInt num)
        {
            // TODO: Is this function working?  Need more test cases to prove it works.
            // #387
            BigInt nivenDivisor = 0;
            String nivenString = num.ToString();
            BigInt counter = 0;
            if (num <= 0 || !isNivenNumber(num))
            {
                return false;
            }
            foreach (var item in nivenString)
            {
                counter += int.Parse(item.ToString());
            }
            nivenDivisor = num / counter;
            if (isPrime(nivenDivisor))
            {
                return true;
            }
            return false;
        }
    }
}
