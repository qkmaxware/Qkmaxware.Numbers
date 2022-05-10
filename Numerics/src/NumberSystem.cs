namespace Qkmaxware.Numbers {

/// <summary>
/// Class for systems that can be used to represent a number
/// </summary>
public abstract class NumberSystem {
    /// <summary>
    /// Parse a number in this number system to an integer
    /// </summary>
    /// <param name="value">string representation of the number in this number system</param>
    /// <returns>base 10 integer</returns>
    public abstract int Parse(string value);
    /// <summary>
    /// Convert a base 10 integer to a string representation in this number system
    /// </summary>
    /// <param name="value">base 10 value</param>
    /// <returns>string representation of the value in the this number system</returns>
    public abstract string ToString(int value);
}

/// <summary>
/// Extension methods for dealing with number systems
/// </summary>
public static class NumberSystemExtensions {
    /// <summary>
    /// Quickly convert an integer to a string in the given number system
    /// </summary>
    /// <param name="value">integer value</param>
    /// <param name="system">number system</param>
    /// <returns>string representation of the value in the this number system</returns>
    public static string ToString(this int value, NumberSystem system) {
        return system.ToString(value);
    }
}

}