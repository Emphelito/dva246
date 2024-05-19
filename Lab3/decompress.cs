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
        private Dictionary<byte, List<byte>> keyValuePairs;

        private Node root = new Node();
        public Decompress(string fileName)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            foreach (byte b in data)
            {
                Console.Write(b);
            }

            ByteToBit();

            BuildTree(0, root);
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
            treeStucture = new byte[i];
            Array.Copy(data, 1, treeStucture, 0, i);
            bitArray = new BitArray(data);


            keyValuePairs = new Dictionary<byte, List<byte>>();
            List<byte> path = new List<byte>();

            foreach (byte b in treeStucture)
            {
                if (b != 0 && b != 1)
                {                    
                    keyValuePairs.Add(b, path);
                      
                    path = new List<byte>();
                }
                else
                {
                    path.Add(b);
                }
            }
        }
        private int BuildTree(int index, Node current)
        {  
            if (treeStucture[index] == 1)
            {
                current.value = treeStucture[index];
                index += 2;
                return index;
            }
            else
            {
                index++;
                Node left = new Node();
                current.left = left;
                index = BuildTree(index, current.left);

                Node right = new Node();
                current.right = right;
                index = BuildTree(index, current.right);
            }
            return index;
        }
    }
}
