using Huffman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dva246_lab3
{
    public class main
    {
        static void Main(string[] args)
        {
            Compress cpi = new Compress("text.txt");
            Decompress dcpi = new Decompress("text.hf");
            return;
            if(args.Length != 2) 
            {
                Console.WriteLine("Usage: Huffman.exe -<c|d> <filename>");
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
}
