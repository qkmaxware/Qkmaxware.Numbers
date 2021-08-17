using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 5 number system
/// </summary>
public class Quinary : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3', '4'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}