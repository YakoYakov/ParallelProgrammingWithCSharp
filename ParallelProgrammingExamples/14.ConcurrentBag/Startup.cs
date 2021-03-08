using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _14.ConcurrentBag
{
    class Startup
    {
        static void Main(string[] args)
        {
            // ConcurrentBag has no order to the elemets added or removed
            ConcurrentBag<int> bag = new ConcurrentBag<int>();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                int toAdd = i;
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    bag.Add(toAdd);
                    Console.WriteLine($"{Task.CurrentId} has added {toAdd}");
                    int result;
                    if (bag.TryPeek(out result))
                        Console.WriteLine($"{Task.CurrentId} has peeked the value {result}");
                }));
            }

            Task.WaitAll(tasks.ToArray());

            if (bag.TryTake(out int result))
                Console.WriteLine($"You take the value {result} from the bag");
        }
    }
}
