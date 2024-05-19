using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    public class FileHandling
    {
        private string fileName;
        private byte[] data;
        public FileHandling(string _fileName)
        {
            fileName = _fileName;
        }
        public FileHandling(string _fileName, byte[] _data)
        {
            fileName = _fileName;
            data = _data;
        }

        public byte[] Read()
        {
            if(fileName == null)
            {
                return null;
            }
            try
            {
                data = File.ReadAllBytes(fileName);

                return data;
            }
            catch
            {
                throw new Exception($"Could not read file: {fileName}");
            }
        }

        public void Write(byte[] data)
        {
            try
            {
                if (fileName.Substring(fileName.LastIndexOf('.') + 1) == "hf")
                {

                    fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        File.WriteAllBytes(fileName, data);
                }
                else
                {

                    foreach (byte b in data)
                    {
                        Console.Write(b);
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    fileName = fileName.Substring(0, fileName.LastIndexOf('.')) + ".hf";
                    using (var stream = File.Open(fileName, FileMode.Create))
                    {
                        using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                            writer.Write(data);
              
                    }
                }
                
            }
            catch
            {
                throw new Exception($"Could not write to file: {fileName}");
            }
            
        }
    }
}
