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
        private Dictionary<string, byte> encodedTable;
        private string bitString;
        private List<byte> content;

        private Node root = new Node();

        public Decompress(string fileName, out List<byte> utTree, out byte[] utData, out byte[] utRawData)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            ByteToBit();

            utData = data;

            BuildTree(root, 0);

            unitTestTree = new List<byte>();
            printTree(root);
            utTree = unitTestTree;

            encodedTable = new Dictionary<string, byte>();
            DecodeTable(root, "");

            content = new List<byte>();
            DecodeData(root);

            // Unit Test Raw Data
            utRawData = content.ToArray();

            fh.Write(content.ToArray());
        }

        public Decompress(string fileName)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            ByteToBit();

            BuildTree(root, 0);

            unitTestTree = new List<byte>();
            printTree(root);

            encodedTable = new Dictionary<string, byte>();
            DecodeTable(root, "");

            content = new List<byte>();
            DecodeData(root);

            fh.Write(content.ToArray());
        }
        private void ByteToBit()
        {
            int i;
            byte bp = data[0];
            keyValuePairs = new Dictionary<byte, string>();
            string pathString = "";

            // Determines where the instructions for how to build the tree ends
            // it also adds all the instructions to a dict
            for (i =1; i < data.Length; i++)
            {
                pathString += data[i];

                if (data[i] == 1)
                {
                    keyValuePairs.Add(data[i + 1], pathString);
                    i++;
                    if (data[i] == bp)
                    {
                        treeDataIndex = i;
                        break;
                    }
                    pathString = "";
                }
            }
            treeDataIndex += 1;

            treeStucture = new byte[i];
            Array.Copy(data, 1, treeStucture, 0, i);
            byte[] dataTmp = new byte[data.Length - treeDataIndex];
            Array.Copy(data, treeDataIndex, dataTmp, 0, data.Length - treeDataIndex);
            data = dataTmp;

            bitString = "";
            i = 0;
            foreach (var b in dataTmp)
            {
                i++;
                bitString += Convert.ToString(b, 2).PadLeft(8, '0');
                if(i == 7040)
                {
                    Console.WriteLine();
                }
            }
            var tmp = bitString.Length;
        }
        private int BuildTree(Node current, int index)
        {
            if (treeStucture[index] == 1)
            {
                current.value = treeStucture[index+ 1];
                return index + 1;
            }
            if(current.left == null) 
            {
                Node left = new Node();
                current.left = left;
            }            
            index = BuildTree(current.left, index + 1);
            if(current.right == null)
            {
                Node right = new Node();
                current.right = right;
            }
            index = BuildTree(current.right, index + 1);

            return index;
        }

        // Creates a dict that will be used when decoding the data
        private void DecodeTable(Node current, string path)
        {
            if(current.left == null && current.right == null)
            {
                encodedTable.Add(path, current.value);
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
                        content.Add(encodedTable[pathString]);
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
                        content.Add(encodedTable[pathString]);
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
