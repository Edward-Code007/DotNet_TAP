namespace CancellationTokenExample;

public class CancellationToken
{
    public static void Run()
    {
        CancellationTokenSource sourceToken = new CancellationTokenSource();
        sourceToken.CancelAfter(20);//Cambiar Valor Altera el Resultado del Task


        var task = Task.Factory.StartNew(() =>
        {
            int j = 0;
            for (int i = 0; i < int.MaxValue; i++)
            {
                sourceToken.Token.ThrowIfCancellationRequested();
                int childNUmber = j;
                Task.Factory.StartNew(() =>
        {
                Console.WriteLine($"Log dentro del Task Child #{childNUmber}");
            }, TaskCreationOptions.AttachedToParent);
                j++;
            }
        }, sourceToken.Token);

        task.ContinueWith(x =>
        {
            if (x.IsCompletedSuccessfully)
            {
                Console.WriteLine("Completado Exitosamente");
            }
            else if (x.IsCanceled)
            {
                Console.WriteLine("Cancelada");
            }
            else if (x.IsFaulted)
            {
                Console.WriteLine("Falló con excepción: " + x.Exception?.InnerException?.Message);
            }
        });

        Console.ReadLine();

    }
}