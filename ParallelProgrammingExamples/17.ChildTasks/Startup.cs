using System;
using System.Threading;
using System.Threading.Tasks;

namespace _17.ChildTasks
{
    class Startup
    {
        static void Main(string[] args)
        {
            Task parent = new Task(() =>
            {
                Task child = new Task(() =>
                {
                    Console.WriteLine("Child task started");
                    Thread.Sleep(3000);
                    Console.WriteLine("Child task finished");
                    //throw new Exception();
                }, TaskCreationOptions.AttachedToParent);


                var successHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine("Child ran to completion");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnRanToCompletion);

                var errorHandler = child.ContinueWith(t =>
                {
                    Console.WriteLine("Child task failed");
                }, TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.OnlyOnFaulted);

                child.Start();
            });

            parent.Start();

            try
            {
                parent.Wait();
            }
            catch (AggregateException ae)
            {
                ae.Handle(e => true);
            }
        }
    }
}
