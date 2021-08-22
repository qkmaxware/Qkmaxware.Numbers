using System;
using System.Numerics;

namespace Qkmaxware.Numbers {
/// <summary>
/// Arbitrary precision large Arbitrary number
/// </summary>
public struct Arbitrary : INumeric<Arbitrary>, IScalable<Arbitrary,Arbitrary> {
    /// <summary>
    /// A Arbitrary representing 0
    /// </summary>
    public static readonly Arbitrary Zero = new Arbitrary(0,0);

    /// <summary>
    /// A Arbitrary representing 1
    /// </summary>
    public static readonly Arbitrary One = new Arbitrary(1,0);

    /// <summary>
    /// Digits in the significand
    /// </summary>
    /// <value>digits</value>
    public BigInteger Significand {get; private set;}

    /// <summary>
    /// Exponent applied as a power of 10
    /// </summary>
    /// <value>exponent</value>
    public int Exponent {get; private set;}

    /// <summary>
    /// Total number of digits in the number
    /// </summary>
    /// <returns>Precision of the number</returns>
    public int Precision {get; private set; }
    /// <summary>
    /// Total number of digits to the right of the decimal point
    /// </summary>
    public int Scale {get; private set; }

    /// <summary>
    /// The precision used for divide operations by default when not specified
    /// </summary>
    /// <value>integer</value>
    public static int DefaultDivisionScale {get; private set;} = 50;

    /// <summary>
    /// The whole number part of this Arbitrary
    /// </summary>
    /// <returns>number without fractional part</returns>
    public Arbitrary WholePart() => this.Truncate();
    /// <summary>
    /// The fractional part of this Arbitrary
    /// </summary>
    /// <returns>number without the whole part</returns>
    public Arbitrary FractionalPart() => this - WholePart();
    /// <summary>
	/// Sign of the value
	/// </summary>
	/// <returns>sign -1, 0, 1 for negative, 0, or positive</returns>
    public int Sign => this.Significand.Sign;

    /// <summary>
    /// Create a new Arbitrary
    /// </summary>
    /// <param name="mantissa">digits in the mantissa</param>
    /// <param name="exponent">exponent as a power of 10</param>
    public Arbitrary(BigInteger mantissa, int exponent = 0) : this(mantissa, exponent, true) {}

    private Arbitrary(BigInteger mantissa, int exponent, bool normalize) {
        // Assign base values
        this.Significand = mantissa;
        this.Exponent = exponent;

        // Normalize
        if (this.Significand.IsZero) {
            this.Exponent = 0;
        } else {
            if (normalize) {
                BigInteger remainder = 0;
                while (remainder == 0) {
                    var shortened = BigInteger.DivRem(dividend: this.Significand, divisor: 10, out remainder);
                    if (!remainder.IsZero) {
                        continue;
                    }
                    this.Significand = shortened;
                    this.Exponent++;
                }
            }
        }

        // Compute metrics
        this.Precision = digits(Significand) + Math.Max(0, Exponent);
        this.Scale = Math.Abs(Math.Min(0, Exponent));
    }

    /// <summary>
	/// Check if the value is 0
	/// </summary>
	/// <returns>true if 0</returns>s
    public bool IsZero() {
        return this.Significand.IsZero;
    }

    /// <summary>
	/// Absolute value of this number
	/// </summary>
	/// <returns>absolute value</returns>
    public Arbitrary Abs() {
        return new Arbitrary(BigInteger.Abs(this.Significand), this.Exponent, normalize: false); // Value already normalized
    }

    /// <summary>
    /// Set the precision of this value
    /// </summary>
    /// <param name="digits_precision">digits of precision</param>
    /// <returns>Arbitrary</returns>
    public Arbitrary SetPrecision(int digits_precision) {
        var shortened = this;
        var mantissa = this.Significand;
        var exp = this.Exponent;
        // remove the least significant digits, as long as the number of digits is higher than the given Precision
        var current_digits = digits(mantissa);
        while (current_digits > digits_precision) {
            mantissa /= 10; // Each one of these removes a digit from the end of the BigInteger
            exp++;
            current_digits--;
        }
        return new Arbitrary(mantissa, exp);
    }

    /// <summary>
    /// Round the number to the given number of decimal places
    /// </summary>
    /// <param name="decimals">number of decimals</param>
    /// <returns>Rounded Arbitrary</returns>
    public Arbitrary SetScale(int decimals) {
        var whole_digits = digits(Significand) + Exponent;
        var whole_with_decimals = whole_digits + decimals;
        return this.SetPrecision(whole_with_decimals);
    }

    /// <summary>
    /// Trim decimal places
    /// </summary>
    /// <returns>Arbitrary</returns>
    public Arbitrary Truncate() {
        return this.SetScale(0);
    }

