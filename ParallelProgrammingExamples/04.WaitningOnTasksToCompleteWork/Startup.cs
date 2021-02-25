using System;
using System.Threading;
using System.Threading.Tasks;

namespace _04.WaitningOnTasksToCompleteWork
{
    class Startup
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;


            Task task = new Task(() =>
            {
                Console.WriteLine("I take 5 seconds");

                for (int i = 0; i < 5; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(1000);
                }

                Console.WriteLine("I`m done.");
            }, token);
            task.Start();

            Task task2 = Task.Factory.StartNew(() => Thread.Sleep(3000));

            // Can wait on a perticular task like task2.Wait();
            // Or can use Task.WaitAll() witch will wait for all tasks to be completed
            // Task.WaitAny() will end when the task that takes less time is completed

            Task.WaitAny(task, task2);

            Console.WriteLine($"Task 'task' status is {task.Status}");
            Console.WriteLine($"Task 'task2' status is {task2.Status}");


            Console.WriteLine("Main Program done.");
            Console.ReadKey();
        }
    }
}
