using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 9 number system
/// </summary>
public class Nonary : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3', '4', '5', '6', '7', '8'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}