using System;
using System.Numerics;

namespace Qkmaxware.Numbers {
/// <summary>
/// Arbitrary precision large Arbitrary number
/// </summary>
public struct Arbitrary {
    /// <summary>
    /// A Arbitrary representing 0
    /// </summary>
    public static readonly Arbitrary Zero = new Arbitrary(0,0);

    /// <summary>
    /// Digits in the mantissa
    /// </summary>
    /// <value>digits</value>
    public BigInteger Mantissa {get; private set;}

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
    /// Create a new Arbitrary
    /// </summary>
    /// <param name="mantissa">digits in the mantissa</param>
    /// <param name="exponent">exponent as a power of 10</param>
    public Arbitrary(BigInteger mantissa, int exponent) {
        // Assign base values
        this.Mantissa = mantissa;
        this.Exponent = exponent;

        // Normalize
        if (this.Mantissa.IsZero) {
            this.Exponent = 0;
        } else {
            BigInteger remainder = 0;
            while (remainder == 0) {
                var shortened = BigInteger.DivRem(dividend: this.Mantissa, divisor: 10, out remainder);
                if (!remainder.IsZero) {
                    continue;
                }
                this.Mantissa = shortened;
                this.Exponent++;
            }
        }

        // Compute metrics
        this.Precision = digits(Mantissa) + Math.Max(0, Exponent);
        this.Scale = Math.Abs(Math.Min(0, Exponent));
    }

    /// <summary>
    /// Set the precision of this value
    /// </summary>
    /// <param name="digits_precision">digits of precision</param>
    /// <returns>Arbitrary</returns>
    public Arbitrary SetPrecision(int digits_precision) {
        var shortened = this;
        var mantissa = this.Mantissa;
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
        var whole_digits = digits(Mantissa) + Exponent;
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
        return Floor().Mantissa;
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
        return Ceil().Mantissa;
    }

    public override string ToString() {
        return this.Mantissa.ToString() + "E" + this.Exponent;
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
        return value.Mantissa * BigInteger.Pow(10, value.Exponent - reference.Exponent);
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
        return (double)value.Mantissa * Math.Pow(10, value.Exponent);
    }

    public static explicit operator float(Arbitrary value) {
        return Convert.ToSingle((double)value);
    }

    public static explicit operator decimal(Arbitrary value) {
        return (decimal)value.Mantissa * (decimal)Math.Pow(10, value.Exponent);
    }

    public static explicit operator int(Arbitrary value) {
        return (int)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
    }

    public static explicit operator BigInteger(Arbitrary value) {
        return (BigInteger)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
    }

    public static explicit operator uint(Arbitrary value) {
        return (uint)(value.Mantissa * BigInteger.Pow(10, value.Exponent));
    }

    #endregion

    #region arithmetic

    public static Arbitrary operator +(Arbitrary value) {
        return value;
    }

    public static Arbitrary operator -(Arbitrary value) {
        return new Arbitrary(value.Mantissa * -1, value.Exponent);
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
            ? new Arbitrary(AlignExponent(left, right) + right.Mantissa, right.Exponent)
            : new Arbitrary(AlignExponent(right, left) + left.Mantissa, left.Exponent);
    }

    public static Arbitrary operator *(Arbitrary left, Arbitrary right){
        return new Arbitrary(left.Mantissa * right.Mantissa, left.Exponent + right.Exponent);
    }

    public static Arbitrary operator /(Arbitrary dividend, Arbitrary divisor) {
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
            (this.Mantissa * BigInteger.Pow(10, exponentChange)) / divisor.Mantissa, 
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
        return other.Mantissa.Equals(Mantissa) && other.Exponent == Exponent;
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }
        return obj is Arbitrary && Equals((Arbitrary) obj);
    }

    public override int GetHashCode() {
        unchecked {
            return (Mantissa.GetHashCode()*397) ^ Exponent;
        }
    }

    public static bool operator ==(Arbitrary left, Arbitrary right) {
        return left.Exponent == right.Exponent && left.Mantissa == right.Mantissa;
    }

    public static bool operator !=(Arbitrary left, Arbitrary right) {
        return left.Exponent != right.Exponent || left.Mantissa != right.Mantissa;
    }

    public static bool operator <(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) < right.Mantissa : left.Mantissa < AlignExponent(right, left);
    }

    public static bool operator >(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) > right.Mantissa : left.Mantissa > AlignExponent(right, left);
    }

    public static bool operator <=(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) <= right.Mantissa : left.Mantissa <= AlignExponent(right, left);
    }

    public static bool operator >=(Arbitrary left, Arbitrary right) {
        return left.Exponent > right.Exponent ? AlignExponent(left, right) >= right.Mantissa : left.Mantissa >= AlignExponent(right, left);
    }

    #endregion
}

}