    /// <summary>
    /// Floor of the value
    /// </summary>
    /// <returns>Arbitrary</returns>
    public Arbitrary Floor() {
        var no_decimals = Truncate();
        if (no_decimals > this) {
            return --no_decimals;
        } else {
            return no_decimals;
        }
    }

    /// <summary>
    /// Floor of the value as an integer
    /// </summary>
    /// <returns>BigInteger</returns>
    public BigInteger FloorToInt() {
        return Floor().Significand;
    }

    /// <summary>
    /// Ceiling of the value
    /// </summary>
    /// <returns>Arbitrary</returns>
    public Arbitrary Ceil() {
        var no_decimals = Truncate();
        if (this > no_decimals) {
            return ++no_decimals;
        } else {
            return no_decimals;
        }
    }

    /// <summary>
    /// Ceiling of the value as an integer
    /// </summary>
    /// <returns>BigInteger</returns>
    public BigInteger CeilToInt() {
        return Ceil().Significand;
    }

    /// <summary>
	/// Multiply by 10^x by shifting the exponent
	/// </summary>
	/// <param name="x">Amount so shift the exponent by</param>
	/// <returns>New scientific </returns>
    public Arbitrary x10(int x) {
        return new Arbitrary(this.Significand, this.Exponent + x, normalize: false); // Value already normalized
    }

    public override string ToString() {
        return this.Significand.ToString() + "E" + this.Exponent;
    }
    
    #region utilites

    private static int digits(BigInteger value) {
        if (value.IsZero)
            return 0;
        value = BigInteger.Abs(value);
        if (value.IsOne)
            return 1;

        var exp = (int)Math.Ceiling(BigInteger.Log10(value));
        var test = BigInteger.Pow(10, exp);
        return value >= test ? exp + 1 : exp;
    }

    private static BigInteger AlignExponent(Arbitrary value, Arbitrary reference) {
        return value.Significand * BigInteger.Pow(10, value.Exponent - reference.Exponent);
    }

    #endregion


    #region implicit boxing

    public static implicit operator Arbitrary(int value) {
        return new Arbitrary(value, 0);
    }

    public static implicit operator Arbitrary(double value) {
        var mantissa = (BigInteger) value;
        var exponent = 0;
        double scaleFactor = 1;
        while (Math.Abs(value * scaleFactor - (double)mantissa) > 0) {
            exponent -= 1;
            scaleFactor *= 10;
            mantissa = (BigInteger)(value * scaleFactor);
        }
        return new Arbitrary(mantissa, exponent);
    }

    public static implicit operator Arbitrary(decimal value) {
        var mantissa = (BigInteger)value;
        var exponent = 0;
        decimal scaleFactor = 1;
        while ((decimal)mantissa != value * scaleFactor)
        {
            exponent -= 1;
            scaleFactor *= 10;
            mantissa = (BigInteger)(value * scaleFactor);
        }
        return new Arbitrary(mantissa, exponent);
    }

    #endregion

    #region explicit unboxing

    public static explicit operator double(Arbitrary value) {
        return (double)value.Significand * Math.Pow(10, value.Exponent);
    }

    public static explicit operator float(Arbitrary value) {
        return Convert.ToSingle((double)value);
    }

    public static explicit operator decimal(Arbitrary value) {
        return (decimal)value.Significand * (decimal)Math.Pow(10, value.Exponent);
    }

    public static explicit operator int(Arbitrary value) {
        return (int)(value.Significand * BigInteger.Pow(10, value.Exponent));
    }

    public static explicit operator BigInteger(Arbitrary value) {
        return (BigInteger)(value.Significand * BigInteger.Pow(10, value.Exponent));
    }

    public static explicit operator uint(Arbitrary value) {
        return (uint)(value.Significand * BigInteger.Pow(10, value.Exponent));
    }

    public static explicit operator Scientific(Arbitrary value) {
        return new Scientific((int)value.Significand, value.Exponent);
    }

    #endregion

    #region arithmetic

    public static Arbitrary operator +(Arbitrary value) {
        return value;
    }

    public static Arbitrary operator -(Arbitrary value) {
        return new Arbitrary(value.Significand * -1, value.Exponent);
    }

    public static Arbitrary operator ++(Arbitrary value) {
        return value + 1;
    }

    public static Arbitrary operator --(Arbitrary value) {
        return value - 1;
    }

    public static Arbitrary operator +(Arbitrary left, Arbitrary right) {
        return add(left, right);
    }

    public static Arbitrary operator -(Arbitrary left, Arbitrary right) {
        return add(left, -right);
    }

    private static Arbitrary add(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent
            ? new Arbitrary(AlignExponent(left, right) + right.Significand, right.Exponent)
            : new Arbitrary(AlignExponent(right, left) + left.Significand, left.Exponent);
    }

    public static Arbitrary operator *(Arbitrary left, Arbitrary right){
        return new Arbitrary(left.Significand * right.Significand, left.Exponent + right.Exponent);
    }

