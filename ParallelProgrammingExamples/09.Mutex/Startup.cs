using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _09.Mutex
{
    public class BankAccount
    {
        public int Balance { get; set; }

        public void Deposit(int amount)
        {
            this.Balance += amount;
        }

        public void Withdraw(int amount)
        {
            this.Balance -= amount;
        }

        public void Transfer(BankAccount to, int amount)
        {
            this.Balance -= amount;
            to.Balance += amount;
        }
    }

    class Startup
    {
        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();
            var ba2 = new BankAccount();
            System.Threading.Mutex mutex = new System.Threading.Mutex();
            System.Threading.Mutex mutex2 = new System.Threading.Mutex();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex.WaitOne();
                        try
                        {
                            ba.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock)
                                mutex.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = mutex2.WaitOne();
                        try
                        {
                            ba2.Deposit(1);
                        }
                        finally
                        {
                            if (haveLock)
                                mutex2.ReleaseMutex();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool haveLock = System.Threading.Mutex.WaitAll(new[] { mutex, mutex2 });

                        try
                        {
                            ba2.Transfer(ba, 1);
                        }
                        finally
                        {
                            if (haveLock)
                            {
                                mutex.ReleaseMutex();
                                mutex2.ReleaseMutex();
                            }
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The current balance of ba is {ba.Balance}");
            Console.WriteLine($"The current balance of ba2 is {ba2.Balance}");
        }
    }
}
