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
        private PriorityQueue<int, Node> priorityQueue;
        Dictionary<byte, List<int>> encodeTable = new Dictionary<byte, List<int>>();

        public Compress(string fileName) 
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            foreach (byte b in data)
            {
                Console.Write(b);
            }

            priorityQueue = new PriorityQueue<int,Node>();

            if (!DataToHeap()) throw new Exception($"No data in file: {fileName}");

            HuffmanTree();

            encodedTree = new List<byte>();
            List<int> bitArray = new List<int>();

            priorityQueue.TryPeek(out int p, out Node current);
            EncodeTable(current, bitArray);

            priorityQueue.TryPeek(out p, out current);
            main m = new main();
            //m.printTree(current);

            List<byte> byteList = new List<byte>();
            EncodeTree(current, byteList);
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
        private void EncodeTable(Node current, List<int> path)
        {
            if (current == null)
            {
                return;
            }
            else if (current.left == null && current.right == null)
            {
                encodeTable[current.value] = path;
            }
            List<int> strR = new List<int>(path);
            strR.Add(1);
            List<int> strL = new List<int>(path);
            strL.Add(0);
            EncodeTable(current.left, strL);
            EncodeTable(current.right, strR);
        }

        private void EncodeTree(Node Current, List<byte> path)
        {
            if (Current.left == null && Current.right == null) 
            {
                encodedTree.AddRange(path);
                encodedTree.Add(1);
                encodedTree.Add(Current.value);
                path.Clear();
                return;
            }

            path.Add(0);
            EncodeTree(Current.left, path);
            EncodeTree(Current.right, path);



            return;
        }

        private void EncodeData()
        {
            string bitString = "";

            foreach(var d in data)
            {
                List<int> _bitArray = encodeTable[d];
                foreach (var bit in _bitArray)
                {
                    bitString += bit.ToString();
                }
            }

            data = new byte[((bitString.Length) / 8)];
            for (int i = 0; i < bitString.Length/8; ++i)
            {
                data[i] = Convert.ToByte(bitString.Substring(8 * i, 8), 2);
            }
        }

    }
}
