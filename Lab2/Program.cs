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

			//queue.Dequeue();

            var handle = queue.Enqueue("Hej", 23);
            queue.TryPeek(out element, out priority);
            Console.WriteLine(element + "/" + priority);

			for (int i = 0; i < 200; i++)
			{
				queue.Enqueue("placeholder", i);
			}

			queue.TryPeek(out element, out priority);
			Console.WriteLine(element + "/" + priority);

			queue.Enqueue("tjo", 4000);
            queue.TryPeek(out element, out priority);
            Console.WriteLine(element + "/" + priority);

			queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();
			for(int i = 0; i < 197;  i++)
			{
				queue.Dequeue();
			}
            queue.TryPeek(out element, out priority);
            Console.WriteLine(element + "/" + priority);


        }

	}
}