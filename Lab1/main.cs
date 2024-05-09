using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace dva246_lab1
{
    internal class main
    {
        static void Main(string[] args)
        {
            OrderedSet<int> sets = new OrderedSet<int>();

            for (int i = 0; i <= 1000; i++)
            {
                sets.Insert(i);
            }
            Console.WriteLine(sets.Successor(256));
            Console.ReadLine();
        }

    }
}



