using System;
using System.Collections.Concurrent;

namespace _12.ConcurrentQueue
{
    class Startup
    {
        static void Main(string[] args)
        {
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            int result;
            if (queue.TryDequeue(out result))
                Console.WriteLine($"Successfully removed {result} from the queue");

            if (queue.TryPeek(out result))
                Console.WriteLine($"The front element of the queue is {result}");
        }
    }
}
