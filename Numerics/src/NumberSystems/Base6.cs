using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 6 number system
/// </summary>
public class Senary : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3', '4', '5'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}