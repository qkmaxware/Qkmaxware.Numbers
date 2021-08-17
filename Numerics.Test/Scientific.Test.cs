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
            Assert.AreEqual(1.42, v1.Significand);
            Assert.AreEqual(1, v1.Exponent);

            Scientific v2 = new Scientific(0.05);
            Assert.AreEqual(5, v2.Significand);
            Assert.AreEqual(-2, v2.Exponent);

            Scientific v3 = new Scientific(-14.2);
            Assert.AreEqual(-1.42, v3.Significand);
            Assert.AreEqual(1, v3.Exponent);

            Scientific v4 = new Scientific(-0.05);
            Assert.AreEqual(-5, v4.Significand);
            Assert.AreEqual(-2, v4.Exponent);
        }

        [TestMethod]
        public void TestAbs() {
            Scientific v1 = new Scientific(-145);
            Scientific a1 = v1.Abs();
            Assert.AreEqual(1, a1.Sign);
            Assert.AreEqual(true, a1.Significand > 0);
        }

        [TestMethod]
        public void TestArithmetic() {
            Scientific v1 = new Scientific(14.5, 1);
            Scientific v2 = new Scientific(1.24, 2);
            
            Scientific add1 = v1 + v2;
            Assert.AreEqual(2.69, add1.Significand, 0.00005);
            Assert.AreEqual(2, add1.Exponent);

            Scientific sub1 = v1 - v2;
            Assert.AreEqual(2.1, sub1.Significand, 0.00005);
            Assert.AreEqual(1, sub1.Exponent);

            Scientific impl = 360d;
            Assert.IsNotNull(impl);
            Assert.AreEqual(3.6, impl.Significand);
            Assert.AreEqual(2, impl.Exponent);

            Scientific div1 = new Scientific(1000, 0) / impl;
            Assert.IsNotNull(div1);
            Assert.AreEqual(0, div1.Exponent);
            Assert.AreEqual(2.7, (double)div1, 0.1f);

            Scientific floor1 = div1.Floor();
            Assert.IsNotNull(floor1);

            Scientific mul1 = floor1 * 360;
            Assert.IsNotNull(mul1);
        }
    }
}
