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
        public Compress(string fileName, out List<byte> utTree, out byte[] utData, out byte[] utRawData, out List<int> utBitArray)
        {
            // Read given file
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            // Unit Test Raw Data
            utRawData = data;

            // Convert Byte Data
            priorityQueue = new PriorityQueue<int, Node>();
            DataToHeap();

            // Build HuffmanTree
            HuffmanTree();

            // Unit test Tree
            unitTestTree = new List<byte>();
            
            printTree(root);
            utTree = unitTestTree;

            // Encode Table
            List<int> encodeList = new List<int>();
            EncodeTable(root, encodeList);


            encodedTree = new List<byte>();
            // Encode Data
            EncodeData();

            // Encode Tree
            EncodeTree(root);

            // Unit Test Data
            utData = data;
            utBitArray = bitArray;

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
            DataToHeap();

            // Build HuffmanTree
            HuffmanTree();

            // Encode Table
            List<int> encodeList = new List<int>();
            EncodeTable(root, encodeList);

            encodedTree = new List<byte>();
            // Encode Data
            EncodeData();


            // Encode Tree
            EncodeTree(root);        


            fh.Write(data, encodedTree.ToArray());
        }

        // Creates a dict with data as key and freqency as value
        private void DataToHeap()
        {
            Dictionary<byte, int> frequency = new Dictionary<byte, int>();

            if (data == null) throw new Exception($"No data in file");

            foreach (byte d in data)
            {
                if (!frequency.TryAdd(d, 1))
                {       
                    frequency[d]++;
                }
            }
            foreach (var keypair in frequency)
            {
                priorityQueue.Enqueue(keypair.Key, new Node { frequency = keypair.Value, value = keypair.Key });
            }                
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

            List<int> strL = new List<int>(path);
            strL.Add(0);
            EncodeTable(current.left, strL);
            List<int> strR = new List<int>(path);
            strR.Add(1);
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
        }

        private void EncodeData()
        {
            bitArray = new List<int>();

            foreach (var d in data)
            {
                 bitArray.AddRange(encodeTable[d]);
            }

            int numOfBytes = (bitArray.Count + 7) / 8; // Calculate the number of bytes required

            byte[] dataLength = BitConverter.GetBytes(data.Length);
            // Gives us consitency in the way the size is written to the file
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataLength);
            }

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
            /* This adds the length of the tree after decoding to the file 
            /* that means that even if we write garbage or extra useless bits to 
            /* the file the decoding side will know when/where to cut of exccees
            */
            for(int i = 0; i < dataLength.Length; i++ )
            {
                encodedTree.Add(dataLength[i]);
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
