using Microsoft.VisualBasic;

namespace ParallelExample.For;

public class ParallelForEach
{
    public static void Execute()
    {

        Random rng = new Random();
        int[] numeros = Enumerable.Range(0, 100)
                                  .Select(_ => rng.Next(0, 101))
                                  .ToArray();
        Parallel.ForEachAsync(numeros, (number, canceToken) =>
        {
            Console.WriteLine($"Completed From Thread {Thread.CurrentThread.ManagedThreadId}: {number} * 2 equals ={number*2}");
           
            return ValueTask.CompletedTask;
        }); //Implementar For Paralelo
    }
}