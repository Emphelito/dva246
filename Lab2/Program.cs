using System;
using System.Collections;
using System.Collections.Generic;

namespace PriorityQueues
{

	class MainClass
	{
        public static void GetHeapType()
        {
            PriorityQueue<string, int> queue = new PriorityQueue<string, int>();
            queue.Enqueue("one", 1);
            queue.Enqueue("two", 2);
            string element;
            int priority;
            bool success = queue.TryPeek(out element, out priority);
            var handler = queue.Dequeue();
            if (success == false || handler.Priority != priority)
                Environment.Exit(10);
            if (priority == 1)
                Console.WriteLine("PriorityQueue is implemented as a MinHeap!");
            if (priority == 2)
                Console.WriteLine("PriorityQueue is implemented as a MaxHeap!");
        }
        public static void Main(string[] args)
		{
            PriorityQueue<string, int> queue = new PriorityQueue<string, int>();
            GetHeapType();
            //A three step increase n*2^3

            stepMethod(2, queue);           //1 steps
            stepMethod(16, queue);          //4 steps
            stepMethod(128, queue);         //7 steps
            stepMethod(1024, queue);        //10 steps
            stepMethod(8192, queue);        //13 steps
            stepMethod(65536, queue);       //16 steps
            stepMethod(524288, queue);      //19 steps
            stepMethod(4194304, queue);     //22 steps

            /*
             * Steps for 128 elements is 7 steps which means we expect that for 524k elements it should take 19 steps if it is O(log n) because,
             * (524.288/128 = 2^12) that is a 12 step difference between 128 to 524.288.
             * So if this is O(log n) there should only be 3 step difference between 524.288 and 4.194.304 (4.194.304/524.288 = 2^3) which there is.
             * This is true for both the enqueue method and the dequeue method.
             * 
             * For O(log n) everytime n doubles in size the amount of steps increases by one this is a trend that we can se in the above code as well.
             * If we print the steps everytime we add or remove and element we can also see that the amount of time it takes for the steps to increase, increases
             * since the "distance" between the numbers increases ex. 100->200 is smaller than 500k->1m.
             */
        }
        private static void stepMethod(int range, PriorityQueue<string, int> queue)
        {
            for (int i = 0; i <= range; i++)
            {
                queue.Enqueue("placeholder", i);
            }
            Console.WriteLine("Enqueue steps for " + range + ": " + queue.stepList.Last());
            queue.stepList.Clear();

            for (int i = 0; i <= range; i++)
            {
                queue.Dequeue();
            }
            Console.WriteLine("Dequeue steps for " + range + ": " + queue.stepList[0] + "\n");
            queue.stepList.Clear();
        }

    }
}