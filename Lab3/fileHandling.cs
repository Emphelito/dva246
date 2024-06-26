﻿using System;
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
        public byte[] Read()
        {
            if(fileName == null)
            {
                throw new Exception($"Please provide a filename");
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
                // Removes everything after a . (example.hf -> example)
                fileName = fileName.Substring(0, fileName.LastIndexOf("."));
                using (var stream = File.Open(fileName, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream, Encoding.ASCII, false))
                    {
                        writer.Write(data);
                    }
                }
            }
            catch
            {
                throw new Exception($"Could not write to file: {fileName}");
            }
            
        }
        public void Write(byte[] data, byte[] treePath)
        {
            // If given fileName contains a file extension(example.rar) replace it with .hf
            if(fileName.Contains("."))
            {
                fileName = fileName.Substring(0, fileName.LastIndexOf('.')) + ".hf";
            }
            // If no file extension exists add .hf
            else
            {
                 fileName += ".hf";
            }

            using (var stream = File.Open(fileName, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.ASCII, false))
                {
                    writer.Write(treePath);
                    writer.Write(data);
                }

            }
        }
    }
}
