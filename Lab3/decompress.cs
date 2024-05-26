using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    public class Decompress
    {
        private byte[] data;
        private int treeDataIndex;
        private byte[] treeStucture;
        private Dictionary<string, byte> decodedTable;
        private BitArray bitArray;
        private List<byte> content;
        private int rawLength;

        private Node root = new Node();

        public Decompress(string fileName, out List<byte> utTree, out byte[] utData, out byte[] utRawData, out BitArray utBitArray)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            // Gets the first 4 bytes that represent data length after decode
            SegmentData();

            treeDataIndex = BuildTree(root, 0);

            ByteToBit();

            // Unit Test Data
            utData = data;
            utBitArray = bitArray;  

            // Unit Test Tree
            unitTestTree = new List<byte>();
            printTree(root);
            utTree = unitTestTree;

            // Create a table used or decoding
            decodedTable = new Dictionary<string, byte>();
            DecodeTable(root, "");

            // Decode the data
            content = new List<byte>();
            DecodeData(root);

            // If an extra char is added remove it     
            while (content.Count > rawLength)
            {
                content.RemoveAt(content.Count - 1);
            }

            // Unit Test Raw Data
            utRawData = content.ToArray();

            fh.Write(content.ToArray());
        }

        public Decompress(string fileName)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            // Gets the first 4 bytes that represent data length after decode
            SegmentData();

            treeDataIndex = BuildTree(root, 0);

            // Convert the read bytes to bits
            ByteToBit();

            BuildTree(root, 0);

            // Create a table used or decoding
            decodedTable = new Dictionary<string, byte>();
            DecodeTable(root, "");

            // Decode the data
            content = new List<byte>();
            DecodeData(root);

            // If an extra char is added remove it
            while (content.Count > rawLength)
            {
                content.RemoveAt(content.Count - 1);
            }

            fh.Write(content.ToArray());
        }
        // This removes the first 4 bytes from "data"
        // They represent the size of the data after decoding
        private void SegmentData()
        {
            byte[] dataLength = new byte[4];
            Array.Copy(data, dataLength, 4);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataLength);
            }
            rawLength = BitConverter.ToInt32(dataLength, 0);

            byte[] dataTmp = new byte[data.Length - 4];
            Array.Copy(data, 4, dataTmp, 0, data.Length - 4);
            data = dataTmp;
        }
        private int BuildTree(Node current, int index)
        {
            if (data[index] == 1)
            {
                current.value = data[index + 1];
                return index + 1;
            }
            else if (data[index] != 0 && data[index] != 1)
            {
                return index;
            }
            Node left = new Node();
            current.left = left;
            index = BuildTree(current.left, index + 1);

            Node right = new Node();
            current.right = right;
            index = BuildTree(current.right, index + 1);

            return index;
        }

        private void ByteToBit()
        {
            treeDataIndex += 1;

            byte[] dataTmp = new byte[data.Length - treeDataIndex];
            Array.Copy(data, treeDataIndex, dataTmp, 0, data.Length - treeDataIndex);
            data = dataTmp;

            bitArray = new BitArray(data.Length * 8);
            string bitString = "";
            for (int i = 0; i < data.Length; i++)
            {
                bitString += Convert.ToString(data[i], 2).PadLeft(8, '0');
                for(int j = 0; j < 8; j++)
                {
                    bitArray.Set((i * 8) + j, bitString[j] == '1' ? true : false);
                }
                bitString = "";
            }
        }

        // Creates a dict that will be used when decoding the data
        private void DecodeTable(Node current, string path)
        {
            if(current.left == null && current.right == null)
            {
                decodedTable.Add(path, current.value);
                return;

            }
            string lpath = path + "0";
            string rpath = path + "1";
            DecodeTable(current.left, lpath);
            DecodeTable(current.right, rpath);
            
        }
        private void DecodeData(Node current)
        {
            string pathString = "";
            for (int i = 0; i < bitArray.Count; i++)
            {
                if (current.left == null)
                {
                    current = root;
                    content.Add(decodedTable[pathString]);
                    pathString = "";
                    i--;
                }
                else
                {
                    pathString += bitArray[i] == true ? '1' : '0';
                    if (!bitArray[i])
                    {
                        current = current.left;
                    }
                    else
                    {
                        current = current.right;
                    }
                }
            }
        }

        // Postorder
        private List<byte> unitTestTree;
        private void printTree(Node current)
        {
            if (current == null)
                return;

            printTree(current.left);
            printTree(current.right);

            unitTestTree.Add(current.value);
        }
    }
}
