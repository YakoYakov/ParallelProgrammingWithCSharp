using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace _11.ConcurrentDictionary
{
    class Startup
    {
        private static System.Collections.Concurrent.ConcurrentDictionary<string, string> capitals =
            new System.Collections.Concurrent.ConcurrentDictionary<string, string>();

        private static void AddParis()
        {
            bool isSuccessful = capitals.TryAdd("France", "Paris");
            string who = Task.CurrentId.HasValue ? ("Task " + Task.CurrentId) : "Main thread";
            Console.WriteLine($"{who} {(isSuccessful ? "added" : "not added")} the element");
        }

        static void Main(string[] args)
        {
            Task.Factory.StartNew(AddParis).Wait();
            AddParis();

            capitals["Russia"] = "Leningrad";
            capitals.AddOrUpdate("Russia", "Moscow", (key, oldValue) => $"old value was {oldValue} now is => Moscow");
            Console.WriteLine($"The capital of Russia is {capitals["Russia"]}");

            //capitals["Sweden"] = "Upsala";
            string cptSweden = capitals.GetOrAdd("Sweden", "Stockholm");
            Console.WriteLine($"The capital of Sweden is {cptSweden}");

            const string toRemove = "Russia";
            string removed = null;
            bool isRemoved = capitals.TryRemove(toRemove, out removed);

            if (isRemoved)
            {
                Console.WriteLine($"We removed {removed}");
            }
            else
            {
                Console.WriteLine($"Missing key with value {toRemove} in capitals dicitionary");
            }
        }
    }
}
