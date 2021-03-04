using System;
using System.Threading;
using System.Threading.Tasks;

namespace CancellingTasks
{
    class Startup
    {
        static void Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            token.Register(() =>
            {
                Console.WriteLine("Cancelation has been requested!");
            });

            Task t1 = new Task(() =>
            {
                int i = 0;
                while (true)
                {
                    token.ThrowIfCancellationRequested();
                        Console.WriteLine(++i);
                }
            }, token);
            t1.Start();

            Task.Factory.StartNew(() =>
            {
                token.WaitHandle.WaitOne();
                Console.WriteLine(Task.CurrentId);
                Console.WriteLine("Wait handle released, so cancelation was requested");
            });

            Console.ReadKey();
            cts.Cancel();


            // linking cts`s together
            CancellationTokenSource planned = new CancellationTokenSource();
            CancellationTokenSource preventative = new CancellationTokenSource();
            CancellationTokenSource emergency = new CancellationTokenSource();

            CancellationTokenSource paranoid = CancellationTokenSource.CreateLinkedTokenSource(
                planned.Token, preventative.Token, emergency.Token);

            Task.Factory.StartNew(() =>
            {
                int i = 0;
                while (true)
                {
                    paranoid.Token.ThrowIfCancellationRequested();
                    Console.WriteLine($"{i++}");
                    Thread.Sleep(1000);
                }
            }, paranoid.Token);

            Console.ReadKey();
            emergency.Cancel(); // on linked tokens you can call cancel on any of them and it will stop the task

            Console.WriteLine("Main Program done.");
            Console.ReadKey();
        }
    }
}
