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
        private List<int> bitArray;

        Node root;

        // Constructor for Unit Tests
        public Compress(string fileName, out List<byte> utTree, out byte[] utData, out byte[] utRawData, out string utBitString)
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
            
            printTree(root);
            utTree = unitTestTree;

            // Encode Table
            List<int> _bitArray = new List<int>();
            EncodeTable(root, _bitArray);

            // Encode Tree
            encodedTree = new List<byte>();
            EncodeTree(root);
            // Inserts last element of encoded tree(it is always a symbol) into the index 0, its used to determine when
            // instructions for tree structure begins and ends.
            encodedTree.Insert(0, encodedTree[encodedTree.Count - 1]);

            // Encode Data
            EncodeData();

            // Unit Test Data
            utData = data;
            utBitString = string.Join("", bitArray);

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
            EncodeTable(root, bitArray);

            // Encode Tree
            encodedTree = new List<byte>();
            EncodeTree(root);
            // Inserts last element of encoded tree(it is always a symbol) into the index 0, its used to determine when
            // instructions for tree structure begins and ends.
            encodedTree.Insert(0, encodedTree[encodedTree.Count - 1]);

            // Encode Data
            EncodeData();

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
            priorityQueue.TryPeek(out p, out root);
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
        private void EncodeTree(Node Current)
        {
            if (Current.left == null && Current.right == null) 
            {
                encodedTree.Add(1);
                encodedTree.Add(Current.value);
                return;
            }
            encodedTree.Add(0);
            EncodeTree(Current.left);
            EncodeTree(Current.right);

            return;
        }

        private void EncodeData()
        {
            bitArray = new List<int>();
            foreach (var d in data)
            {
                 bitArray.AddRange(encodeTable[d]);
            }

            int numOfBytes = bitArray.Count / 8; // Calculate the number of bytes required
            if (bitArray.Count % 8 != 0) numOfBytes++; // Add an extra byte if the number of bits is not divisible by 8

            byte[] dataLength = BitConverter.GetBytes(data.Length);

            data = new byte[(numOfBytes)];
            int byteIndex = 0;
            int bitIndex = 0;

            foreach (int bit in bitArray)
            {
                data[byteIndex] |= (byte)(bit << (7 - bitIndex)); // Set the bit in the byte
                bitIndex++;
                if (bitIndex == 8) // Move to the next byte if all bits in the current byte are set
                {
                    byteIndex++;
                    bitIndex = 0;
                }
            }
            for(int i = dataLength.Length - 1; i >= 0; i-- )
            {
                encodedTree.Insert(0, dataLength[i]);
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
