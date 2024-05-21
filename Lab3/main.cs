using Huffman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Huffman

{

    public class main
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Huffman.exe -<c/d> <filename>");
                return;
            }
            if (args[0] == "-c")
            {
                Compress cp = new Compress(args[1]);
            }
            if (args[0] == "-d")
            {
                Decompress dcp = new Decompress(args[1]);
            }

        }
    }

    public class Node : IComparable<Node>
    {
        public int frequency;
        public byte value;

        public Node left = null;
        public Node right = null;

        public int CompareTo(Node other)
        {
            return frequency - other.frequency;
        }
    }
}
