using System;
using System.Reflection;

namespace Qkmaxware.Numbers {

/// <summary>
/// Class able to perform basic arithmetic on the given type
/// </summary>
/// <typeparam name="T">type to compute with</typeparam>
public interface ICalculator<T> {
    T Add(T lhs, T rhs);
    T Subtract(T lhs, T rhs);
    T Multiply(T lhs, T rhs);
    T Divide(T lhs, T rhs);
}

/// <summary>
/// Class capable of providing a calulator for the given type
/// </summary>
/// <typeparam name="T">type to compute with</typeparam>
public interface ICalulatorProvider<T> {
    public ICalculator<T> CreateCalculator();
}

/// <summary>
/// Extension methods for obtaining calculators for various types
/// </summary>
public static class CalculatorExtensions {
    public static ICalculator<long> GetCalculator(this long value) {
        return new LongCalculator();
    }
    public static ICalculator<int> GetCalculator(this int value) {
        return new IntCalculator();
    }
    public static ICalculator<double> GetCalculator(this double value) {
        return new DoubleCalculator();
    }
    public static ICalculator<float> GetCalculator(this float value) {
        return new FloatCalculator();
    }
    /// <summary>
    /// Generic method of obtaining calculators assuming the type provides its own calculator
    /// </summary>
    /// <param name="value">value to obtain the calculator for</param>
    /// <typeparam name="T">type of the value to obtain the calculator for</typeparam>
    /// <returns>the calculator if the value provides its own caluator for its own type otherwise an exception is thrown</returns>
    public static ICalculator<T> GetCalculator<T>(this T value) {
        if (value is ICalulatorProvider<T> provider) {
            return provider.CreateCalculator();
        } else {
            throw new ArgumentException("This type does not define a calculator");
        }
    }
}

public class LongCalculator : ICalculator<long> {
    public long Add(long lhs, long rhs) => lhs + rhs;
    public long Subtract(long lhs, long rhs) => lhs - rhs;
    public long Multiply(long lhs, long rhs) => lhs * rhs;
    public long Divide(long lhs, long rhs) => lhs / rhs;
}

public class IntCalculator : ICalculator<int> {
    public int Add(int lhs, int rhs) => lhs + rhs;
    public int Subtract(int lhs, int rhs) => lhs - rhs;
    public int Multiply(int lhs, int rhs) => lhs * rhs;
    public int Divide(int lhs, int rhs) => lhs / rhs;
}

public class DoubleCalculator : ICalculator<double> {
    public double Add(double lhs, double rhs) => lhs + rhs;
    public double Subtract(double lhs, double rhs) => lhs - rhs;
    public double Multiply(double lhs, double rhs) => lhs * rhs;
    public double Divide(double lhs, double rhs) => lhs / rhs;
}

public class FloatCalculator : ICalculator<float> {
    public float Add(float lhs, float rhs) => lhs + rhs;
    public float Subtract(float lhs, float rhs) => lhs - rhs;
    public float Multiply(float lhs, float rhs) => lhs * rhs;
    public float Divide(float lhs, float rhs) => lhs / rhs;
}

public class ScientificCalculator : ICalculator<Scientific> {
    public Scientific Add(Scientific lhs, Scientific rhs) => lhs + rhs;
    public Scientific Subtract(Scientific lhs, Scientific rhs) => lhs - rhs;
    public Scientific Multiply(Scientific lhs, Scientific rhs) => lhs * rhs;
    public Scientific Divide(Scientific lhs, Scientific rhs) => lhs / rhs;
}

public class ArbitraryCalculator : ICalculator<Arbitrary> {
    public Arbitrary Add(Arbitrary lhs, Arbitrary rhs) => lhs + rhs;
    public Arbitrary Subtract(Arbitrary lhs, Arbitrary rhs) => lhs - rhs;
    public Arbitrary Multiply(Arbitrary lhs, Arbitrary rhs) => lhs * rhs;
    public Arbitrary Divide(Arbitrary lhs, Arbitrary rhs) => lhs / rhs;
}

}