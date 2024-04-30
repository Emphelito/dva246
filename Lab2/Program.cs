using System;
using System.Collections;
using System.Collections.Generic;

namespace PriorityQueues
{

	class MainClass
	{

		public static void Main(string[] args)
		{
			PriorityQueue<string, int> queue;

			queue = new PriorityQueue<string, int>();

			string element;
			int priority;


			for (int i = 0; i <= 5000; i++)
			{
				queue.Enqueue("placeholder", i);
			}

            for(int i = 0; i <= 5000;  i++)
			{
				queue.TryDequeue(out element, out priority);
				Console.WriteLine(priority);
			}
        }

	}
}