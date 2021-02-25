using System;
using System.Threading;
using System.Threading.Tasks;

namespace _03.WaitingOnTask
{
    class Startup
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task task = new Task(() =>
            {
                Console.WriteLine("You have 5 seconds to disarm the bomb, by pressing any key!");

                bool isCancelled = token.WaitHandle.WaitOne(5000);

                Console.WriteLine(isCancelled ? "Bomb was disarmed!" : "BOOM!!!");
            }, token);
            task.Start();

            Console.ReadKey();
            cts.Cancel();

            Console.WriteLine("Main Program done.");
            Console.ReadKey();
        }
    }
}
