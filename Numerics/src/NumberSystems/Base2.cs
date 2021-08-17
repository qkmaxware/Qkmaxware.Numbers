using System;

namespace Qkmaxware.Numbers.NumberSystems {


/// <summary>
/// Base 2 number system
/// </summary>
public class Binary : BaseNumberSystem {
    private static char[] charSet = new char[]{
        '0', '1'
    };

    protected override char[] GetCharacterSet() {
        return charSet;
    }
}

}