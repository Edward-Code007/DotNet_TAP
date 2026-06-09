using System.Collections.Concurrent;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace ParallelExample.For;

public class ParallelForEach
{
    private static async Task ExecuteFor(ConcurrentBag<string> bag)
    {
        Random rng = new Random();
        int[] numeros = Enumerable.Range(0, 100)
                                  .Select(_ => rng.Next(0, 101))
                                  .ToArray();
       Parallel.ForEach(numeros, (number, canceToken) =>
        {
            var cadena = $"Completed From Thread {Thread.CurrentThread.ManagedThreadId}: {number} * 2 equals ={number * 2}";
            bag.Add(cadena);
        });
    }
    private static Task TryReadBag(ConcurrentBag<string> concurrentBag, CancellationToken cancellationToken)
    {

        int errCount = 0;
        object _lock = new object();
        while (errCount < 10)
        {
            if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);
            
            Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                bool isTaken = concurrentBag.TryTake(out string? cadena);
                if (isTaken) Console.WriteLine(cadena);
                lock (_lock)
                {
                    errCount++;
                }

            },
            cancellationToken);
        }
        Console.WriteLine("Threshold Alcanzado");
        return Task.CompletedTask;
    }
    public static async Task Run()
    {
        var concurrentBag = new ConcurrentBag<string>();

       await ExecuteFor(concurrentBag);

        CancellationTokenSource source = new CancellationTokenSource();
        source.CancelAfter(2);//Controlar si se Cancela antes de q las tareas Terminen

        var result = TryReadBag(concurrentBag, source.Token);

        result.ContinueWith(completed =>
        {
             if (completed.IsCanceled)
            {
                Console.WriteLine("Tarea Cancelada");
            }
            else if (completed.IsCompletedSuccessfully)
            {
                Console.WriteLine("Tarea Completada con Exito");
            }
            
        });
    }
}