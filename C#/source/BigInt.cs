// BigInt.cs (v 1.03)
// stephen swensen
// created march 2009
// updated may 2009
// c# 3.0, .net 3.5
// License: The Code Project Open License (CPOL) 1.02

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ProjectEuler
{
    /// <summary>
    /// BigInt is a general-purpose unbounded integer implementation consistent with C# and .NET numeric type conventions
    /// </summary>
    [System.Serializable] //add [NonSerialized] attribute to any (possible future) fields other than digits and isneg
    [System.Runtime.InteropServices.ComVisible(false)]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)] //since we don't care about COM interop
    public struct BigInt : IComparable<BigInt>, IEquatable<BigInt>, IConvertible, IXmlSerializable
    {
        #region Instance Fields

        private LinkedList<byte> digits; //is null if and only if bigint is zero
        private bool isneg; //value irrelevant if digits is null

        #endregion Instance Fields

        #region Constructors

        //as is more natural for ValueTypes, we use explicit and and implicit conversion operators 
        //(and Parse for strings) in-lue of equivalent contructors (and to spare redundancy)

        /// <summary>
        /// shallow copy
        /// </summary>
        private BigInt(LinkedList<byte> digits, bool isneg)
        {
            this.digits = digits;
            this.isneg = isneg;
        }
       
        #endregion Constructors

        #region Static Fields

        //being common and useful privately, we deduce the same for the public

        public static readonly BigInt Zero = new BigInt(); //data null, isneg false
        public static readonly BigInt One = BigInt.Parse("1");
        public static readonly BigInt NegativeOne = BigInt.Parse("-1");
        public static readonly BigInt Two = BigInt.Parse("2");

        #endregion Static Fields

        #region Instance Properties

        /// <summary>
        /// Traverse the unsigned digits of this BigInt, left to right.
        /// </summary>
        public IEnumerable<byte> DigitsLeftToRight //DigitsL2R so tempting!
        {
            get
            {
                if (digits == null)
                    yield return (byte)0;
                else
                {
                    for (LinkedListNode<byte> cur = digits.First; cur != null; cur = cur.Next)
                        yield return cur.Value;
                }
            }
        }

        /// <summary>
        /// Traverse the unsigned digits of this BigInt, right to left.
        /// </summary>
        public IEnumerable<byte> DigitsRightToLeft //DigitsR2L
        {
            get
            {
                if (digits == null)
                    yield return (byte)0;
                else
                {
                    for (LinkedListNode<byte> cur = digits.Last; cur != null; cur = cur.Previous)
                        yield return cur.Value;
                }
            }
        }

        /// <summary>
        /// Iterate the proper divisors of this BigInt.  Zero yeilds no divisors.  Negatives yeild negative divisors.
        /// </summary>
        public IEnumerable<BigInt> ProperDivisors //naive implementation
        {
            get
            {
                if (this > BigInt.Zero)
                {
                    BigInt half = BigInt.DivideCeiling(this, BigInt.Two);
                    BigInt cur = BigInt.One;
                    do
                    {
                        if (this % cur == BigInt.Zero)
                            yield return cur;

                        cur++;
                    } while (cur <= half);
                }
                else if (this < BigInt.Zero)
                {
                    BigInt half = BigInt.DivideFloor(this, BigInt.Two);
                    BigInt cur = BigInt.NegativeOne;
                    do
                    {
                        if (this % cur == BigInt.Zero)
                            yield return cur;

                        cur--;
                    } while (cur >= half);
                }
                //else //is zero - NOT PRACTICAL
                //{
                //    BigInt cur = BigInt.Zero;
                //    while (true)
                //        yield return ++cur;
                //}
            }
        }

        /// <summary>
        /// Iterate the divisors of this BigInt.  Zero yeilds no divisors.  Negatives yeild negative divisors.
        /// </summary>
        public IEnumerable<BigInt> Divisors //naive implementation
        {
            get
            {
                foreach (BigInt cur in ProperDivisors)
                    yield return cur;

                if(this.digits != null) //not zero
                    yield return this;
            }
        }
        #endregion Instance Properties

        #region Equals, GetHashCode, and ToString Overrides

        /// <summary>
        /// Determines whether this BigInt is equivalent to the given object
        /// </summary>
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;

            return (obj is BigInt) ? Equals((BigInt)obj) : false;
        }

        /// <summary>
        /// Returns the hashcode for this BigInt
        /// </summary>
        public override int GetHashCode()
        {
            if (digits == null)
                return 0;


            int code, count;
            code = count = digits.Count;
            unchecked
            {
                foreach (byte b in digits)
                {
                    code = code * (b == 0 ? count : b);
                    if (code == 0)
                        code = count;
                }
            }

            return (isneg ? -1 : 1) * code;
        }

        /// <summary>
        /// String representation of this BigInt
        /// </summary>
        public override string ToString()
        {
            if (digits == null)
                return "0";

            StringBuilder sb = new StringBuilder();

            if (isneg)
                sb.Append("-");

            foreach (byte digit in this.digits)
                sb.Append(digit.ToString());

            return sb.ToString();
        }

        #endregion Equals, GetHashCode, and ToString Overrides

        #region Comparison and Equality

        /// <summary>
        /// Compare two BigInts
        /// </summary>
        public static int Compare(BigInt lhs, BigInt rhs)
        {
            return lhs.CompareTo(rhs);
        }

        /// <summary>
        /// Compare this BigInt to another
        /// </summary>
        public int CompareTo(BigInt rhs) //was less than
        {
            //permutations of this and rhs being zero
            if (this.digits == null && rhs.digits == null)
                return 0; //they are equal (both 0)
            else if (this.digits == null && rhs.digits != null)
                return rhs.isneg ? 1 : -1;
            else if (this.digits != null && rhs.digits == null)
                return this.isneg ? -1 : 1;

            //this is negative and rhs is positive
            if (this.isneg && !rhs.isneg)
                return -1;
            else if (!this.isneg && rhs.isneg)
                return 1;
            else if (this.isneg && rhs.isneg) //this and rhs are negative
            {
                if (this.digits.Count > rhs.digits.Count)
                    return -1;
                else if (this.digits.Count < rhs.digits.Count)
                    return 1;
                else //count == count
                    return CompareFirstDiffDigit(rhs.digits, this.digits);
            }
            else //!this.isneg && !rhs.isneg (this and rhs are positive
            {
                if (this.digits.Count > rhs.digits.Count)
                    return 1;
                else if (this.digits.Count < rhs.digits.Count)
                    return -1;
                else //count == count
                    return CompareFirstDiffDigit(this.digits, rhs.digits);
            }
        }

        public static bool operator <(BigInt lhs, BigInt rhs)
        {
            return lhs.CompareTo(rhs) == -1;
        }

        public static bool operator <=(BigInt lhs, BigInt rhs)
        {
            int result = lhs.CompareTo(rhs);
            return (result == -1 || result == 0);
        }

        public static bool operator >(BigInt lhs, BigInt rhs)
        {
            return lhs.CompareTo(rhs) == 1;
        }

        public static bool operator >=(BigInt lhs, BigInt rhs)
        {
            int result = lhs.CompareTo(rhs);
            return (result == 1 || result == 0);
        }

        public static bool operator ==(BigInt lhs, BigInt rhs)
        {
            return lhs.CompareTo(rhs) == 0;
        }

        public static bool operator !=(BigInt lhs, BigInt rhs)
        {
            return !(lhs == rhs);
        }

        /// <summary>
        /// arguments may not be null
        /// </summary>
        private static bool IsLessThan(LinkedList<byte> lhs_digits, LinkedList<byte> rhs_digits)
        {
            return (lhs_digits.Count == rhs_digits.Count && (CompareFirstDiffDigit(lhs_digits, rhs_digits) == -1)) || (lhs_digits.Count < rhs_digits.Count);
        }

        /// <summary>
        /// compares first most signifigant digits
        /// assumes lhs and rhs are both non-null, with equal counts greater than 0
        /// </summary>
        private static int CompareFirstDiffDigit(LinkedList<byte> lhs, LinkedList<byte> rhs)
        {
            LinkedListNode<byte> cur_lhs = lhs.First;
            LinkedListNode<byte> cur_rhs = rhs.First;
            for (int i = 0; i < lhs.Count; i++, cur_lhs = cur_lhs.Next, cur_rhs = cur_rhs.Next)
                if (cur_lhs.Value != cur_rhs.Value)
                    return cur_lhs.Value < cur_rhs.Value ? -1 : 1;

            return 0;//all digits equal
        }

        /// <summary>
        /// Determines whether this BigInt is equivalent to the given BigInt
        /// </summary>
        public bool Equals(BigInt obj)
        {
            return this == obj;
        }

        #endregion Comparison and Equality

        #region Arithmetic Algorithms and Operators

        public static BigInt operator +(BigInt lhs, BigInt rhs)
        {
            return Add(lhs, rhs);
        }

        /// <summary>
        /// Add two BigInts
        /// </summary>
        public static BigInt Add(BigInt lhs, BigInt rhs)
        {
            if (lhs.digits == null && rhs.digits == null) //0 + 0 = 0
                return BigInt.Zero;
            else if (lhs.digits == null && rhs.digits != null) //0 + b = b
                return rhs;
            else if (lhs.digits != null && rhs.digits == null) //a + b = 0
                return lhs;
            else
                return AddPhase2(lhs.digits, lhs.isneg, rhs.digits, rhs.isneg);
        }
        
        /// <summary>
        /// operands are non-zero
        /// 
        /// There are 4 general cases:
        /// 1) c = a + b
        /// 2) c = -a + -b = -(a + b)
        /// 3) c = a + -b = a - b
        /// 4) c = -a + b = b - a
        /// special: c = a + b = b + a
        /// </summary>
        private static BigInt AddPhase2(LinkedList<byte> lhs_digits, bool lhs_isneg, LinkedList<byte> rhs_digits, bool rhs_isneg)
        {
            //use algerbra to make sure a and b are positive
            if (lhs_isneg && rhs_isneg) // (-a) + (-b) = -(a + b)
                return new BigInt(AddPhase3(lhs_digits, rhs_digits), true);
            else if (!lhs_isneg && rhs_isneg) // a + (-b) = a - b
                return SubtractPhase3(lhs_digits, rhs_digits);
            else if (lhs_isneg && !rhs_isneg) // (-a) + b = b - a
                return SubtractPhase3(rhs_digits, lhs_digits);
            else  //now a and b are positive
                return new BigInt(AddPhase3(lhs_digits, rhs_digits), false);
        }

        /// <summary>
        /// operands are non-zero and positive
        /// </summary>
        private static LinkedList<byte> AddPhase3(LinkedList<byte> lhs_digits, LinkedList<byte> rhs_digits)
        {
            //make sure a is greater than b
            if (IsLessThan(lhs_digits, rhs_digits))
                return AddPhase4(rhs_digits, lhs_digits); // a + b = b + a
            else //now a and b are positive and a is greater than b
                return AddPhase4(lhs_digits, rhs_digits);
        }

        /// <summary>
        /// operands are non-zero, positive, and lhs >= rhs
        /// </summary>
        private static LinkedList<byte> AddPhase4(LinkedList<byte> lhs_digits, LinkedList<byte> rhs_digits)
        {
            LinkedList<byte> sum_digits = new LinkedList<byte>();
            LinkedListNode<byte> cur_lhs = lhs_digits.Last;
            LinkedListNode<byte> cur_rhs = rhs_digits.Last;
            int carry = 0;
            int rhs_topindex = rhs_digits.Count - 1;
            for(int i = 0; ;)
            {
                byte val_lhs = cur_lhs.Value;
                byte val_rhs = i <= rhs_topindex ? cur_rhs.Value : (byte)0;
                int sum = (val_lhs + val_rhs + carry);
                carry = 0;
                if (sum > 9)
                {
                    sum -= 10;
                    carry = 1;
                }
                sum_digits.AddFirst((byte)sum);

                if (cur_lhs == lhs_digits.First)
                    break;

                cur_lhs = cur_lhs.Previous;
                if(++i <= rhs_topindex)
                    cur_rhs = cur_rhs.Previous;
            }
            if (carry == 1)
                sum_digits.AddFirst((byte)carry);

            return sum_digits;
        }

        /// <summary>
        /// operands are non-zero and both positive or both negative.  mutates lhs_digits.  because of extra checks required to handeld both a less than b
        /// and b less than a cases, slightly slower than AddPhase4, but when doing accumlative addition (such as Multiply), performance
        /// is much increased by sparing repeated large memory allocation for tempory states.
        /// </summary>
        private static void AddTo(LinkedList<byte> lhs_digits, LinkedList<byte> rhs_digits)
        {
            LinkedListNode<byte> cur_lhs = lhs_digits.Last;
            LinkedListNode<byte> cur_rhs = rhs_digits.Last;
            int carry = 0;
            int rhs_topindex = rhs_digits.Count - 1;
            int lhs_topindex = lhs_digits.Count - 1;
            int max_index = Math.Max(rhs_topindex, lhs_topindex);
            for (int i = 0; ; )
            {
                byte val_lhs = i <= lhs_topindex ? cur_lhs.Value : (byte)0;
                byte val_rhs = i <= rhs_topindex ? cur_rhs.Value : (byte)0;
                int sum = (val_lhs + val_rhs + carry);
                carry = 0;
                if (sum > 9)
                {
                    sum -= 10;
                    carry = 1;
                }

                if (i > lhs_topindex)
                {
                    lhs_digits.AddFirst((byte)sum);
                    if (i == max_index)
                        break;

                    cur_lhs = lhs_digits.First;
                }
                else
                {
                    cur_lhs.Value = (byte)sum;
                    if (i == max_index)
                        break;

                    cur_lhs = cur_lhs.Previous;
                }

                if (++i <= rhs_topindex)
                    cur_rhs = cur_rhs.Previous;
            }

            if (carry == 1)
                lhs_digits.AddFirst((byte)carry);
        }

        public static BigInt operator -(BigInt lhs, BigInt rhs)
        {
            return Subtract(lhs, rhs);
        }

        /// <summary>
        /// Subtract the right-hand-side from the left-hand-side
        /// </summary>
        public static BigInt Subtract(BigInt lhs, BigInt rhs)
        {
            if (lhs.digits == null && rhs.digits == null) //0 - 0 = 0,  (may consider value of implementing a - a = 0 [SEE Subtract3])
                return BigInt.Zero;
            else if (lhs.digits == null && rhs.digits != null) //0 - (a) = -a, 0 - (-a) = a
                return new BigInt(rhs.digits, !rhs.isneg);
            else if (lhs.digits != null && rhs.digits == null) //a - 0 = a
                return lhs;
            else
                return SubtractPhase2(lhs.digits, lhs.isneg, rhs.digits, rhs.isneg);
        }

        /// <summary>
        /// a and b are non-zero
        /// 
        /// There are 4 general cases:
        /// 1) c = a - b
        /// 2) c = a - (-b) = a + b
        /// 3) c = (-a) - b = (-a) + (-b)
        /// 4) c = (-a) - (-b) = b - a
        /// special: c = a - b = -(b - a)
        /// </summary>
        private static BigInt SubtractPhase2(LinkedList<byte> lhs_digits, bool lhs_isneg, LinkedList<byte> rhs_digits, bool rhs_isneg)
        {
            //use algerbra to make sure a and b are positive
            if (!lhs_isneg && rhs_isneg) // a - (-b) = a + b
                return new BigInt(AddPhase3(lhs_digits, rhs_digits), false);
            else if (lhs_isneg && !rhs_isneg) // (-a) - b = (-a) + (-b) = -(a + b)
                return new BigInt(AddPhase3(lhs_digits, rhs_digits), true); //also: return (-a + b); works
            else if (lhs_isneg && rhs_isneg) // (-a) - (-b) = b - a
                return SubtractPhase3(rhs_digits, lhs_digits);
            else //a and b are positive.
                return SubtractPhase3(lhs_digits, rhs_digits);
        }

        /// <summary>
        /// a and b are non-zero and positive (returns zero if equal)
        /// </summary>
        private static BigInt SubtractPhase3(LinkedList<byte> lhs_digits, LinkedList<byte> rhs_digits)
        {
            //we do a special check for a == b which means a - b = 0 for performance increases
            if (lhs_digits.Count == rhs_digits.Count) //a is either equal to or less than b
            {
                int diff = CompareFirstDiffDigit(lhs_digits, rhs_digits);
                if (diff == 0) //they are equal
                    return BigInt.Zero;
                else if (diff == -1) //a < b
                    goto neg_b_minus_a;
                else //a > b
                    goto pos_a_minus_b;
            }
            else if (lhs_digits.Count < rhs_digits.Count)//a < b
                goto neg_b_minus_a;
            else //a > b
                goto pos_a_minus_b;

            //the following are the only two uses of SubtractPhase4, so we are ok so far in placing equality condition here, in Phase3
            neg_b_minus_a: return new BigInt(SubtractPhase4(rhs_digits, lhs_digits), true);  //a - b = -(b - a), when a < b
            pos_a_minus_b: return new BigInt(SubtractPhase4(lhs_digits, rhs_digits), false);
        }

        /// <summary>
        /// a and b are non-zero, positive, a > b, a does not equl b (use phase 3)
        /// </summary>
        private static LinkedList<byte> SubtractPhase4(LinkedList<byte> lhs_digits, LinkedList<byte> rhs_digits)
        {
            //use a standard borrowing subtraction algorithm
            LinkedList<byte> diff_digits = new LinkedList<byte>();
            LinkedListNode<byte> cur_lhs = lhs_digits.Last;
            LinkedListNode<byte> cur_rhs = rhs_digits.Last;
            int borrow = 0;
            int rhs_topindex = rhs_digits.Count - 1;
            for (int i = 0; ; )
            {
                byte val_lhs = cur_lhs.Value;
                byte val_rhs = i <= rhs_topindex ? cur_rhs.Value : (byte)0;
                int diff = (val_lhs - val_rhs - borrow);
                borrow = 0;
                if (diff < 0)
                {
                    diff += 10;
                    borrow = 1;
                }
                diff_digits.AddFirst((byte)diff);

                if (cur_lhs == lhs_digits.First)
                    break;

                cur_lhs = cur_lhs.Previous;
                if (++i <= rhs_topindex)
                    cur_rhs = cur_rhs.Previous;
            }

            while (diff_digits.First.Value == (byte)0)
                diff_digits.RemoveFirst();

            return diff_digits;
        }

        public static BigInt operator *(BigInt lhs, BigInt rhs)
        {
            return Multiply(lhs, rhs);
        }

        private static LinkedList<byte> GenMultPart(int leading_mult, int zero_count)
        {
            LinkedList<byte> llist = new LinkedList<byte>();
            
            string leading_mult_str = leading_mult.ToString();
            for(int i = 0; i < leading_mult_str.Length; i++)
                llist.AddLast(byte.Parse(leading_mult_str[i].ToString()));

            for (int j = 0; j < zero_count; j++)
                llist.AddLast((byte)0);

            return llist;
        }

        /// <summary>
        /// Multiply two BigInts
        /// </summary>
        public static BigInt Multiply(BigInt lhs, BigInt rhs)
        {
            if(lhs.digits == null || rhs.digits == null) //0 * b = 0 = a * 0
                return BigInt.Zero;

            LinkedList<byte> result_mult = null;

            int lzeros = lhs.digits.Count - 1;
            LinkedListNode<byte> ldigit = lhs.digits.First;
            for (; lzeros >= 0; lzeros--, ldigit = ldigit.Next) //standard multiplication algorithm
            {
                int rzeros = rhs.digits.Count - 1;
                LinkedListNode<byte> rdigit = rhs.digits.First;
                for (; rzeros >= 0; rzeros--, rdigit = rdigit.Next)
                {
                    int leading_mult = (ldigit.Value * rdigit.Value);
                    if (leading_mult != 0) //skip adding zero products
                    {
                        if (result_mult == null) //init result_mult... don't like doing this check after already initialized
                            result_mult = GenMultPart(leading_mult, rzeros + lzeros);
                        else //add to result mult
                            AddTo(result_mult, GenMultPart(leading_mult, rzeros + lzeros)); //mutational addition for better memory management (less allocation of large linkedlists).
                    }
                }
            }

            return new BigInt(result_mult, lhs.isneg ^ rhs.isneg);
        }

        /// <summary>
        /// Increment
        /// </summary>
        public static BigInt operator ++(BigInt bi)
        {
            return bi + BigInt.One;
        }

        /// <summary>
        /// Decrement
        /// </summary>
        public static BigInt operator --(BigInt bi)
        {
            return bi - BigInt.One;
        }

        ///// <summary>
        ///// version 1, very slow
        ///// Raise a BigInt to an int power
        ///// </summary>
        //public static BigInt Pow(BigInt lhs, int power)
        //{
        //    if (power < 0)
        //        throw new ArgumentOutOfRangeException("rhs must be postive");
        //    else if (power == 0)
        //        return BigInt.One;
        //    else if (power == 1)
        //        return lhs;

        //    BigInt result = lhs; //Multiply is non-mutational, so don't need deep copy.
        //    for (int i = 1; i < power; i++)
        //        result = BigInt.Multiply(result, lhs);

        //    return result;
        //}

        /// <summary>
        /// Raises a BigInt to an uint power
        /// </summary>
        /// <param name="x"></param>
        /// <param name="n"></param>
        /// <see cref="http://en.wikipedia.org/wiki/Exponentiation_by_squaring"/>
        /// <returns></returns>
        public static BigInt Pow(BigInt x, uint n)
        {
            BigInt result = BigInt.One;
            while (n != 0) {
                if ((n & 1) != 0)  /* n is odd, bitwise test */
                    result = BigInt.Multiply(result, x);

                x = BigInt.Multiply(x, x); //might consider mutational mult. (would have to remember to reassign x to a deep copy of itself first)
                n /= 2;     /* integer division, rounds down */
            }
            return result;
        }


        /// <summary>
        /// Perform division with remainder
        /// </summary>
        /// <param name="a">dividend</param>
        /// <param name="d">divisor</param>
        /// <param name="r">remainder</param>
        /// <returns>quotient</returns>
        public static BigInt Divide(BigInt a, BigInt d, out BigInt r)
        {
            if (d.digits == null) // a / 0 DNE
                throw new DivideByZeroException();

            if (a.digits == null) // 0 / d = 0, remainder 0
            {
                r = BigInt.Zero;
                return BigInt.Zero;
            }

            //now we know a.digits and d.digits are both non-null
            if (IsLessThan(a.digits, d.digits)) // a < d -> a / d = 0, remainder = a
            {
                r = a;
                return BigInt.Zero;
            }

            LinkedList<byte> q = new LinkedList<byte>(); //we know q isn't zero
            LinkedList<byte> r_digits = null;
            for (LinkedListNode<byte> curbyte = a.digits.First; curbyte != null; curbyte = curbyte.Next)
            {
                if (r_digits != null || curbyte.Value != 0) //skip leading zeros
                {
                    if (r_digits == null)
                        r_digits = new LinkedList<byte>();

                    r_digits.AddLast(curbyte.Value);
                }

                //d.digits is never null, so if r_digits is null, it is "less" than d.digits
                if (r_digits == null || IsLessThan(r_digits, d.digits))
                {
                    if(q.Count != 0) //skip leading zeros
                        q.AddLast((byte)0);
                }
                else
                {
                    byte q_digit = BruteDivide(ref r_digits, d.digits); //r_digits is both input and output parameter
                    if(q.Count != 0 || q_digit != 0) //skip leading zeros
                        q.AddLast(q_digit);
                }
            }

            r = new BigInt(r_digits, a.isneg);//even though mathematicians would always have r positive, we are being consistant with int type.
            return new BigInt(q, a.isneg ^ d.isneg);
        }

        /// <summary>
        /// brut divisions where
        /// a_digits != null
        /// d_digits != null
        /// a and d are positive
        /// a is not less than d
        /// and quotient is a less than 10
        /// </summary>
        private static byte BruteDivide(ref LinkedList<byte> a_digits, LinkedList<byte> d_digits)
        {
            BigInt r = new BigInt(a_digits, false);
            int q = 0;
            for (; r.digits != null ; q++)
            {
                r = SubtractPhase3(r.digits, d_digits);
                if (r.isneg)
                    break;
                else if (r.digits == null)
                {
                    q++;
                    break;
                }
            }

            if(r.isneg)
                r = SubtractPhase3(d_digits, r.digits); //d + r = d - abs(r), when r<0

            a_digits = r.digits;
            return (byte)q;
        }

        /// <summary>
        /// Division operator
        /// </summary>
        public static BigInt operator /(BigInt lhs, BigInt rhs)
        {
            return Divide(lhs, rhs);
        }

        /// <summary>
        /// Perform division without remainder
        /// </summary>
        public static BigInt Divide(BigInt lhs, BigInt rhs)
        {
            BigInt r;
            return Divide(lhs, rhs, out r);
        }

        /// <summary>
        /// The ceiling after division
        /// </summary>
        public static BigInt DivideCeiling(BigInt lhs, BigInt rhs)
        {
            BigInt r;
            BigInt result = Divide(lhs, rhs, out r);
            
            if (r == BigInt.Zero) //no remainder
                return result;
            else if (lhs.isneg ^ rhs.isneg) //result (with remainder) is negative
                return result;
            else
                return ++result;
        }

        /// <summary>
        /// The floor after division
        /// </summary>
        public static BigInt DivideFloor(BigInt lhs, BigInt rhs)
        {
            BigInt r;
            BigInt result = Divide(lhs, rhs, out r);

            if (r == BigInt.Zero) //no remainder
                return result;
            else if (lhs.isneg ^ rhs.isneg) //result (with remainder) is negative
                return --result;
            else
                return result;
        }

        /// <summary>
        /// mod: remainder after division, operator
        /// </summary>
        public static BigInt operator %(BigInt lhs, BigInt rhs)
        {
            return Mod(lhs, rhs);
        }

        /// <summary>
        /// The remainder after division
        /// </summary>
        public static BigInt Mod(BigInt lhs, BigInt rhs)
        {
            BigInt r;
            Divide(lhs, rhs, out r);
            return r;
        }

        /// <summary>
        /// negation operator
        /// </summary>
        public static BigInt operator -(BigInt bi)
        {
            return Negate(bi);
        }

        /// <summary>
        /// The negation of a BigInt
        /// </summary>
        public static BigInt Negate(BigInt bi)
        {
            return new BigInt(bi.digits, !bi.isneg);
        }

        /// <summary>
        /// The absolute value of a BigInt
        /// </summary>
        public static BigInt Abs(BigInt bi)
        {
            return new BigInt(bi.digits, false);
        }

        #endregion Arithmetic Algorithms and Operators

        #region  Implicit Operators (from integral types to BigInt)

        //note: implicit operators also serve as explicit operators

        public static implicit operator BigInt(Byte value)
        {
            return BigInt.Parse(value.ToString());
        }

        public static implicit operator BigInt(SByte value)
        {
            return BigInt.Parse(value.ToString());
        }

        public static implicit operator BigInt(UInt16 value)
        {
            return BigInt.Parse(value.ToString());
        }

        public static implicit operator BigInt(Int16 value)
        {
            return BigInt.Parse(value.ToString());
        }

        public static implicit operator BigInt(UInt32 value)
        {
            return BigInt.Parse(value.ToString());
        }

        public static implicit operator BigInt(Int32 value)
        {
            return BigInt.Parse(value.ToString());
        }

        public static implicit operator BigInt(UInt64 value)
        {
            return BigInt.Parse(value.ToString());
        }

        public static implicit operator BigInt(Int64 value)
        {
            return BigInt.Parse(value.ToString());
        }

        //may consider implicit conversion from bool or datetime to since no precision / information loss
        //on the other hand, these are not integral types, so not purely logical

        #endregion Implicit Operators (from integral types to BigInt)

        #region Explicit Operators (from boolean, DateTime, string, and rational types to BigInt)

        public static explicit operator BigInt(Decimal value)
        {
            return BigInt.Parse(Math.Truncate(value).ToString());
        }

        public static explicit operator BigInt(Double value)
        {
            return BigInt.Parse(Math.Truncate(value).ToString());
        }

        public static explicit operator BigInt(Single value)
        {
            return BigInt.Parse(Math.Truncate(value).ToString());
        }

        public static explicit operator BigInt(Boolean value)
        {
            return value ? BigInt.One : BigInt.Zero;
        }

        public static explicit operator BigInt(DateTime value)
        {
            return value.Ticks;
        }

        //while a little unnatural and can always use BigInt.Parse(string),
        //will serve well for purposes of Enumerable<string>.Cast<BigInt>()
        public static explicit operator BigInt(string value)
        {
            return BigInt.Parse(value);
        }

        #endregion Explicit Operators (from boolean, DateTime, and rational types to BigInt)

        #region Explicit Operators (from BigInt to other types)

        public static explicit operator Byte(BigInt value)
        {
            return Byte.Parse(value.ToString());
        }

        public static explicit operator SByte(BigInt value)
        {
            return SByte.Parse(value.ToString());
        }
        
        public static explicit operator UInt16(BigInt value)
        {
            return UInt16.Parse(value.ToString());
        }
        
        public static explicit operator Int16(BigInt value)
        {
            return Int16.Parse(value.ToString());
        }
        
        public static explicit operator UInt32(BigInt value)
        {
            return UInt32.Parse(value.ToString());
        }

        public static explicit operator Int32(BigInt value)
        {
            return Int32.Parse(value.ToString());
        }

        public static explicit operator UInt64(BigInt value)
        {
            return UInt64.Parse(value.ToString());
        }

        public static explicit operator Int64(BigInt value)
        {
            return Int64.Parse(value.ToString());
        }

        //though will never throw, unnatural for implicit conversion
        public static explicit operator Boolean(BigInt value)
        {
            return value.digits == null ? false : true;
        }

        //though will never throw, unnatural for implicit conversion
        public static explicit operator DateTime(BigInt value)
        {
            return new DateTime(Int64.Parse(value.ToString()));
        }

        public static explicit operator Char(BigInt value)
        {
            return (Char)UInt16.Parse(value.ToString());
        }

        #endregion Explicit Operators (from BigInt to other types)

        #region IConvertable

        //we risk redundant implementations for the sake of performance

        /// <summary>
        /// Returns the System.TypeCode for the Swensen.Object
        /// </summary>
        /// <returns></returns>
        public TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return this.digits == null ? false : true;
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Byte.Parse(this.ToString());
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            return (Char)UInt16.Parse(this.ToString());
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return new DateTime(long.Parse(this.ToString()));
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Decimal.Parse(this.ToString());
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Double.Parse(this.ToString());
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Int16.Parse(this.ToString());
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Int32.Parse(this.ToString());
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Int64.Parse(this.ToString());
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            return SByte.Parse(this.ToString());
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Single.Parse(this.ToString());
        }

        string IConvertible.ToString(IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this, conversionType, provider); //huh?
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            return UInt16.Parse(this.ToString());
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            return UInt32.Parse(this.ToString());
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            return UInt64.Parse(this.ToString());
        }

        #endregion IConvertable

        #region Parse Methods

        /// <summary>
        /// Converts the string representation of an integer to its BigInt equivalent
        /// </summary>
        public static BigInt Parse(string numstr)
        {
            if (numstr == null)
                throw new ArgumentNullException("numstr");

            BigInt result;
            if (!TryParse(numstr, out result))
                throw new FormatException("Input string was not in the correct format");

            return result;
        }

        /// <summary>
        /// Converts the string representation of an integer to its BigInt equivalent.  
        /// A return value indicates whether the conversion succeeded
        /// </summary>
        public static bool TryParse(string numstr, out BigInt result)
        {
            LinkedList<byte> digits;
            bool isneg;

            bool success = TryParse(numstr, out digits, out isneg);
            result = new BigInt(digits, isneg); //digits is null if success is false; thus result == BigInt.zero
            
            return success;
        }
        
        /// <summary>
        /// Convert numstr into digits, and isneg BigInt components; return success; digits is null if success is false
        /// </summary>
        private static bool TryParse(string numstr, out LinkedList<byte> digits, out bool isneg)
        {
            digits = null;
            isneg = false;

            if (numstr == null)
                return false;

            numstr = numstr.Trim();
            if (numstr == string.Empty)
                return false;

            if (numstr[0] == '-')
            {
                if (numstr.Length == 1) //"-"
                    return false;

                isneg = true;
            }

            int i = isneg ? 1 : 0;
            //skip leading zeros, including sole zero
            for (; i < numstr.Length; i++)
                if (numstr[i] != '0')
                    break;

            if (i == numstr.Length) //"-00000" == "0"
                return true;

            LinkedList<byte> digits_try = new LinkedList<byte>();
            for (; i < numstr.Length; i++)
            {
                byte digit;
                if (!byte.TryParse(numstr[i].ToString(), out digit))
                    return false; //digits is still null

                digits_try.AddLast(digit);
            }

            digits = digits_try;
            return true;
        }
        #endregion Parse Methods

        #region Eval
        //note that ops of longer length are listed first where using compound symbols (compared to BinaryOpTokens too)
        static readonly string[] UnaryOpTokens =
            new string[] 
            { 
                "gethashcode", "properdivisors", "divisors", 
                "negate", "abs", "++", "--",
                "sum", "min", "max", "range",
                "lcm", "gcd", "sqrt", "pow"
            };

        //note that ops of longer length are listed first where using compound symbols
        static readonly string[] BinaryOpTokens =
            new string[] 
            { 
                "==", "!=", ">=", "<=", ">=", "<=", ">", "<",
                "/floor", "/ceiling", "/%", "/", "%", // "/%" is division with remainder
                "+", "*", "^", "-" //make sure "-" is last
            };

        static readonly string[] AllOpTokens = Enumerable.Union(UnaryOpTokens, BinaryOpTokens).ToArray();

        /// <summary>
        /// Evaluates simple BigInt expressions.
        /// </summary>
        public static object Eval(string expression)
        {
            if (expression == null)
                throw new NullReferenceException("expression cannot be null");

            string[] tokens = null;

            expression = expression.Replace(" ", string.Empty);

            string op = null;
            for (int i = 0; i < AllOpTokens.Length; i++)
            {
                op = AllOpTokens[i];
                if (expression.Contains(op))
                {
                    tokens = expression.Split(new string[] {op}, StringSplitOptions.RemoveEmptyEntries);
                    break;
                }
            }

            if (tokens == null)
                throw new FormatException("no op tokens in expression");
            else if (tokens.Length == 2)
            {
                BigInt a = BigInt.Parse(tokens[0]);
                BigInt b = BigInt.Parse(tokens[1]);

                switch (op)
                {
                    case "+":
                        return (a + b);
                    case "-":
                        return (a - b);
                    case "*":
                        return (a * b);
                    case "^":
                        return (BigInt.Pow(a, Convert.ToUInt32(tokens[1])));
                    case "/%":
                        BigInt r;
                        BigInt q = Divide(a, b, out r);
                        return string.Format("q = {0}, r = {1}", q, r);
                    case "/floor":
                        return DivideFloor(a, b);
                    case "/ceiling":
                        return DivideCeiling(a, b);
                    case "/":
                        return (a / b);
                    case "%":
                        return (a % b);
                    case "==":
                        return (a == b);
                    case ">":
                        return (a > b);
                    case ">=":
                        return (a >= b);
                    case "<":
                        return (a < b);
                    case "<=":
                        return (a <= b);
                    default:
                        throw new FormatException("Invalid binary operator");
                }
            }
            else if (tokens.Length == 1)
            {
                string[] strArgs = tokens[0].Split(',');
                BigInt[] args = Parse(strArgs).ToArray();
                BigInt a = args[0];
                switch (op)
                {
                    case "gethashcode":
                        return a.GetHashCode();
                    case "abs":
                        return BigInt.Abs(a);
                    case "negate":
                        return BigInt.Negate(a);
                    case "properdivisors":
                        return ToString(a.ProperDivisors);
                    case "divisors":
                        return ToString(a.Divisors);
                    case "sum":
                        return Sum(args);
                    case "range":
                        return ToString(BigInt.Range(args[0], args[1]));
                    case "min":
                        return Min(args);
                    case "max":
                        return Max(args);
                    case "lcm":
                        return Lcm(args[0], args[1]);
                    case "gcd":
                        return Gcd(args[0], args[1]);
                    case "pow":
                        return Pow(a, Convert.ToUInt32(strArgs[1]));
                    case "sqrt":
                        return Sqrt(a);
                    case "++":
                        return (++a);
                    case "--":
                        return (--a);
                    default:
                        throw new FormatException("Invalid unary operator");
                }
            }

            throw new FormatException("invalid expression");
        }

        private static IEnumerable<BigInt> Parse(IEnumerable<string> input)
        {
            foreach (string i in input)
                yield return BigInt.Parse(i);
        }

        private static string ToString(object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        private static string ToString<T>(IEnumerable<T> objs)
        {
            if (objs == null)
                return string.Empty;

            return string.Join(", ", objs.Select<T, string>((T x) => x.ToString()).ToArray());
        }

        #endregion Eval

        #region Common Algorithms

        /// <summary>
        /// Yields the inclusive range of values from start to end
        /// </summary>
        public static IEnumerable<BigInt> Range(BigInt start, BigInt end)
        {
            //if (upperBound < lowerBound)
                //throw new ArgumentOutOfRangeException("upperBound cannot be less than lowerBound");

            if (end >= start)
            {
                do
                {
                    yield return start;
                    start++;
                } while (start <= end);
            }
            else
            {
                do
                {
                    yield return start;
                    start--;
                } while (start >= end);
            }
        }

        /// <summary>
        /// Calculates the maximum of two BigInts
        /// </summary>
        public static BigInt Max(BigInt lhs, BigInt rhs)
        {
            return lhs >= rhs ? lhs : rhs;
        }

        /// <summary>
        /// Calculates the maximum of a set of BigInts
        /// </summary>
        public static BigInt Max(params BigInt[] seq)
        {
            return seq.Max();
        }

        /// <summary>
        /// Calculates the maximum of a set of BigInts
        /// </summary>
        public static BigInt Max(IEnumerable<BigInt> seq)
        {
            return seq.Max();
        }

        /// <summary>
        /// Calculates the minimum of two BigInts
        /// </summary>
        public static BigInt Min(BigInt lhs, BigInt rhs)
        {
            return lhs <= rhs ? lhs : rhs;
        }

        /// <summary>
        /// Calculates the minimum of a set of BigInts
        /// </summary>
        public static BigInt Min(params BigInt[] seq)
        {
            return seq.Min();
        }

        /// <summary>
        /// Calculates the minimum of a set of BigInts
        /// </summary>
        public static BigInt Min(IEnumerable<BigInt> seq)
        {
            return seq.Min();
        }

        /// <summary>
        /// Calculates the sumation of a set of BigInts
        /// </summary>
        public static BigInt Sum(params BigInt[] seq)
        {
            return Sum((IEnumerable<BigInt>) seq);
        }

        /// <summary>
        /// Calculates the sumation of a set of BigInts
        /// </summary>
        public static BigInt Sum(IEnumerable<BigInt> seq)
        {
            bool seeded = false;
            BigInt sum = BigInt.Zero;
            foreach (BigInt bi in seq)
            {
                if (bi != BigInt.Zero) //don't bother
                {
                    if (!seeded) //seed sum
                    {
                        sum = new BigInt(new LinkedList<byte>(bi.digits), bi.isneg); //deep copy for AddTo
                        seeded = true;
                    }
                    else if (sum.isneg ^ bi.isneg) //can use AddTo if and only if sum and bi are both positive or both negative
                        sum += bi;
                    else //when we can
                        BigInt.AddTo(sum.digits, bi.digits);
                }
            }

            return sum;
        }

        /// <summary>
        /// Finds the greatest common divisor of two BigInts
        /// </summary>
        //public static BigInt Gcd(BigInt lhs, BigInt rhs)
        //{
        //    if (lhs == BigInt.Zero)
        //        throw new ArgumentOutOfRangeException("lhs", "Gcd is not defined for BigInt.Zero");
        //    else if (lhs == BigInt.Zero) //bug
        //        throw new ArgumentOutOfRangeException("rhs", "Gcd is not defined for BigInt.Zero");

        //    lhs = BigInt.Abs(lhs);
        //    rhs = BigInt.Abs(rhs);
        //    return Max(Enumerable.Intersect(lhs.Divisors, rhs.Divisors)); //not fast, should use euclidean method
        //}

        /// <summary>
        /// Finds the greatest common divisor of two BigInts
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <see cref="http://en.wikipedia.org/wiki/Euclidean_algorithm"/>
        /// <returns></returns>
        public static BigInt Gcd(BigInt lhs, BigInt rhs)
        {
            if (lhs.digits == null)
                throw new ArgumentOutOfRangeException("lhs", "Gcd is not defined for BigInt.Zero");
            else if (rhs.digits == null)
                throw new ArgumentOutOfRangeException("rhs", "Gcd is not defined for BigInt.Zero");
            
            while(rhs.digits != null)
            {
                if(lhs > rhs) //not sure if we can trust digits not to be null
                    lhs = BigInt.Subtract(lhs, rhs);
                else
                    rhs = BigInt.Subtract(rhs, lhs);
            }

            return lhs;
        }

        //public static BigInt GCD(params BigInt[] seq)
        //{
        //    return GCD((IEnumerable<BigInt>)seq);
        //}

        //public static BigInt GCD(IEnumerable<BigInt> seq)
        //{

        //}

        /// <summary>
        /// Finds the least common multiple of two BigInts
        /// </summary>
        public static BigInt Lcm(BigInt lhs, BigInt rhs)
        {
            if (lhs.digits == null || rhs.digits == null)
                return BigInt.Zero;

            return (lhs * rhs) / Gcd(lhs, rhs);
        }

        /// <summary>
        /// Calcultes the truncated square root of a BigInt
        /// </summary>
        /// <param name="value">the operand</param>
        /// <see cref="http://www.codecodex.com/wiki/index.php?title=Calculate_an_integer_square_root"/>
        /// <returns>Square root of value</returns>
        public static BigInt Sqrt(BigInt value)
        {
            if (value.digits == null) 
                return BigInt.Zero;  // Avoid zero divide

            BigInt n = DivideCeiling(value, BigInt.Two);// Initial estimate, never low
            BigInt n1 = (n + (value / n)) / BigInt.Two;
            while (n1 < n)
            {
                n = n1;
                n1 = (n + (value / n)) / BigInt.Two;
            } // end while

            return n;
        } // end Isqrt()

        #endregion Common Algorithms

        #region IXmlSerializable

        //default xml serialization is unsuitable (public properties and fields are exactly what we don't want!)

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteString(this.ToString());
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            TryParse(reader.ReadString(), out this.digits, out this.isneg);
        }

        XmlSchema IXmlSerializable.GetSchema()
        {
            return (null);
        }

        #endregion IXmlSerializable
    }
}
//implement IEnumerable extensions for Sum, Ave, etc.

