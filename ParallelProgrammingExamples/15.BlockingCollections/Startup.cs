using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BlockingCollections
{
    class Startup
    {
        static ConcurrentBag<int> theBag = new ConcurrentBag<int>();
        static BlockingCollection<int> block =
            new BlockingCollection<int>(theBag, 10);
        static CancellationTokenSource cts = new CancellationTokenSource();
        static Random random = new Random();

        static void ProducerConsumer()
        {
            var producer = Task.Factory.StartNew(RunProducer);
            var consumer = Task.Factory.StartNew(RunConsumer);

            try
            {
                Task.WaitAll(new[] { producer, consumer }, cts.Token);
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => true);
            }
        }

        static void Main(string[] args)
        {
            Task.Factory.StartNew(ProducerConsumer, cts.Token);

            Console.ReadKey();
            cts.Cancel();
        }

        private static void RunConsumer()
        {
            foreach (var item in block.GetConsumingEnumerable())
            {
                cts.Token.ThrowIfCancellationRequested();
                Console.WriteLine($"-{item}\t");
                Thread.Sleep(random.Next(1500));
            }
        }

        private static void RunProducer()
        {
            while (true)
            {
                cts.Token.ThrowIfCancellationRequested();
                int i = random.Next(100);
                block.Add(i);
                Console.WriteLine($"+{i}\t");
                Thread.Sleep(random.Next(150));
            }
        }
    }
}
