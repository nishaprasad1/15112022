class Program
{
    private static Task DoComputation(int limit)
    {
        Console.Write("Computing...");
        Console.WriteLine("In Computaion Function Task Done by : {0} Thread", Thread.CurrentThread.ManagedThreadId);
        var c = new Computation();
        var w = new System.Diagnostics.Stopwatch();
        w.Start();
        return c.ComputeAsync(1, limit)
                .ContinueWith(previous => 
                {   
                    long r = previous.Result;
                    w.Stop();
                    Console.WriteLine("After Compute Async completes Task Done by : {0} Thread", Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("Done!");
                    Console.WriteLine("Result = {0}, computed in {1:0.000} seconds.", r, w.Elapsed.TotalSeconds);
                });
    }

    static void Main(string[] args)
    {
        int n = int.Parse(args[0]);
        Console.WriteLine("In Main Task Done by : {0} Thread", Thread.CurrentThread.ManagedThreadId);
        var job = DoComputation(n);
        while(!job.IsCompleted)
        {
            Console.Write(". {0}",Thread.CurrentThread.ManagedThreadId );
            Thread.Sleep(500);
        }
    }

    class Computation
    {
        public long Compute(int first, int last)
        {
            Console.WriteLine("In Run in Compute from ComputeAsync Task Done by : {0} Thread", Thread.CurrentThread.ManagedThreadId);
            return Enumerable.Range(first, last - first + 1)
                        .Select(Worker.DoWork)
                        .Sum();
        }

        public Task<long> ComputeAsync(int first, int last)
        {
            Console.WriteLine("In Compute Async Task Done by : {0} Thread", Thread.CurrentThread.ManagedThreadId);
            //the operation passed to Run is invoked by a pooled
            //thread allowing the caller thread to resume and obtain
            //the result of invocation from the retuned Task once the
            //pooled thread completes that invocation
            return Task<long>.Run(() => Compute(first, last));
        }
    }
}