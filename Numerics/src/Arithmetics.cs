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
/// Type can be multiplied by a scalar value
/// </summary>
/// <typeparam name="I">Type of scalar value</typeparam>
/// <typeparam name="R">Result type</typeparam>
public interface IScalable<I, R> {
    R ScaleBy(I lhs);
}

/// <summary>
/// Interface for types that can be used within a vector
/// </summary>
/// <typeparam name="T">arithmetic type</typeparam>
public interface IVectorable<T> {
    public T ScalarNegation();
    public T ScalarSqrt();
    public T ScalarScaleBy(T scaling);
    public T ScalarAddBy(T other);
    public T ScalarSubtractBy(T other);
    public T ScalarMultiplyBy(T other);
    public T ScalarDivideBy(T other);
}

}