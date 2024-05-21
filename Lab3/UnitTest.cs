using Huffman;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            Compress cmp = new Compress("test", out List<byte> cmpTree, out byte[] cmpData, out byte[] cmpRawData);
            Decompress dcmp = new Decompress("test.hf", out List<byte> dcmpTree, out byte[] dcmpData, out byte[] dcmpRawData);
            TreeCompare(cmpTree, dcmpTree);
            DataCompare(cmpData, dcmpData);
           // RawDataCompare(cmpRawData, dcmpRawData);
        }

        public void TreeCompare(List<byte> cmpTree, List<byte> dcmpTree)
        {
            for (int i = 0; i < cmpTree.Count; i++)
            {
                Assert.AreEqual(cmpTree[i], dcmpTree[i]);
            }
        }

        public void DataCompare(byte[] cmpData, byte[] dcmpData) 
        {
            for(int i = 0;i < cmpData.Length;i++)
            {
                Assert.AreEqual(cmpData[i], dcmpData[i]);
            }
        }

        public void RawDataCompare(byte[] cmpRawData, byte[] dcmpRawData)
        {
            for(int i = 0;i < cmpRawData.Length; i++)
            {
                Assert.AreEqual(cmpRawData[i], dcmpRawData[i]);
            }
        }
    }
}
