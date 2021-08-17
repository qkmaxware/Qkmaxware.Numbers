using Qkmaxware.Numbers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Numerics.Test {
    [TestClass]
    public class TestReal {
        [TestMethod]
        public void TestConstructor() {
            Scientific v1 = new Scientific(32);
            Assert.AreEqual(32, ((double)v1));
            Scientific v2 = new Scientific(14, 0);
            Assert.AreEqual(14, ((double)v2));
            Scientific v3 = (18.0).x10(2);
            Assert.AreEqual(1800, ((double)v3));

            Scientific v4 = (12.52).x10(2);
            Assert.AreEqual(1252, (double)v4);
            Scientific v5 = (12.52).x10(-2);
            Assert.AreEqual(0.1252, (double)v5);
        
            Scientific v6 = new Scientific(-32);
            Assert.AreEqual(-32, ((double)v6));
        }

        [TestMethod]
        public void TestNormalized() {
            Scientific v1 = new Scientific(14.2, 0);
            Scientific n1 = v1.Normalized();
            Assert.AreEqual(1.42, n1.Significand);
            Assert.AreEqual(1, n1.Exponent);

            Scientific v2 = new Scientific(0.05);
            Scientific n2 = v2.Normalized();
            Assert.AreEqual(5, n2.Significand);
            Assert.AreEqual(-2, n2.Exponent);

            Scientific v3 = new Scientific(-14.2);
            Scientific n3 = v3.Normalized();
            Assert.AreEqual(-1.42, n3.Significand);
            Assert.AreEqual(1, n3.Exponent);

            Scientific v4 = new Scientific(-0.05);
            Scientific n4 = v4.Normalized();
            Assert.AreEqual(-5, n4.Significand);
            Assert.AreEqual(-2, n4.Exponent);
        }

        [TestMethod]
        public void TestAbs() {
            Scientific v1 = new Scientific(-145);
            Scientific a1 = v1.Abs();
            Assert.AreEqual(1, a1.Sign);
            Assert.AreEqual(true, a1.Significand > 0);
        }

        [TestMethod]
        public void TestSetExponent() {
            Scientific v1 = new Scientific(145, 0);
            Scientific v2 = v1.SetExponent(1);
            Scientific v3 = v1.SetExponent(2);
            Scientific v4 = v2.SetExponent(2);

            Assert.AreEqual(145, v1.Significand, 0.00005);
            Assert.AreEqual(14.5, v2.Significand, 0.00005);
            Assert.AreEqual(1.45, v3.Significand, 0.00005);
            Assert.AreEqual(1.45, v4.Significand, 0.00005);

            Scientific v5 = new Scientific(1.24, 2).SetExponent(1);
            Assert.AreEqual(12.4, v5.Significand, 0.00005);
        }

        [TestMethod]
        public void TestArithmetic() {
            Scientific v1 = new Scientific(14.5, 1);
            Scientific v2 = new Scientific(1.24, 2);
            
            Scientific add1 = v1 + v2;
            Assert.AreEqual(2.69, add1.Significand, 0.00005);
            Assert.AreEqual(2, add1.Exponent);

            Scientific sub1 = v1 - v2;
            Assert.AreEqual(0.21, sub1.Significand, 0.00005);
            Assert.AreEqual(2, sub1.Exponent);
        }
    }
}
