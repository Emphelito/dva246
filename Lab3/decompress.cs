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
        private BitArray bitArray;
        private int treeDataIndex;
        private byte[] treeStucture;
        private Dictionary<string, byte> keyValuePairs;
        private string bitString;
        private List<byte> content;

        private Node root = new Node();
        public Decompress(string fileName)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            ByteToBit();

            BuildTree(root);

            content = new List<byte>();
            DecodeData(root);

            fh.Write(content.ToArray());
        }
        private void ByteToBit()
        {
            int i;
            byte bp = data[0]; 
            for(i =1; i < data.Length; i++)
            {
                if (data[i] == bp)
                {
                    treeDataIndex = i;
                    break;
                }
            }
            treeDataIndex += 1;

            treeStucture = new byte[i];
            Array.Copy(data, 1, treeStucture, 0, i);
            byte[] dataTmp = new byte[data.Length - treeDataIndex];
            Array.Copy(data, treeDataIndex, dataTmp, 0, data.Length - treeDataIndex);
            bitArray = new BitArray(dataTmp);

            bitString = "";

            foreach (var b in dataTmp)
            {
                bitString += Convert.ToString(b, 2).PadLeft(8, '0');
            }

            keyValuePairs = new Dictionary<string, byte>();
            string pathString = "";

            foreach (byte b in treeStucture)
            {
                if (b != 0 && b != 1)
                {                    
                    keyValuePairs.Add(pathString, b);

                    pathString = "";
                }
                else
                {
                    pathString += b.ToString();
                }
            }
        }
        private void BuildTree(Node current)
        {
            foreach(var n in treeStucture)
            {
                if (n == 1)
                {
                    if (current.right == null)
                    {
                        Node right = new Node();
                        current.right = right;
                    }
                    current = current.right;
                }
                else if (n == 0)
                {
                    if (current.left == null)
                    {
                        Node left = new Node();
                        current.left = left;
                    }
                    current = current.left;
                }
                else
                {
                    current.value = n;
                    current = root;
                }
            }
        }

        private void DecodeData(Node current)
        {
            string tmp = "";
            string pathString = "";
            for(int i = 0; i < bitString.Length; i++)
            {
                tmp += bitString[i];
                if (bitString[i].Equals('0'))
                {
                    if(current.left == null)
                    {
                        current = root;
                        content.Add(keyValuePairs[pathString]);
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
                    if(current.right == null)
                    {
                        current = root;
                        content.Add(keyValuePairs[pathString]);
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
