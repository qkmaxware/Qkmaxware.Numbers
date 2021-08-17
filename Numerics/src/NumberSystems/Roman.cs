using System;
using System.Collections.Generic;
using System.Text;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Roman Numerals
/// </summary>
public class Roman : NumberSystem {
    private static Dictionary<char, int> charSet = new Dictionary<char, int> {
        { 'I', 1 },
        { 'V', 5 },
        { 'X', 10 },
        { 'L', 50 },
        { 'C', 100 },
        { 'D', 500 },
        { 'M', 1000 },
        /*
        Extended Roman for larger numbers, unicode is hating me for the macrons
        { 'V̅', 5 * 1000},
        { 'X̄', 10 * 1000 },
        { 'L̅', 50 * 1000 },
        { 'C̄', 100 * 1000 },
        { 'D̅', 500 * 1000 },
        { 'M̅', 1000 * 1000 },
        */
    };

    public override int Parse(string value) {
        if (string.IsNullOrEmpty(value))
            return 0;

        var roman = value.ToUpper(); 

        int total = 0;
        int last = 0;
        for (int i = roman.Length - 1; i >= 0; i--) {
            int new_value;
            if (charSet.TryGetValue(roman[i], out new_value)) {
                // See if we should add or subtract.
                if (new_value < last)
                    total -= new_value;
                else {
                    total += new_value;
                    last = new_value;
                }

            } else {
                throw new ArgumentException($"Character '{roman[i]}' doesn't exist in the Roman number system");
            }
        }

        if (total >= 4000) {
            throw new ArgumentException("Roman numerals can only represent numbers up to 3999");
        }

        return total;
    }

    public override string ToString(int value) {
        if (value == 0 || value < 0) {
            return string.Empty;
        }
        if (value >= 4000) {
            throw new ArgumentException("Roman numerals can only represent numbers up to 3999");
        }


        StringBuilder result = new StringBuilder();

        string[] ThouLetters = 
            { "", "M", "MM", "MMM" };
        string[] HundLetters =
            { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        string[] TensLetters =
            { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
        string[] OnesLetters =
            { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };

        // Pull out thousands.
        int num;
        num = value / 1000;
        result.Append(ThouLetters[num]);
        value %= 1000;

        // Handle hundreds.
        num = value / 100;
        result.Append(HundLetters[num]);
        value %= 100;

        // Handle tens.
        num = value / 10;
        result.Append(TensLetters[num]);
        value %= 10;

        // Handle ones.
        result.Append(OnesLetters[value]);

        return result.ToString();
    }
}

}