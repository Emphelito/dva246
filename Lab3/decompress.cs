using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    internal class Decompress
    {
        private byte[] data;
        private int treeDataIndex;
        private byte[] treeStucture;
        private Dictionary<byte, string> keyValuePairs;
        private Dictionary<string, byte> encodedTable;
        private string bitString;
        private List<byte> content;

        private Node root = new Node();
        public Decompress(string fileName)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            ByteToBit();

            BuildTree(root, 0);

            Console.WriteLine();
            main m = new main();
            //m.printTree(root);

            encodedTable = new Dictionary<string, byte>();
            DecodeTable(root, "");

            content = new List<byte>();
            DecodeData(root);

            fh.Write(content.ToArray());
        }
        private void ByteToBit()
        {
            int i;
            byte prev = 0;
            byte bp = data[0];
            keyValuePairs = new Dictionary<byte, string>();
            string pathString = "";
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

            bitString = "";

            foreach (var b in dataTmp)
            {
                bitString += Convert.ToString(b, 2).PadLeft(8, '0');
            }
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

        private void DecodeTable(Node current, string path)
        {
            if(current.left == null && current.right == null)
            {
                encodedTable.Add(path, current.value);
                //path = "";
                return;

            }
            string lpath = path + "0";
            string rpath = path + "1";
            DecodeTable(current.left, lpath);
            DecodeTable(current.right, rpath);
            
        }
        private void DecodeData(Node current)
        {
            string tmp = "";
            string pathString = "";
            for (int i = 0; i < bitString.Length; i++)
            {
                tmp += bitString[i];
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
    }
}
