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
            OrderedSet<int> sets2 = new OrderedSet<int>();
            sets.UnionWith(sets2);

            for (int i = 0; i <= 1000; i++)
            {
                sets.Insert(i);
            }
            Console.WriteLine(sets.Successor(999));
                
            Console.ReadLine();
        }

    }
}



