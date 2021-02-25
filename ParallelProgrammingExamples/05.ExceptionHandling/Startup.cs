using System;
using System.Threading.Tasks;

namespace _05.ExceptionHandling
{
    class Startup
    {
        static void Main(string[] args)
        {
            try
            {
                ExceptionHandling();
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                    Console.WriteLine($"Exception {ex.GetType()} handled elsewhere!");
            }

            Console.WriteLine("Main Program done.");
            Console.ReadKey();
        }

        private static void ExceptionHandling()
        {
            Task t = Task.Factory.StartNew(() =>
            {
                throw new InvalidOperationException("Can`t do this") { Source = "t" };
            });

            Task t2 = Task.Factory.StartNew(() =>
            {
                throw new AccessViolationException("Can`t access this") { Source = "t2" };
            });

            // when you do Task.WaitAll() you get the exceptions

            try
            {
                Task.WaitAll(t, t2);
            }
            catch (AggregateException ae)
            {
                //foreach (var ex in ae.InnerExceptions)
                //    Console.WriteLine($"Exception {ex.GetType()} with message '{ex.Message}' from {ex.Source}");
                ae.Handle(ae =>
                {
                    if (ae is InvalidOperationException)
                    {
                        Console.WriteLine("Invalid op handled");
                        return true;
                    }
                    return false;
                });
            }
        }
    }
}
