namespace Qkmaxware.Numbers {

/// <summary>
/// Interface for converting values of one type to another
/// </summary>
/// <typeparam name="T">conversion output</typeparam>
public interface IConvertable<T> {
    /// <summary>
    /// Convert from the current type to an intance of the given type
    /// </summary>
    /// <param name="value">result of the conversion</param>
    void Convert(out T value);
}

}