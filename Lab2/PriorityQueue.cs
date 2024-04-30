﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;

namespace PriorityQueues
{
	public class PriorityQueue<TElement, TPriority> : IPriorityQueue<TElement, TPriority>
	{
		private List<(TElement, TPriority)> priorityQueue;
        private IComparer<TPriority> comparer;

        public PriorityQueue()
		{
            priorityQueue = new List<(TElement, TPriority)> ();
            comparer = Comparer<TPriority>.Default;
        }
        public PriorityQueue(IComparer<TPriority> _comparer)
		{
            priorityQueue = new List<(TElement, TPriority)>();
			comparer = _comparer;
        }


        public int Count { get; private set; } 	

		public IPriorityQueueHandle<TElement, TPriority> Enqueue(TElement element, TPriority priority)
		{
            var handle = new PriorityQueueHandle(this, 0);
            priorityQueue.Add((element, priority));

            Count++;
            Console.WriteLine();
            for (int i = 0; i < Count; i++)
            {
                Console.Write(priorityQueue[i].Item2 + ", ");
            }
            Console.WriteLine();
            int stepps = PriorityUp(Count - 1);

			return handle;
		}
        public IPriorityQueueHandle<TElement, TPriority> Dequeue()
		{
			if (Count <= 0) throw new InvalidOperationException();

			var handel = new PriorityQueueHandle(this, 0);

            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < Count; i++)
            {
                Console.Write(priorityQueue[i].Item2 + ", ");
            }

            var tmp = priorityQueue[Count - 1];
			priorityQueue[Count -1] = priorityQueue[0];
			priorityQueue[0] = tmp;
			priorityQueue.RemoveAt(Count - 1);

            Count--;

            Console.WriteLine();
            int stepps = PriorityDown(0);

            Console.WriteLine();
            for (int i = 0; i < Count; i++)
            {
                Console.Write(priorityQueue[i].Item2 + ", ");
            }
			Console.WriteLine();


            return handel;

        }
		public bool TryDequeue (out TElement element, out TPriority priority) 
		{
			if (Count <= 0) {
				element = default;
				priority = default;
				return false;
			}
			var handle = Dequeue();
			element = handle.Element;
			priority = handle.Priority;
			return true;
		}
        private int PriorityUp(int index) 
		{
			int steps = 0;          
            while (index > 0)
			{
                int parentIndex = (index -1) / 2;

                
                if (comparer.Compare(priorityQueue[parentIndex].Item2, priorityQueue[index].Item2) >= 0)
				{
					break;
				}

				var tmp = priorityQueue[parentIndex];
				priorityQueue[parentIndex] = priorityQueue[index];
				priorityQueue[index] = tmp;
				index = parentIndex;

				steps++;

			}
			return steps;
		}
        private int PriorityDown(int index) 
		{
			int rChildIndex;
			int lChildIndex;

            int stepps = 0;
			while (true && Count > 1)
			{
				stepps++;
                lChildIndex = 2 * index + 1;
                rChildIndex = 2 * index + 2;
                int smallestIndex = index;

				Console.WriteLine(rChildIndex + "/" + lChildIndex + "///" + smallestIndex );

				if (rChildIndex < Count && comparer.Compare(priorityQueue[rChildIndex].Item2, priorityQueue[smallestIndex].Item2) > 0)
				{
					Console.WriteLine("R");
					smallestIndex = rChildIndex;
				}
                if (lChildIndex < Count && comparer.Compare(priorityQueue[lChildIndex].Item2, priorityQueue[smallestIndex].Item2) > 0)
                {
                    Console.WriteLine("L");
                    smallestIndex = lChildIndex;
                }
				if (index != smallestIndex)
				{
					var tmp = priorityQueue[index];
					priorityQueue[index] = priorityQueue[smallestIndex];
					priorityQueue[smallestIndex] = tmp;
				}
				else break;
				index = smallestIndex;
            }
			return stepps;
            



        }

        public bool TryPeek(out TElement element, out TPriority priority)
        {
			if(Count <= 0)
			{
				element = default;
				priority = default;
				return false;
			}
			var handle = new PriorityQueueHandle (this, 0);
			element = handle.Element;
			priority = handle.Priority;
			return true;
        }

		private class PriorityQueueHandle : IPriorityQueueHandle<TElement, TPriority>
		{
            private readonly PriorityQueue<TElement, TPriority> _priorityQueue;
			private int index;

			public PriorityQueueHandle(PriorityQueue<TElement, TPriority> priorityQueue, int index)
            {
                _priorityQueue = priorityQueue;
                this.index = index;
            }

            public TElement Element => _priorityQueue.priorityQueue[index].Item1;
			public TPriority Priority
			{
				get => _priorityQueue.priorityQueue[index].Item2;

				set
				{

				}
			}

		}
    }

}
