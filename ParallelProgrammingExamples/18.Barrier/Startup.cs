using System;
using System.Threading;
using System.Threading.Tasks;

namespace _18.Barrier
{
    class Startup
    {
        static readonly System.Threading.Barrier barrier = new System.Threading.Barrier(2, b =>
        {
            Console.WriteLine($"Phase {b.CurrentPhaseNumber} is finished");
        });

        public static void Water()
        {
            Console.WriteLine("Putting water to boil (takes a bit longer)");
            Thread.Sleep(2000);
            barrier.SignalAndWait();
            Console.WriteLine("Pouring water into cup.");
            barrier.SignalAndWait();
            Console.WriteLine("Turn off the water heater.");
        }

        public static void Cup()
        {
            Console.WriteLine("Finding the nicest cup of tea (fast)");
            barrier.SignalAndWait();
            Console.WriteLine("Adding tea.");
            barrier.SignalAndWait();
            Console.WriteLine("Adding sugar.");
        }

        static void Main()
        {
            Task water = Task.Factory.StartNew(Water);
            Task cup = Task.Factory.StartNew(Cup);

            Task tea = Task.Factory.ContinueWhenAll(new[] { water, cup }, tasks =>
             {
                 Console.WriteLine("Enjoy your cup of tea");
             });

            tea.Wait();
        }
    }
}
