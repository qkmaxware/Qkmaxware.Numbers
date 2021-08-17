using System;

namespace Qkmaxware.Numbers {

/// <summary>
/// Fraction representation using calculators to perform computations
/// </summary>
/// <typeparam name="T">Numeric type</typeparam>
public class Fraction<T> {

    private ICalculator<T> calculator;
    /// <summary>
    /// The numerator in the fraction
    /// </summary>
    /// <value>numerator</value>
    public T Numerator {get; private set;}
    /// <summary>
    /// The denominator in the fraction
    /// </summary>
    /// <value>denominator</value>
    public T Denominator {get; private set;}

    /// <summary>
    /// Create a fraction
    /// </summary>
    /// <param name="numerator">Fraction numerator</param>
    /// <param name="denominator">Fraction denominator</param>
    public Fraction(T numerator, T denominator) {
        this.calculator = numerator.GetCalculator() ?? denominator.GetCalculator();
        this.Numerator = numerator;
        this.Denominator = denominator;
    }

    public static Fraction<T> operator + (Fraction<T> lhs, Fraction<T> rhs) {
        var tl = lhs.calculator.Multiply(lhs.Numerator, rhs.Denominator);
        var tr = rhs.calculator.Multiply(rhs.Numerator, lhs.Denominator);
        var bottom = lhs.calculator.Multiply(lhs.Denominator, rhs.Denominator);

        return new Fraction<T>(
           lhs.calculator.Add(tl, tr),
           bottom
        );
    }

    public static Fraction<T> operator - (Fraction<T> lhs, Fraction<T> rhs) {
        var tl = lhs.calculator.Multiply(lhs.Numerator, rhs.Denominator);
        var tr = rhs.calculator.Multiply(rhs.Numerator, lhs.Denominator);
        var bottom = lhs.calculator.Multiply(lhs.Denominator, rhs.Denominator);

        return new Fraction<T>(
           lhs.calculator.Subtract(tl, tr),
           bottom
        );
    }

    public static Fraction<T> operator / (Fraction<T> lhs, Fraction<T> rhs) {
        return new Fraction<T>(
            lhs.calculator.Multiply(lhs.Numerator, rhs.Denominator),
            lhs.calculator.Multiply(lhs.Denominator, rhs.Numerator)
        );
    }

    public static Fraction<T> operator * (Fraction<T> lhs, Fraction<T> rhs) {
        return new Fraction<T>(
            lhs.calculator.Multiply(lhs.Numerator, rhs.Numerator),
            lhs.calculator.Multiply(lhs.Denominator, rhs.Denominator)
        );
    }

    /// <summary>
    /// Downcast from a fraction to a raw value
    /// </summary>
    /// <param name="value">fraction</param>
    public static explicit operator T (Fraction<T> value) {
        return value.calculator.Divide(value.Numerator, value.Denominator);
    }

    public override string ToString() {
        return $"{this.Numerator}/{this.Denominator}";
    }

    public override bool Equals(object other) {
        if (other is null)
            return false;
        if (other is Fraction<T> frac) {
            return this.Numerator.Equals(frac.Numerator) && this.Denominator.Equals(frac.Denominator);
        } else {
            return false;
        }
    }
    public override int GetHashCode() {
        return HashCode.Combine(this.Numerator, this.Denominator);
    }

}

}