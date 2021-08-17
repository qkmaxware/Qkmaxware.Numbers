using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 18 number system
/// </summary>
public class Octodecimal : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }

    protected override string PrepareInput(string raw) {
        return raw.ToLower();
    }
}

}