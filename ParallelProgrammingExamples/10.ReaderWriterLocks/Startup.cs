using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _10.ReaderWriterLocks
{
    class Startup
    {
        static ReaderWriterLockSlim padLock = new ReaderWriterLockSlim();
        static Random random = new Random();
        static void Main(string[] args)
        {
            int x = 0;
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 10; i++)
            {
                padLock.EnterReadLock();

                Console.WriteLine($"Entered read lock, x = {x}");

                Thread.Sleep(5000);

                padLock.ExitReadLock();

                Console.WriteLine($"Exited read lock, x = {x}");
            }

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException ae)
            {
                ae.Handle(e =>
                {
                    Console.WriteLine(e);
                    return true;
                });
            }

            while (true)
            {
                Console.ReadKey();
                padLock.EnterWriteLock();
                Console.WriteLine("Write lock acquired");
                int newValue = random.Next(10);
                x = newValue;
                Console.WriteLine($"Set x = {x}");
                padLock.ExitWriteLock();
                Console.WriteLine("Write lock released");
            }
        }
    }
}
