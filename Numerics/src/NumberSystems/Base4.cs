using System;

namespace Qkmaxware.Numbers.NumberSystems {

/// <summary>
/// Base 4 number system
/// </summary>
public class Quaternary : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1', '2', '3'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}