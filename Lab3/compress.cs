using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{

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
            
            encodedTree.Insert(0, encodedTree[encodedTree.Count - 1]);
            fh.Write(data, encodedTree.ToArray());
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
                priorityQueue.Enqueue(keypair.Key, new Node { frequency = keypair.Value, value = keypair.Key });
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

        /*private void EncodeTree(Node current, List<int> bitPrefix)
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
                EncodeTree(current.left, leftPrefix);

                encodedTree.Add(0);
                List<int> rightPrefix = new List<int>();
                rightPrefix.AddRange(bitPrefix);
                rightPrefix.Add(0);
                EncodeTree(current.right, rightPrefix);
            }
            return;

        }*/
        private void EncodeTree(Node current, List<int> str)
        {
            if (current == null)
            {
                return;
            }
            else if (current.value != 0)
            {
                encodeTable[current.value] = str;
            }
            List<int> strR = new List<int>(str);
            strR.Add(1);
            List<int> strL = new List<int>(str);
            strL.Add(0);
            EncodeTree(current.left, strL);
            EncodeTree(current.right, strR);
        }

        private void EncodeData()
        {
            string bitString = "";
            foreach(var b in encodeTable)
            {
                for(int i = 0; i < b.Value.Count; i += 1)
                {
                    encodedTree.Add(Convert.ToByte(b.Value[i]));
                }
                encodedTree.Add(b.Key);
            }

            foreach(var d in data)
            {
                List<int> _bitArray = encodeTable[d];
                foreach (var bit in _bitArray)
                {
                    bitString += bit.ToString();
                }
            }

            data = new byte[((bitString.Length + 7) / 8) + 8];
            for (int i = 0; i < bitString.Length/8; ++i)
            {
                data[i] = Convert.ToByte(bitString.Substring(8 * i, 8), 2);
            }
        }

    }
}
