using System;
using System.Threading.Tasks;

namespace _01.CreatingAndStartingTasks
{
    class Startup
    {
        public static void Write (char symbol)
        {
            int i = 100;
            while(i -- > 0)
            {
                Console.Write(symbol);
            }
        }

        public static void Write (object o)
        {
            int i = 1000;
            while (i -- > 0)
            {
                Console.Write(o);
            }
        }

        public static int TextLength(object o)
        {
            Console.WriteLine($"\nTask with id {Task.CurrentId} processing object {o}...");
            return o.ToString().Length;
        }

        static void Main(string[] args)
        {
            // Creating task with Factori creates it and starts it 
            Task.Factory.StartNew(() => Write('.'));

            Task task = new Task(() => Write('?'));
            task.Start();

            // Calling the same code with parameters inculded to be passed to the method
            Task.Factory.StartNew(Write, "Work");
            Task tWithObj = new Task(Write, "123");
            tWithObj.Start();

            // Getting data from a task
            string text1 = "Testing", text2 = "text";

            Task<int> returnedTask1 = new Task<int>(TextLength, text1);
            returnedTask1.Start();

            Task<int> returnedTask2 = Task.Factory.StartNew(TextLength, text2);

            Console.WriteLine($"Length of '{text1}' is {returnedTask1.Result}");
            Console.WriteLine($"Length of '{text2}' is {returnedTask2.Result}");

            Console.WriteLine("Main Program done.");
            Console.ReadKey();
        }
    }
}
