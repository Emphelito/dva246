using dva246_lab1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        static int test = 500;
        [TestMethod]
        public void TestMethod1()
        {
            OrderedSet<int> sets = new OrderedSet<int>();

            for (int i = 0; i <= 1000; i++)
            {
                sets.Insert(i);
            }
            for(int i = 999;i >= 1; i--)
            {
                Assert.AreEqual(i + 1, sets.Successor(i));
                Assert.AreEqual(i - 1, sets.Predecessor(i));
            }
            Assert.AreEqual(null, sets.Successor(1000));
            Assert.AreEqual(null, sets.Predecessor(0));

            for (int i = 1000; i >= 0; i--)
            {
                sets.Insert(i);
            }
            for (int i = 1; i <= 999; i++)
            {
                Assert.AreEqual(i + 1, sets.Successor(i));
                Assert.AreEqual(i - 1, sets.Predecessor(i));
            }
            Assert.AreEqual(null, sets.Successor(1000));
            Assert.AreEqual(null, sets.Predecessor(0));
        }
    }
}
