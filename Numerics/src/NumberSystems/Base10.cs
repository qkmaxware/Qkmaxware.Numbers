using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 10 number system
/// </summary>
public class Decimal : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}