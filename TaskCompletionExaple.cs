namespace TaskCompletionExaple
{
    public class TaskCompletion
    {

       private static TaskCompletionSource<bool> TaskDeclaration()
        {
            var tcs = new TaskCompletionSource<bool>();
            Task task = tcs.Task;

            task.ContinueWith(completed =>
            {
                
                if (completed.IsCompletedSuccessfully)
                {
                    Console.WriteLine("Se Completo Exitosamente");
                }
                else
                {
                    Console.WriteLine("Task Cancelada o Fallida");
                }

            });
            return tcs;
        }
        public static void Run()
        {
            var tcs = TaskDeclaration();
            while (true)
            {
            Console.WriteLine("1 para completar la tarea\n 2 para Cancelar");
                var opt = Console.ReadLine();
                if (opt == "1") { tcs.SetResult(true); }
                if (opt == "2") { tcs.SetCanceled(); }
            }
        }

    }

}