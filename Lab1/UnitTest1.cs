using dva246_lab1;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            succ_Pred();
            searchTest();
            min_Max();
            unionTest();
            insertTest();
        }
        private void insertTest()
        {
            OrderedSet<int> sets = new OrderedSet<int>();

            Assert.AreEqual(0, sets.Count);
            sets.Insert(10);
            Assert.AreEqual(1, sets.Count);
            sets.Insert(10);
            Assert.AreEqual(1, sets.Count);
        }
        private void unionTest()
        {
            OrderedSet<int> sets = new OrderedSet<int>();
            OrderedSet<int> sets2 = new OrderedSet<int>();
            sets.UnionWith(sets2);  // This should not throw an error

            sets2.Insert(10);
            sets.UnionWith(sets2);

            Assert.IsTrue(sets.Search(10));

            for(int i = 0; i < 100000; i++)
            {
                sets.Insert(i);
            }

            sets2.UnionWith(sets);
            Assert.AreEqual(100000, sets2.Count);

            for(int i = 100000; i < 200000; i++)
            {
                sets.Insert(i);
            }
            sets2.UnionWith(sets);
            Assert.AreEqual(200000, sets2.Count);

            // Assure that no duplicates are added
            Assert.AreEqual(200000, sets.Count);
            sets.UnionWith(sets2);
            Assert.AreEqual(200000, sets.Count);
        }

        private void min_Max() 
        {
            OrderedSet<int> sets = new OrderedSet<int>();

            Assert.AreEqual(null, sets.Maximum());
            Assert.AreEqual(null, sets.Minimum());

            sets.Insert(10);
            Assert.AreEqual(10, sets.Minimum());
            Assert.AreEqual(10, sets.Maximum());

            sets.Insert(20);
            Assert.AreEqual(10, sets.Minimum());
            Assert.AreEqual(20, sets.Maximum());

            sets.Insert(5);
            Assert.AreEqual(5, sets.Minimum());
            Assert.AreEqual(20, sets.Maximum());
        }

        private void searchTest()
        {
            OrderedSet<int> sets = new OrderedSet<int>();

            sets.Insert(10);
            sets.Insert(20);

            Assert.IsTrue(sets.Search(10));
            Assert.IsTrue(sets.Search(20));
            Assert.IsFalse(sets.Search(11));
        }

        private void succ_Pred()
        {
            OrderedSet<int> sets = new OrderedSet<int>();

            for (int i = 0; i <= 1000; i++)
            {
                sets.Insert(i);
            }
            for (int i = 999; i >= 1; i--)
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

            Assert.AreEqual(null, sets.Predecessor(1001));
            Assert.AreEqual(null, sets.Successor(-1));
        }
    }
}
