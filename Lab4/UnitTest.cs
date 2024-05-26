namespace LCS
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            LCS lcs = new LCS();
            Assert.AreEqual(4, lcs.CalcLCS("abcdefgh", "defh"));
            Assert.AreEqual(0, lcs.CalcLCS("abc", "def"));
            Assert.AreEqual(4, lcs.CalcLCS("ABCD", "ABCD"));
            Assert.AreEqual(3, lcs.CalcLCS("XYZ", "XAYBZ"));
            Assert.AreEqual(4, lcs.CalcLCS("banana", "atana"));
            Assert.AreEqual(4, lcs.CalcLCS("workattech", "branch"));
            Assert.AreEqual(5, lcs.CalcLCS("playword", "helloworld"));
            Assert.AreEqual(0, lcs.CalcLCS("abc", "def"));
        }
    }
}