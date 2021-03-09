using System;
using System.Threading.Tasks;

namespace _16.Continuations
{
    class Startup
    {
        static void Main(string[] args)
        {
            Task boiling = Task.Factory.StartNew(() =>  Console.WriteLine("Boiling the water"));
            Task usingBoilledWater = boiling.ContinueWith(t => {
                Console.WriteLine($"Water booiling was finished task - {t.Id}");
             });

            usingBoilledWater.Wait();

            Task<string> task = Task.Factory.StartNew(() => "Task 1");
            Task<string> task2 = Task.Factory.StartNew(() => "Task 2");
            
            Task task3 = Task.Factory.ContinueWhenAll(new[] { task, task2 }, tasks =>
            {
                Console.WriteLine("Tasks completed:");
                foreach (var result in tasks)
                    Console.WriteLine(" - " + result.Result);
                Console.WriteLine("All tasks done");
            });

            task3.Wait();
        }
    }
}
