using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 27 number system
/// </summary>
public class Septemvigesimal : BaseNumberSystem {
    private static char[] charSet = new char[]{ 
        ' ', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }

    protected override string PrepareInput(string raw) {
        return raw.ToLower();
    }
}

}