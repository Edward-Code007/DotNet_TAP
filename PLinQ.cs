using System.Diagnostics;

namespace PLinqExample;

public class Plinq
{
    public static void Run()
    {
        var numbers = Enumerable.Range(0, 50); // menos items, más claro

        var stopwatch = Stopwatch.StartNew();

        var tasks = numbers
            .AsParallel()
            .AsUnordered()
            .WithDegreeOfParallelism(6)
            .Select(n => ExpensiveTaskEmu(n))
            .ToList();



        stopwatch.Stop();

        foreach (var result in tasks)
            Console.WriteLine($"Result: {result}");

        Console.WriteLine($"Elapsed: {stopwatch.Elapsed}");

    }

    private static int ExpensiveTaskEmu(int number)
    {
        Thread.Sleep(200);
        return number + number;
    }
}