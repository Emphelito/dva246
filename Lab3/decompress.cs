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
        public Decompress(string fileName)
        {
            FileHandling fh = new FileHandling(fileName);
            data = fh.Read();

            foreach (byte b in data)
            {
                Console.Write(b);
                break;
            }

            ByteToBit();
        }
        private void ByteToBit()
        {
            bitArray = new BitArray(data);

        }
    }
}
