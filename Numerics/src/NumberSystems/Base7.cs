using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 7 number system
/// </summary>
public class Septenary : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3', '4', '5', '6'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}