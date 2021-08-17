using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qkmaxware.Numbers {

[TestClass]
public class NumberSystemTest {
    [TestMethod]
    public void TestBinary() {
        var system = new NumberSystems.Binary();
        var values = new int[] { 
            0,
            1,
            2,
            3, 
            4,
            14,
        };
        var strings = new string[] {
            "0",
            "1",
            "10",
            "11",
            "100",
            "1110",
        };
        for (var i = 0; i < values.Length; i++) {
            Assert.AreEqual(strings[i], values[i].ToString(system));
            Assert.AreEqual(values[i], system.Parse(strings[i]));
        }

    }
    [TestMethod]
    public void TestHex() {
        var system = new NumberSystems.Hexadecimal();
        var values = new int[] { 
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20
        };
        var strings = new string[] {
            "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f", "10", "11", "12", "13", "14"
        };
        for (var i = 0; i < values.Length; i++) {
            Assert.AreEqual(strings[i], values[i].ToString(system));
            Assert.AreEqual(values[i], system.Parse(strings[i]));
        }
    }

    [TestMethod]
    public void TestRoman() {
        var system = new NumberSystems.Roman();
        var values = new int[] { 
            1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 20, 30, 40, 50, 60, 70, 80, 90, 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 1001
        };
        var strings = new string[] {
            "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM", "M", "MI"
        };
        for (var i = 0; i < values.Length; i++) {
            Assert.AreEqual(strings[i], values[i].ToString(system));
            Assert.AreEqual(values[i], system.Parse(strings[i]));
        }
    }
}

}