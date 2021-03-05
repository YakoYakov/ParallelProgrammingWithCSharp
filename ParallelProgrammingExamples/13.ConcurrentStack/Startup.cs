using System;
using System.Collections.Concurrent;
using System.Linq;

namespace _13.ConcurrentStack
{
    class Startup
    {
        static void Main(string[] args)
        {
            ConcurrentStack<int> stack = new ConcurrentStack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);

            int result;
            if (stack.TryPeek(out result))
                Console.WriteLine($"The top element is {result}");

            if (stack.TryPop(out result))
                Console.WriteLine($"Element with value {result} popped");

            int[] items = new int[9];

            if (stack.TryPopRange(items, 0, 9) > 0)
            {
                string text = string.Join(", ", items.Select(i => i.ToString()));
                Console.WriteLine("Tru to get 9 elements but got " + text);
            }

        }
    }
}
