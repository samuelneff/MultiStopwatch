using System;
using System.Threading;

namespace MultiStopWatchDemo
{
    class Program
    {
        private static readonly Random _random = new Random();

        static void Main()
        {
            // Lets simulate 100 iterations of an operation 
            // we don't care about timing followed by an 
            // operation we do want to time

            var multiStopwatch = new MultiStopwatch();

            WL("Starting tests...");

            for(int i=0; i<100; i++)
            {
                // this part we don't care about timing
                PrepareData();

                // this part we want to time
                multiStopwatch.Start();
                ProcessData();
                multiStopwatch.Stop();

                if (i % 10 == 9)
                {
                    WL("Finished iteration {0}.", i + 1);
                }
            }

            WL("Completed timing operations");
            WL();
            WL("Average time: {0} ms", multiStopwatch.AverageMilliseconds);
            WL("Average time: {0} ticks", multiStopwatch.AverageTicks);
            WL();
            WL("Total time  : {0} ms", multiStopwatch.ElapsedMilliseconds);
            WL("Total time  : {0} ticks", multiStopwatch.ElapsedTicks);
        }

        private static void PrepareData()
        {
            // simulate some lengthy operation where we prepare some data
            // but don't care about timing
            Thread.Sleep(_random.Next(10, 50));
        }

        private static void ProcessData()
        {
            // simulate an operation that is consistent and we want to time
            Thread.Sleep(_random.Next(20, 30));
        }

        private static void WL()
        {
            Console.WriteLine();
        }
        private static void WL(string message)
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.ffff - ") + message);
        }
        private static void WL(string message, params object[] args)
        {
            WL(string.Format(message, args));
        }
    }
}
