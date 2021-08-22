using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qkmaxware.Numbers {

[TestClass]
public class ArbitraryTest {
    [TestMethod]
    public void TestConstuctor() {
        var real = new Arbitrary(144, -2);

        Assert.AreEqual(144, real.Significand);
        Assert.AreEqual(-2, real.Exponent);
        Assert.AreEqual("144E-2", real.ToString());
        Assert.AreEqual(1.44, (double)real, 0.00001);
    }

    [TestMethod]
    public void TestConvertInt() {
        Arbitrary real = 8;

        Assert.AreEqual(8, real.Significand);
        Assert.AreEqual(0, real.Exponent);
        Assert.AreEqual("8E0", real.ToString());
        Assert.AreEqual(8, (int)real, 0.00001);
    }

    [TestMethod]
    public void TestConvertDouble() {
        Arbitrary real = 1.44;

        Assert.AreEqual(144, real.Significand);
        Assert.AreEqual(-2, real.Exponent);
        Assert.AreEqual("144E-2", real.ToString());
        Assert.AreEqual(1.44, (double)real, 0.00001);
    }

    [TestMethod]
    public void TestPrecision() {
        var number = new Arbitrary(123_45, -2);
        Assert.AreEqual(5, number.Precision);

        number = new Arbitrary(12_34, -2);
        Assert.AreEqual(4, number.Precision);

        number = new Arbitrary(12345, 0);
        Assert.AreEqual(5, number.Precision);

        number = new Arbitrary(123_4, -1);
        Assert.AreEqual(4, number.Precision);

        number = new Arbitrary(0, 0);
        Assert.AreEqual(0, number.Precision);

        number = new Arbitrary(12_345, -3);
        Assert.AreEqual(5, number.Precision);

        number = new Arbitrary(1234_56, -2);
        Assert.AreEqual(6, number.Precision);

        number = new Arbitrary(123_456, -3);
        Assert.AreEqual(6, number.Precision);

        number = new Arbitrary(123450, 0);
        Assert.AreEqual(6, number.Precision);
    }

    [TestMethod]
    public void TestScale() {
        var number = new Arbitrary(123_45, -2);
        Assert.AreEqual(2, number.Scale);

        number = new Arbitrary(12_34, -2);
        Assert.AreEqual(2, number.Scale);

        number = new Arbitrary(12345, 0);
        Assert.AreEqual(0, number.Scale);

        number = new Arbitrary(123_4, -1);
        Assert.AreEqual(1, number.Scale);

        number = new Arbitrary(0, 0);
        Assert.AreEqual(0, number.Scale);

        number = new Arbitrary(12_345, -3);
        Assert.AreEqual(3, number.Scale);

        number = new Arbitrary(1234_56, -2);
        Assert.AreEqual(2, number.Scale);

        number = new Arbitrary(123_456, -3);
        Assert.AreEqual(3, number.Scale);

        number = new Arbitrary(123450, 0);
        Assert.AreEqual(0, number.Scale);
    }

    [TestMethod]
    public void TestAdd() {
        Arbitrary first = new Arbitrary(82, -1);
        Arbitrary second = new Arbitrary(144, -2);

        var result = first + second;

        Assert.AreEqual(964, result.Significand);
        Assert.AreEqual(-2, result.Exponent); 
    }

    [TestMethod]
    public void TestSubtract() {
        Arbitrary first = new Arbitrary(82, -1);
        Arbitrary second = new Arbitrary(144, -2);

        var result = first - second;

        Assert.AreEqual(676, result.Significand);
        Assert.AreEqual(-2, result.Exponent); 
    }

    [TestMethod]
    public void TestMultiply() {
        Arbitrary first = new Arbitrary(82, -1);
        Arbitrary second = new Arbitrary(144, -2);

        var result = first * second;

        Assert.AreEqual(11808, result.Significand);
        Assert.AreEqual(-3, result.Exponent); 
    }

    [TestMethod]
    public void TestDivideAutoScale() {
        Arbitrary first = new Arbitrary(82, -1);
        Arbitrary second = new Arbitrary(144, -2);

        var result = first / second;

        Assert.AreEqual(5694, result.SetPrecision(4).Significand);
        Assert.AreEqual(-3, result.SetPrecision(4).Exponent); 
    }

    [TestMethod]
    public void TestDivideManualScale() {
        var divisor = new Arbitrary(3, 0);
        Assert.AreEqual(0, divisor.Scale);
        var decimals = 25; // Decimals of precision (at most 25 decimals)

        // Small numerator, result should be all decimals
        var numerator = new Arbitrary(1, 0); //1
        Assert.AreEqual(0, numerator.Scale);
        var result = numerator.Divide(divisor, decimals);   // 0.333...
        Assert.AreEqual(25, result.Significand.ToString().Length);
        Assert.AreEqual(decimals, result.Scale);

        // Larger numbers, result should have some whole parts
        numerator = new Arbitrary(1, 1); // 10
        Assert.AreEqual(0, numerator.Scale);
        result = numerator.Divide(divisor, decimals);       // 3.333...
        Assert.AreEqual(26, result.Precision);
        Assert.AreEqual(decimals, result.Scale);

        numerator = new Arbitrary(1, 2); //100
        Assert.AreEqual(0, numerator.Scale);
        result = numerator.Divide(divisor, decimals);       // 33.333...
        Assert.AreEqual(27, result.Precision);
        Assert.AreEqual(decimals, result.Scale);
    }

    [TestMethod]
    public void TestTruncate() {
        Arbitrary first = new Arbitrary(144, -2); // 1.44
        
        var trunc = first.Truncate();
        Assert.AreEqual(1, trunc.Significand);
        Assert.AreEqual(0, trunc.Exponent);

        first = new Arbitrary(-27, -1);    // -2.7
        trunc = first.Truncate();
        Assert.AreEqual(-2, trunc.Significand);
        Assert.AreEqual(0, trunc.Exponent);

        first = new Arbitrary(125, -3);    // 0.125
        trunc = first.Truncate();
        Assert.AreEqual(0, trunc.Significand);
        Assert.AreEqual(0, trunc.Exponent);

        first = 0;              // 0
        trunc = first.Truncate();
        Assert.AreEqual(0, trunc.Significand);
        Assert.AreEqual(0, trunc.Exponent);
    }

    [TestMethod]
    public void TestFloor() {
        var values = new double[] { 2, 2.4, 2.9, -2.7, -2, 0.125, 0 };
        var floor  = new int[]    { 2, 2,   2,   -3,   -2, 0, 0 };

        for (var i = 0; i < values.Length; i++) {
            Arbitrary real = values[i];
            var floored = real.FloorToInt();

            Assert.AreEqual(values[i], (double)real, 0.0001);
            Assert.AreEqual(floor[i], floored);
        }
    }

    [TestMethod]
    public void TestCeil() {
        var values = new double[] { 2, 2.4, 2.9, -2.7, -2, 0.127, 0 };
        var ceil   = new int[]    { 2, 3,   3,   -2,   -2, 1, 0 };

        for (var i = 0; i < values.Length; i++) {
            Arbitrary real = values[i];
            var floored = real.CeilToInt();

            Assert.AreEqual(values[i], (double)real, 0.0001);
            Assert.AreEqual(ceil[i], floored);
        }
    }

    [TestMethod]
    public void TestSqrt() {
        var values = new double[] { 144, 196, 25, 1000000, 3136, 5625, 16, 576, 0.09, 0.25, 0.0025 };
        var sqrt = new double[] { 12, 14, 5, 1000, 56, 75, 4, 24, 0.3, 0.5, 0.05 };

        for (var i = 0; i < values.Length; i++) {
            Arbitrary real = values[i];
            var sqrted = real.Sqrt();

            Assert.AreEqual(values[i], (double)real, 0.0001);
            Assert.AreEqual(sqrt[i], sqrted);
        }
    }

}

}