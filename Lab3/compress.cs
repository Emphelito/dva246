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
            return frequency.CompareTo(other.frequency);
        }
    }
    public class Compress
    {
        private byte[] data;
        private List<byte> encodedTree;
        private List<byte> encodedData;
        private PriorityQueue<int, Node> priorityQueue;
        Dictionary<byte, BitArray> encodeTable = new Dictionary<byte, BitArray>();

        public Compress(string fileName) 
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            priorityQueue = new PriorityQueue<int,Node>();

            if (!DataToHeap()) throw new Exception($"No data in file: {fileName}");

            HuffmanTree();

            encodedTree = new List<byte>();
            encodedData = new List<byte>();
            BitArray bitArray = new BitArray(0);

            priorityQueue.TryPeek(out int p, out Node current);
            EncodeTree(current, bitArray);

            EncodeData();
            
            fh.Write(encodedData.ToArray());
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
                Node left = new Node();
                priorityQueue.TryDequeue(out p, out left);

                Node right = new Node();
                priorityQueue.TryDequeue(out p, out right);

                Node parent = new Node();
                parent.frequency = left.frequency + right.frequency;
                parent.left = left;
                parent.right = right;
                
                priorityQueue.Enqueue(p, parent);
            }
        }

        private void EncodeTree(Node current, BitArray bitPrefix)
        {      
            if (current == null) return;

            if(current.value != 0)
            {
                bitPrefix.Length++;
                bitPrefix[bitPrefix.Length - 1] = true;
                encodeTable.Add(current.value, bitPrefix);
                encodedTree.Add(1);
                encodedTree.Add(current.value);

            }
            else
            {
                encodedTree.Add(0);
                BitArray leftPrefix = new BitArray(bitPrefix);
                leftPrefix.Length++;
                leftPrefix[leftPrefix.Length - 1] = false;
                EncodeTree(current.left, leftPrefix);

                BitArray rightPrefix = new BitArray(bitPrefix);
                rightPrefix.Length++;
                rightPrefix[rightPrefix.Length - 1] = false;
                EncodeTree(current.right, rightPrefix);
            }
            return;

        }

        private byte[] EncodeData()
        {
            byte[] prefix;
            encodedData.AddRange(encodedTree);
            foreach(var d in data)
            {
                BitArray bitArray = encodeTable[d];
                prefix = new byte[bitArray.Count];
                bitArray.CopyTo(prefix, 0);
                encodedData.AddRange(prefix);
                encodedData.Add(d);
            }
            return data;

        }

    }
}
