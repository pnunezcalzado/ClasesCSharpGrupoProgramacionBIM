using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;

namespace Algoritmos.Reorder
{
    class ReorderMain
    {
        public static void Execute()
        {
            // Generacion de lista de numeros desordenada
            var quantity = 100;
            var variance = 3;
            var numbers = Enumerable.Range(1, quantity).ToList();

            var random = new Random();
            while (numbers.Count > 0)
            {
                var index = random.Next(0, variance);
                var num = numbers.ElementAtOrDefault(index);
                if (num != 0)
                {
                    numbers.RemoveAt(index);
                    messages.Enqueue(new MockMessage(num, Guid.NewGuid().ToString().Substring(0, 4)));
                }
            }

            Console.WriteLine("Desordenados:\n");
            Console.WriteLine(string.Join(", ", messages.Select(m => m.Num)));
            Console.WriteLine("\nOrdenados:\n");

            // Timer para crear mock de la recepcion de mensajes
            timer.Elapsed += Launch;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

            Console.ReadLine();
        }

        private static System.Timers.Timer timer = new System.Timers.Timer(1);

        private static Queue<MockMessage> messages = new Queue<MockMessage>();

        private static void Launch(object sender, ElapsedEventArgs e)
        {
            if (!messages.Any())
            {
                timer.Stop();
                Console.WriteLine("*** Recibidos todos los mensajes!");
            }
            else
            {
                var message = messages.Dequeue();

                // Invocacion para cada mensaje recibido
                MessageManager<IMessage>
                    .ProcessMessage(message, m =>
                    {
                        var text = string.Join(" | ", m, DateTime.Now.ToString("mm:ss.fff"),"Thread: " + Thread.CurrentThread.ManagedThreadId);
                        Console.WriteLine(text);
                    });
            }
        }
    }
}
