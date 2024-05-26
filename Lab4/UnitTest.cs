namespace LCS
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            LCS lcs = new LCS();
            Assert.AreEqual(4, lcs.CalcLCS("abcdefgh", "defh"));            // defh     : 4
            Assert.AreEqual(4, lcs.CalcLCS("ABCD", "ABCD"));                // ABCD     : 4
            Assert.AreEqual(3, lcs.CalcLCS("XYZ", "XAYBZ"));                // XYZ      : 3
            Assert.AreEqual(4, lcs.CalcLCS("banana", "atana"));             // aana     : 4
            Assert.AreEqual(4, lcs.CalcLCS("workattech", "branch"));        // rach     : 4
            Assert.AreEqual(5, lcs.CalcLCS("playword", "helloworld"));      // lword    : 5
            Assert.AreEqual(0, lcs.CalcLCS("abc", "def"));                  // []       : 0
            Assert.AreEqual(2, lcs.CalcLCS("abba", "baab"));                // ab       : 2
        }
    }
}