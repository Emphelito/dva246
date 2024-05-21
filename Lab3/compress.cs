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

        // Constructor for Unit Tests
        public Compress(string fileName, out List<byte> utTree, out byte[] utData, out byte[] utRawData)
        {
            // Read given file
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            // Unit Test Raw Data
            utRawData = data;

            // Convert Byte Data
            priorityQueue = new PriorityQueue<int, Node>();
            if (!DataToHeap()) throw new Exception($"No data in file: {fileName}");

            // Build HuffmanTree
            HuffmanTree();

            // Unit test Tree
            unitTestTree = new List<byte>();
            priorityQueue.TryPeek(out int p, out Node current);
            printTree(current);
            utTree = unitTestTree;

            // Encode Table
            List<int> bitArray = new List<int>();
            priorityQueue.TryPeek(out p, out current);
            EncodeTable(current, bitArray);

            // Encode Tree
            priorityQueue.TryPeek(out p, out current);
            encodedTree = new List<byte>();
            List<byte> byteList = new List<byte>();
            EncodeTree(current, byteList);

            // Encode Data
            EncodeData();

            // Unit Test Data
            utData = data;

            // Inserts last element of encoded tree(it is always a symbol) into the index 0, its used to determine when
            // instructions for tree structure ends.
            encodedTree.Insert(0, encodedTree[encodedTree.Count - 1]);
            fh.Write(data, encodedTree.ToArray());
        }

        // Constructor runs every function
        public Compress(string fileName) 
        {
            // Read given file
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            // Convert Byte Data
            priorityQueue = new PriorityQueue<int,Node>();
            if (!DataToHeap()) throw new Exception($"No data in file: {fileName}");

            // Build HuffmanTree
            HuffmanTree();

            // Encode Table
            List<int> bitArray = new List<int>();
            priorityQueue.TryPeek(out int p, out Node current);
            EncodeTable(current, bitArray);

            // Encode Tree
            priorityQueue.TryPeek(out p, out current);
            encodedTree = new List<byte>();
            List<byte> byteList = new List<byte>();
            EncodeTree(current, byteList);

            // Encode Data
            EncodeData();

            // Inserts last element of encoded tree(it is always a symbol) into the index 0, its used to determine when
            // instructions for tree structure ends.
            encodedTree.Insert(0, encodedTree[encodedTree.Count - 1]);
            fh.Write(data, encodedTree.ToArray());
        }

        // Creates a dict with data as key and freqency as value
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
        //Creates a dict to be used when encoding data
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

        // Creates the instructions for how to build the tree
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
            int tmp = 0;
            foreach(var d in data)
            {
                if(d == 116)
                {
                    tmp++;
                }

                List<int> _bitArray = encodeTable[d];
                foreach (var bit in _bitArray)
                {
                    bitString += bit.ToString();
                }
                if(tmp == 6180)
                {
                    Console.WriteLine();
                }
            }

            data = new byte[(bitString.Length / 8)];
            for (int i = 0; i < bitString.Length/8; ++i)
            {
                //Dividing 8 char's every itteration into one byte
                data[i] = Convert.ToByte(bitString.Substring(8 * i, 8), 2);
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
