using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qkmaxware.Numbers {

[TestClass]
public class NumberComparisonsTest {
    [TestMethod]
    public void TestArithmetic() {
        // Standard double arithmetic
        var resultDouble = 149598073 * Math.Sqrt(1 - 0.01671022*0.01671022);
        Assert.AreEqual(149577185.30, resultDouble, 0.01);

        // Scientific arithmetic
        var resultScientific = new Scientific(149598073) * new Scientific(1 - 0.01671022*0.01671022).Sqrt();
        Assert.AreEqual(149577185.30, (double)resultScientific, 0.01);

        // Arbitrary arithmetic
        var resultArbitrary = new Arbitrary(149598073) * ((Arbitrary)(1 - 0.01671022*0.01671022)).Sqrt();
        Assert.AreEqual(149577185.30, (double)resultScientific, 0.01);
    }

    [TestMethod]
    public void TestAdditions() {
        double d = 0;
        Scientific s = 0;
        Arbitrary a = 0;

        Stopwatch timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            d ++;
        }
        Console.WriteLine($"[Double] Addition {timer.ElapsedMilliseconds}ms");

        timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            s += 1;
        }
        Console.WriteLine($"[Scientific] Addition {timer.ElapsedMilliseconds}ms");

        timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            a += 1;
        }
        Console.WriteLine($"[Arbitrary] Addition {timer.ElapsedMilliseconds}ms");
    }

    [TestMethod]
    public void TestMultiplications() {
        double d = 0;
        Scientific s = 0;
        Arbitrary a = 0;

        Stopwatch timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            var temp = d * 2;
        }
        Console.WriteLine($"[Double] Multiplication {timer.ElapsedMilliseconds}ms");

        timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            var temp = s * 1;
        }
        Console.WriteLine($"[Scientific] Multiplication {timer.ElapsedMilliseconds}ms");

        timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            var temp = a * 2;
        }
        Console.WriteLine($"[Arbitrary] Multiplication {timer.ElapsedMilliseconds}ms");
    }

    [TestMethod]
    public void TestDivision() {
        double d = 0;
        Scientific s = 0;
        Arbitrary a = 0;

        Stopwatch timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            var temp = d / 2;
        }
        Console.WriteLine($"[Double] Division {timer.ElapsedMilliseconds}ms");

        timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            var temp = s / 1;
        }
        Console.WriteLine($"[Scientific] Division {timer.ElapsedMilliseconds}ms");

        timer = Stopwatch.StartNew();
        for (var i = 0; i < 10000; i++) {
            var temp = a / 2;
        }
        Console.WriteLine($"[Arbitrary] Division {timer.ElapsedMilliseconds}ms");
    }
}    

}