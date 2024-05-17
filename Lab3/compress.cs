using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    public class Node:IComparable<Node>
    {
        public int frequency;
        public byte value;

        internal Node left = null;
        internal Node right = null;

        public int CompareTo(Node other)
        {
            return frequency - other.frequency;
        }
    }
    public class Compress
    {
        private byte[] data;
        private List<byte> encodedTree;
        private List<byte> encodedData;
        private PriorityQueue<int, Node> priorityQueue;
        Dictionary<byte, List<int>> encodeTable = new Dictionary<byte, List<int>>();

        public Compress(string fileName) 
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            priorityQueue = new PriorityQueue<int,Node>();

            if (!DataToHeap()) throw new Exception($"No data in file: {fileName}");

            HuffmanTree();

            encodedTree = new List<byte>();
            encodedData = new List<byte>();
            List<int> bitArray = new List<int>();

            priorityQueue.TryPeek(out int p, out Node current);
            EncodeTree(current, bitArray);

            EncodeData();
            
            fh.Write(data);
        }

        private bool DataToHeap()
        {
            Dictionary<byte, int> frequency = new Dictionary<byte, int>();

            if (data == null) return false;

            foreach (byte d in data)
            {
                if (!frequency.ContainsKey(d))
                {
                    frequency.Add(d, 1);
                }
                else
                {
                    frequency[d]++;
                }
            }
            foreach (var keypair in frequency)
            {
                priorityQueue.Enqueue(0, new Node { frequency = keypair.Value, value = keypair.Key });
            }

            return true;
                     
        }

        private void HuffmanTree()
        {
            int p;
            while (priorityQueue.Count > 1)
            {
                Node left;
                priorityQueue.TryDequeue(out p, out left);

                Node right; 
                priorityQueue.TryDequeue(out p, out right);

                Node parent = new Node();
                parent.frequency = left.frequency + right.frequency;
                parent.left = left;
                parent.right = right;
                
                priorityQueue.Enqueue(p, parent);
            }
        }

        private void EncodeTree(Node current, List<int> bitPrefix)
        {      
            if (current == null) return;

            if(current.value != 0)
            {
                bitPrefix.Add(1);
                encodeTable.Add(current.value, bitPrefix);
                encodedTree.Add(1);
                encodedTree.Add(current.value);
                return;
            }
            else
            {
                encodedTree.Add(0);
                List<int> leftPrefix = new List<int>();
                leftPrefix.AddRange(bitPrefix);
                leftPrefix.Add(0);
                string binaryString = Convert.ToString(97, 2);
                EncodeTree(current.left, leftPrefix);

                List<int> rightPrefix = new List<int>();
                rightPrefix.AddRange(bitPrefix);
                rightPrefix.Add(1);
                EncodeTree(current.right, rightPrefix);
            }
            return;

        }

        private void EncodeData()
        {
            //string bitString = string.Join("", encodedTree.ToArray());
            string bitString = "";
            foreach(var d in data)
            {
                List<int> _bitArray = encodeTable[d];
                char c = (char)d;
                //Console.Write(d);
                //Console.Write(" " + c + ": ");
                foreach (var bit in _bitArray)
                {
                    bitString += bit.ToString();
                    //Console.Write(bit);
                }
                //bitString += Convert.ToString(d, 2);
                Console.WriteLine();
                Console.WriteLine();
            }

            data = new byte[(bitString.Length + 7) / 8];
            for (int i = 0; i < bitString.Length; i += 8)
            {
                data[i/8] = Convert.ToByte(bitString.Substring(i, Math.Min(8, bitString.Length - i)), 2);
            }
        }

    }
}
