using Huffman;
using System.Collections;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestMethod()
        {
            Compress cmp = new Compress("test", out List<byte> cmpTree, out byte[] cmpData, out byte[] cmpRawData, out List<int> cmpBitArray);
            Decompress dcmp = new Decompress("test.hf", out List<byte> dcmpTree, out byte[] dcmpData, out byte[] dcmpRawData, out BitArray dcmpBitArray);
            TreeCompare(cmpTree, dcmpTree);
            DataCompare(cmpData, dcmpData);
            RawDataCompare(cmpRawData, dcmpRawData);
            BitStringCompare(cmpBitArray, dcmpBitArray);

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

        public void BitStringCompare(List<int> cmpBitString, BitArray dcmpBitArray) 
        {
            for (int i = 0; i < cmpBitString.Count; i++)
            {
                Assert.AreEqual(cmpBitString[i], dcmpBitArray[i] == true ? 1 : 0);
            }
        }
    }
}
