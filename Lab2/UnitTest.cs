using PQ = PriorityQueues;
namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            PQ.PriorityQueue<string, int> queue = new PQ.PriorityQueue<string, int>();

            for (int i = 0; i <= 1000; i++)
            {
                queue.Enqueue("NR: " + i, i);
            }

            queue.TryPeek(out string element, out int priority);

            Assert.AreEqual(1001, queue.Count);
            Assert.AreEqual(1000, priority);

            for (int i = 0; i <= 999; i++)
            {
                queue.Dequeue();
            }

            queue.TryPeek(out element, out priority);

            Assert.AreEqual(1, queue.Count);
            Assert.AreEqual(0, priority);

            queue.Dequeue();
            Assert.IsFalse(queue.TryPeek(out element, out priority));

            var handle = queue.Enqueue("Test", 1);
            queue.TryPeek(out element, out priority);
            Assert.AreEqual(1, priority);

            handle.Priority = 72;
            queue.TryPeek(out element, out priority);
            Assert.AreEqual(72, priority);

            for (int i = 0;i <= 60; i++)
            {
                queue.Enqueue("NR: " + i, i);
            }

            queue.TryPeek(out element, out priority);
            Assert.AreEqual(72, priority);

            handle.Priority = -1;
            queue.TryPeek(out element, out priority);
            Assert.AreEqual(60, priority);

            for (int i = 0; i <= 60; i++)
            {
                queue.Dequeue();
            }

            queue.TryPeek(out element, out priority);
            Assert.AreEqual(-1, priority);

            Assert.IsTrue(queue.TryDequeue(out element, out priority));
            Assert.IsFalse(queue.TryPeek(out element, out priority));

            for (int i = 10000; i > 0; i--)
            {
                queue.Enqueue("NR: " + i, i);
            }

            queue.TryPeek(out element, out priority);
            Assert.AreEqual(10000, priority);

            for (int i = 10000; i > 0; i--)
            {
                queue.Dequeue();
            }
            Assert.IsFalse(queue.TryDequeue(out element, out priority));
        }
    }
}