    public static Arbitrary operator /(Arbitrary dividend, Arbitrary divisor) {
        //var sigFigs = Math.Min(dividend.Scale, divisor.Scale);
        return dividend.Divide(divisor, DefaultDivisionScale);
    }

    /// <summary>
    /// Divide this number by another
    /// </summary>
    /// <param name="divisor">value by which this number is divided by</param>
    /// <param name="scale">scale of the quotient to be returned (digits to the right of the decimal point)</param>
    /// <returns>quotient</returns>
    public Arbitrary Divide(Arbitrary divisor, int scale) {
        var exponentChange = (scale + Math.Max(this.Exponent, 0)) - this.Scale;
        return new Arbitrary(
            (this.Significand * BigInteger.Pow(10, exponentChange)) / divisor.Significand, 
            this.Exponent - divisor.Exponent - exponentChange
        );
    }

    /*public Arbitrary DivideWithPrecision(Arbitrary divisor, int precision) {
        var exponentChange = precision - (this.Precision - divisor.Precision);
        if (exponentChange < 0) {
            exponentChange = 0;
        }
        return new Arbitrary(
            (this.Mantissa * BigInteger.Pow(10, exponentChange)) / divisor.Mantissa, 
            this.Exponent - divisor.Exponent - exponentChange
        );
    }*/

    #endregion

    #region comparisons

    public bool Equals(Arbitrary other) {
        return other.Significand.Equals(Significand) && other.Exponent == Exponent;
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }
        return obj is Arbitrary && Equals((Arbitrary) obj);
    }

    public override int GetHashCode() {
        unchecked {
            return (Significand.GetHashCode()*397) ^ Exponent;
        }
    }

    public Arbitrary ScaleBy(Arbitrary value) => MultiplyBy(value);

    public Arbitrary Negate() {
        return -this;
    }

    public Arbitrary Add(Arbitrary rhs) {
        return this + rhs;
    }

    public Arbitrary Subtract(Arbitrary rhs) {
        return this - rhs;
    }

    public Arbitrary MultiplyBy(Arbitrary rhs) {
        return this * rhs;
    }

    public Arbitrary DivideBy(Arbitrary rhs) {
        return this / rhs;
    }

    // Source: http://mjs5.com/2016/01/20/c-biginteger-square-root-function/  Michael Steiner, Jan 2016
    // Found at https://stackoverflow.com/questions/3432412/calculate-square-root-of-a-biginteger-system-numerics-biginteger/6084813
    private static BigInteger BigintSqrt(BigInteger number){
        if (number < 9) {
            if (number == 0)
                return 0;
            if (number < 4)
                return 1;
            else
                return 2;
        }

        BigInteger n = 0, p = 0;
        var high = number >> 1;
        var low = BigInteger.Zero;

        while (high > low + 1) {
            n = (high + low) >> 1;
            p = n * n;
            if (number < p) {
                high = n;
            }
            else if (number > p) {
                low = n;
            }
            else {
                break;
            }
        }
        return number == p ? n : low;
    }

    public Arbitrary Sqrt() {
		return this.Sqrt(DefaultDivisionScale);	
	}

    public Arbitrary Sqrt(int scale) {
        var exponentChange = (scale + Math.Max(this.Exponent, 0)) - this.Scale;

        var newSignificand = this.Significand * BigInteger.Pow(10, exponentChange); // append decimals
        var newExponent = this.Exponent - exponentChange;

        if (newExponent.IsEven()) {
			// if n is even just take the square root of x and 10^n and multiply them
			var sqrt = new Arbitrary(BigintSqrt(newSignificand), newExponent / 2);
            //Console.WriteLine($"{this.Significand}E{this.Exponent} === {newSignificand}E{newExponent} -> {sqrt}");
            return sqrt;
		} else {
			// if n is odd, mutiply x by 10 and reduce n by 1 to make it even and then take square root of each and multiply them
			var sqrt = new Arbitrary(BigintSqrt(newSignificand * 10), (newExponent - 1)/2);
            //Console.WriteLine($"{this.Significand}E{this.Exponent} === {newSignificand}E{newExponent} -> {sqrt}");
            return sqrt;
		}
    }

    public static bool operator ==(Arbitrary left, Arbitrary right) {
        return left.Exponent == right.Exponent && left.Significand == right.Significand;
    }

    public static bool operator !=(Arbitrary left, Arbitrary right) {
        return left.Exponent != right.Exponent || left.Significand != right.Significand;
    }

    public static bool operator <(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) < right.Significand : left.Significand < AlignExponent(right, left);
    }

    public static bool operator >(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) > right.Significand : left.Significand > AlignExponent(right, left);
    }

    public static bool operator <=(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) <= right.Significand : left.Significand <= AlignExponent(right, left);
    }

    public static bool operator >=(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) >= right.Significand : left.Significand >= AlignExponent(right, left);
    }

    #endregion
}

}