using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 3 number system
/// </summary>
public class Ternary : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}