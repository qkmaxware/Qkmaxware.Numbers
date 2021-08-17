using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base class for common number systems that can be represented with single characters for each digit
/// </summary>
public abstract class BaseNumberSystem : NumberSystem {
    /// <summary>
    /// Get the characters used in this number system
    /// </summary>
    /// <returns>list of valid characters</returns>
    protected abstract char[] GetCharacterSet();

    /// <summary>
    /// Fast integer power
    /// </summary>
    /// <param name="value">base</param>
    /// <param name="exp">exponent</param>
    /// <returns>base to the power of the given exponent</returns>
    protected int FastIPow(int value, int exp) {
        int result = 1; 
        while (exp > 0) {
            if ((exp & 1) != 0) { 
                result *= value;
            } 
            exp >>= 1; 
            value *= value; 
        } 
        return result; 
    }

    /// <summary>
    /// Prepare input for parsing 
    /// </summary>
    /// <param name="value">raw input</param>
    /// <returns>processed input</returns>
    protected virtual string PrepareInput(string value) {
        return value;
    }

    public override int Parse(string value) {
        var charSet = GetCharacterSet();
        value = PrepareInput(value);

        checked {
            var targetBase = charSet.Length;
            int numeric = default(int);
            var lastIndex = value.Length - 1;
            
            for (var i = 0; i < value.Length; i++) {
                var charIndex = lastIndex - i;
                var @char = value[charIndex];
                var valueIndex = Array.IndexOf(charSet, @char);
                if (valueIndex < 0) {
                    throw new ArgumentException($"Character '{@char}' doesn't exist in the base 2 number system");
                }
                numeric += valueIndex * FastIPow(targetBase, i);
            }

            return numeric;
        }
    }

    public override string ToString(int value) {
        var charSet = GetCharacterSet();
        
        // 32 is the worst cast buffer size for base 2 and int.MaxValue
        int i = 32;
        char[] buffer = new char[i];
        int targetBase= charSet.Length;

        do {
            buffer[--i] = charSet[value % targetBase];
            value = value / targetBase;
        }
        while (value > 0);

        char[] result = new char[32 - i];
        Array.Copy(buffer, i, result, 0, 32 - i);

        return new string(result);
    }
}

}