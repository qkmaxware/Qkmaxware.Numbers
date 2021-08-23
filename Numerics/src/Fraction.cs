using System;

namespace Qkmaxware.Numbers {

/// <summary>
/// Fraction representation using calculators to perform computations
/// </summary>
/// <typeparam name="T">Numeric type</typeparam>
public class Fraction<T> where T:IAddable<T, T>, ISubtractable<T,T>, IDividable<T,T>, IMultiplyable<T,T> {

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
        this.Numerator = numerator;
        this.Denominator = denominator;
    }

    public static Fraction<T> operator + (Fraction<T> lhs, Fraction<T> rhs) {
        var tl = lhs.Numerator.MultiplyBy(rhs.Denominator);
        var tr = rhs.Numerator.MultiplyBy(lhs.Denominator);
        var bottom = lhs.Denominator.MultiplyBy(rhs.Denominator);

        return new Fraction<T>(
           tl.Add(tr),
           bottom
        );
    }

    public static Fraction<T> operator - (Fraction<T> lhs, Fraction<T> rhs) {
        var tl = lhs.Numerator.MultiplyBy(rhs.Denominator);
        var tr = rhs.Numerator.MultiplyBy(lhs.Denominator);
        var bottom = lhs.Denominator.MultiplyBy(rhs.Denominator);

        return new Fraction<T>(
            tl.Subtract(tr),
           bottom
        );
    }

    public static Fraction<T> operator / (Fraction<T> lhs, Fraction<T> rhs) {
        return new Fraction<T>(
            lhs.Numerator.MultiplyBy(rhs.Denominator),
            lhs.Denominator.MultiplyBy(rhs.Numerator)
        );
    }

    public static Fraction<T> operator * (Fraction<T> lhs, Fraction<T> rhs) {
        return new Fraction<T>(
            lhs.Numerator.MultiplyBy(rhs.Numerator),
            lhs.Denominator.MultiplyBy(rhs.Denominator)
        );
    }

    /// <summary>
    /// Downcast from a fraction to a raw value
    /// </summary>
    /// <param name="value">fraction</param>
    public static explicit operator T (Fraction<T> value) {
        return value.Numerator.DivideBy(value.Denominator);
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
        return (this.Numerator, this.Denominator).GetHashCode();
    }

}

}