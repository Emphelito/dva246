using System;

namespace LCS
{
    internal class Program
    {
        static void Main(string[] args) 
        { 
            LCS lCS = new LCS();
            Console.Write(lCS.CalcLCS("abcdefgh", "defh"));
        }

    }
}
