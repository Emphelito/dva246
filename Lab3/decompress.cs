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
        private Dictionary<byte, string> keyValuePairs;
        private Dictionary<string, byte> decodedTable;
        private string bitString;
        private List<byte> content;
        private int rawLength;

        private Node root = new Node();

        public Decompress(string fileName, out List<byte> utTree, out byte[] utData, out byte[] utRawData, out string utBitString)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            // Convert the read bytes to bits
            int tmp = BuildTree(root, 0);
            ByteToBit();

            // Unit Test Data
            utData = data;
            utBitString = bitString;

            

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
        private void ByteToBit()
        {
            int i;

            byte[] dataLength = new byte[4];
            Array.Copy(data, dataLength, 4);
            if(!BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataLength);
            }
            rawLength = BitConverter.ToInt32(dataLength, 0);
            
            byte bp = data[4];

            // Determines where the instructions for how to build the tree ends
            // it also adds all the instructions to a dict
            for (i = 5; i < data.Length; i++)
            {
                if (data[i] == 1)
                {
                    i++;
                    if (data[i] == bp)
                    {
                        treeDataIndex = i;
                        break;
                    }
                }
            }
            treeDataIndex += 1;

            treeStucture = new byte[i];
            Array.Copy(data, 5, treeStucture, 0, i);
            byte[] dataTmp = new byte[data.Length - treeDataIndex];
            Array.Copy(data, treeDataIndex, dataTmp, 0, data.Length - treeDataIndex);
            data = dataTmp;

            bitString = "";
            StringBuilder sb = new StringBuilder(data.Length * 8);
            foreach (var b in data)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            bitString = sb.ToString();
        }
        private int BuildTree(Node current, int index)
        {
            if (data[index] == 1)
            {
                current.value = data[index+ 1];
                return index + 1;
            }
            else if (data[index] != 0 && data[index] != 1)
            {
                Console.WriteLine("Hello");
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
            for (int i = 0; i < bitString.Length; i++)
            {
                if (bitString[i].Equals('0'))
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
                        pathString += bitString[i];
                        current = current.left;
                    }
                }
                else if (bitString[i].Equals('1'))
                {
                    if (current.right == null)
                    {
                        current = root;
                        content.Add(decodedTable[pathString]);
                        pathString = "";
                        i--;
                    }
                    else
                    {
                        pathString += bitString[i];
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
