using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace _06.CriticalSections
{
            
    public class BankAccount
    {
        private int balance;

        public int Balance
        {
            get => balance;
            private set => balance = value;
        }

        public void Deposit(int amount)
        {
            balance += amount;
        }

        public void Withdraw(int amount)
        {
            balance -= amount;
        }
    }


    class Startup
    {
        static SpinLock sl1 = new SpinLock(true);

        static void Main(string[] args)
        {
            var tasks = new List<Task>();
            var ba = new BankAccount();

            SpinLock sl = new SpinLock();
            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Deposit(100);
                        }
                        finally
                        {
                            sl.Exit();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Withdraw(100);
                        }
                        finally
                        {
                            sl.Exit();
                        }
                    }
                }));
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"The current balance is {ba.Balance}");

            LockRecursionTest(5);
        }

        private static void LockRecursionTest(int x)
        {
            bool isTaken = false;

            try
            {
                sl1.Enter(ref isTaken);
                Console.WriteLine($"Successfull lock on {x} iteration");
            }
            catch (LockRecursionException ex)
            {
                Console.WriteLine($"Recursion failed on {x} iteration" + ex);
                throw;
            }
            finally
            {
                LockRecursionTest(x--);
                sl1.Exit();
            }
        }
    }
}
