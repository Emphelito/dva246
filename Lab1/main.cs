using System;

namespace dva246_lab1
{
    internal class main
    {
        static void Main(string[] args)
        {
            OrderedSet<int> sets = new OrderedSet<int>();
            OrderedSet<int> sets2 = new OrderedSet<int>();
            sets.UnionWith(sets2);

            for (int i = 0; i <= 221; i++)
            {
                sets.Insert(i);
            }
            for (int i = 600; i <= 1000; i++)
            {
                sets.Insert(i);
            }
            Console.WriteLine(sets.Successor(500));
            Console.WriteLine(sets.Predecessor(500));
                
            Console.ReadLine();
        }

    }
}



