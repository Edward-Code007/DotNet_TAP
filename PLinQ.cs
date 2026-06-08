using System.Diagnostics;
using System.Globalization;

namespace PLinQExample;

public class Plinq
{

    public static async Task Run()
    {
        var arreglo = Enumerable.Range(0, 10000);

        var parallelLinq = from number in arreglo
                                                .AsParallel()
                                                .AsUnordered()
                                                .WithDegreeOfParallelism(6)               
                           select number;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        parallelLinq.ForAll( async number =>
        {
           var result = await ExpensiveTaskEmu(number);
            Console.WriteLine($"From Thread {Thread.CurrentThread.ManagedThreadId} result: {result}");
        });
        stopwatch.Stop();
        Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");
    }
    private static async Task<int> ExpensiveTaskEmu(int number)
    {

        await Task.Delay(200);
        return number + number;
    }
}