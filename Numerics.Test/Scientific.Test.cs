using Qkmaxware.Numbers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Numerics.Test {
    [TestClass]
    public class TestReal {
        [TestMethod]
        public void TestConstructor() {
            Scientific v1 = new Scientific(32);
            Assert.AreEqual(32, ((double)v1));
            Assert.AreEqual(3.2, v1.Significand);
            Assert.AreEqual(1, v1.Exponent);
            Scientific v2 = new Scientific(14, 0);
            Assert.AreEqual(14, ((double)v2));
            Assert.AreEqual(1.4, v2.Significand);
            Assert.AreEqual(1, v2.Exponent);
            Scientific v3 = (18.0).x10(2);
            Assert.AreEqual(1800, ((double)v3));
            Assert.AreEqual(1.8, v3.Significand);
            Assert.AreEqual(3, v3.Exponent);

            Scientific v4 = (12.52).x10(2);
            Assert.AreEqual(1252, (double)v4);
            Assert.AreEqual(1.252, v4.Significand);
            Assert.AreEqual(3, v4.Exponent);
            Scientific v5 = (12.52).x10(-2);
            Assert.AreEqual(0.1252, (double)v5);
            Assert.AreEqual(1.252, v5.Significand);
            Assert.AreEqual(-1, v5.Exponent);
        
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

        [TestMethod]
        public void Testx10() {
            var s1 = new Scientific(1, 0);
            Assert.AreEqual(1, s1.Significand);
            Assert.AreEqual(0, s1.Exponent);

            var s2 = s1.x10(1);
            Assert.AreEqual(1, s2.Significand);
            Assert.AreEqual(1, s2.Exponent);

            var s3 = s2.x10(1);
            Assert.AreEqual(1, s3.Significand);
            Assert.AreEqual(2, s3.Exponent);

            var s4 = s3.x10(1);
            Assert.AreEqual(1, s4.Significand);
            Assert.AreEqual(3, s4.Exponent);

            var s5 = s1.x10(3);
            Assert.AreEqual(1, s5.Significand);
            Assert.AreEqual(3, s5.Exponent);
        }

        [TestMethod]
        public void TestLarge() {
            Scientific AUs = 1;
            Assert.AreEqual(1, AUs.Significand);
            Assert.AreEqual(0, AUs.Exponent);

            Scientific Au2Km = 149598073;
            Assert.AreEqual(1.49598073, Au2Km.Significand);
            Assert.AreEqual(8, Au2Km.Exponent);

            Scientific kms = AUs * Au2Km;
            Assert.AreEqual(Au2Km.Significand, kms.Significand);
            Assert.AreEqual(Au2Km.Exponent, kms.Exponent);

            var ms = kms * new Scientific(1, 3);
            Assert.AreEqual(149598073000, (double)ms);
        }

        [TestMethod]
        public void TestVeryLarge() {
            Scientific lys = 1;
            Assert.AreEqual(1, lys.Significand);
            Assert.AreEqual(0, lys.Exponent);

            var Lightyears2Km = new Scientific(9460730472580800, -3);
            Assert.AreEqual(9.4607304725808, Lightyears2Km.Significand);
            Assert.AreEqual(12, Lightyears2Km.Exponent);

            Scientific kms = lys * Lightyears2Km;
            Assert.AreEqual(9.4607304725808, kms.Significand);
            Assert.AreEqual(12, kms.Exponent);

            var ms = kms * new Scientific(1, 3);
            Assert.AreEqual(9.4607304725808, ms.Significand);
            Assert.AreEqual(15, ms.Exponent);
        }
    }
}
