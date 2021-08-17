using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 8 number system
/// </summary>
public class Octal : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3', '4', '5', '6', '7'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}