//public static void Test()
//{
//    Console.WriteLine(BigInt.Eval(Console.ReadLine()));
//    //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
//    //sw.Start();
//    //for (int i = 0; i < 100000; i++)
//    //{
//    //    BigInt result = new BigInt("1234321234321") + new BigInt("2343234324321");
//    //}
//    //sw.Stop();
//    //Console.WriteLine(sw.ElapsedTicks);

//    //sw.Reset();
//    //sw.Start();
//    //for (int i = 0; i < 100000; i++)
//    //{
//    //    long result = (long)1234321234321 + (long)2343234324321;
//    //}
//    //sw.Stop();
//    //Console.WriteLine(sw.ElapsedTicks);


//    //BigInt a = new BigInt("2374692387492387432329847234982374923874239847239492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823482374623832984723498292387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823437492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823492387432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823432329847234982374923874239847239482374623832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623479823423832984723498237492387423984723948776238746234237462383298472349823749238742398472394877623874623423746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234");
//    //BigInt b = new BigInt("923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234923874323298472349823749238742398472394823746238329847234982374923874239847239487762387462342374623832984723498237492387423984723948776238746234798234");

//    //Console.WriteLine(a / b);
//    //Console.WriteLine(a);
//    //Console.WriteLine("+");
//    //Console.WriteLine(b);
//    //Console.WriteLine("=");
//    //Console.WriteLine(b - a);

//    //sw.Start();

//    //Console.WriteLine(sw.ElapsedTicks);

//    //sw.Reset();
//    //sw.Start();

//    //sw.Stop();
//    //Console.WriteLine(sw.ElapsedTicks);
//}
