using System;
using System.Threading;
using System.Threading.Tasks;

namespace _19.CountdownEvent
{
    public class StartUp
    {
        static int taskCount = 5;
        static System.Threading.CountdownEvent cde = new System.Threading.CountdownEvent(taskCount);
        static Random random = new Random();

        static void Main(string[] args)
        {
            for (int i = 0; i < taskCount; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"Entering Task {Task.CurrentId}");
                    Thread.Sleep(random.Next(3000));
                    cde.Signal();
                    Console.WriteLine($"Exit Task {Task.CurrentId}");
                });
            }

            Task task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"Waiting in final task id {Task.CurrentId} for countdown to end");
                cde.Wait();
                Console.WriteLine("Finished with final task");
            });

            task.Wait();
        }
    }
}
