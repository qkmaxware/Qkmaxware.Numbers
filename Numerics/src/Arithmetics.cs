namespace Qkmaxware.Numbers {

/// <summary>
/// Type can be negated
/// </summary>
/// <typeparam name="R">Result type</typeparam>
public interface INegatable<R> {
    R Negate();
}

/// <summary>
/// Type can be square rooted
/// </summary>
/// <typeparam name="R">Result type</typeparam>
public interface ISquareRootable<R> {
    R Sqrt();
}

/// <summary>
/// Type can be added with
/// </summary>
/// <typeparam name="I">Type of second value</typeparam>
/// <typeparam name="R">Result type</typeparam>
public interface IAddable<I, R> {
    R Add(I rhs);
}

/// <summary>
/// Type can be subtracted with
/// </summary>
/// <typeparam name="I">Type of second value</typeparam>
/// <typeparam name="R">Result type</typeparam>
public interface ISubtractable<I, R> {
    R Subtract(I rhs);
}

/// <summary>
/// Type can be divided by
/// </summary>
/// <typeparam name="I">Type of second value</typeparam>
/// <typeparam name="R">Result type</typeparam>
public interface IDividable<I, R> {
    R DivideBy(I rhs);
}

/// <summary>
/// Type can be multiplied by
/// </summary>
/// <typeparam name="I">Type of second value</typeparam>
/// <typeparam name="R">Result type</typeparam>
public interface IMultiplyable<I, R> {
    R MultiplyBy(I rhs);
}

/// <summary>
/// Interface for numerics types that support all basic arithmetic operators
/// </summary>
/// <typeparam name="T">arithmetic type</typeparam>
public interface INumeric<T> : INegatable<T>, ISquareRootable<T>, IAddable<T,T>, ISubtractable<T,T>, IMultiplyable<T,T>, IDividable<T,T> {}

}