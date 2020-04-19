using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

/*
 * Algoritmo para reordenar mensajes asincronicos
 * 
 * Existen servicios en la nube para comunicacion Real-time, como por ejemplo SignalR
 * https://azure.microsoft.com/en-us/services/signalr-service/
 * 
 * Este tipo de servicios invierten la arquitectura cliente-servidor, de forma que el servidor
 * puede enviar directamente mensajes instantaneos al cliente
 * 
 * Tiene un inconveniente, y es que no asegura que los mensajes se reciban en estricto orden
 * si se trata de mensajes muy rapidos. Por esto, hay que reordenarlos en el cliente tras
 * la recepcion
 * 
 * El objetivo de la practica es aprender genericos, interfaces, y orientacion a objetos
 */

namespace Algoritmos.Reorder
{
    class ReorderMain
    {
        public static void Execute()
        {
            GenerateMessages(100, 3);

            StartTimer(1);

            Console.ReadLine();
        }

        private static Timer timer = new Timer();

        private static Queue<MockMessage> messages = new Queue<MockMessage>();

        private static void GenerateMessages(int quantity, int variance)
        {
            // Generacion de lista de numeros desordenada
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
        }

        private static void StartTimer(int miliseconds)
        {
            // Setup y Start del Timer
            timer.Interval = miliseconds;
            timer.Elapsed += SendMessage;
            timer.AutoReset = true;
            timer.Enabled = true;
            Console.WriteLine("*** Start " + DateTime.Now.ToString("mm:ss.fff"));
            timer.Start();
        }

        private static void SendMessage(object sender, ElapsedEventArgs e)
        {
            if (!messages.Any())
            {
                timer.Stop();
                Console.WriteLine("*** End " + DateTime.Now.ToString("mm:ss.fff"));
            }
            else
            {
                var message = messages.Dequeue();

                // Invocacion para cada mensaje recibido
                MessageManager<IMessage>.ProcessMessage(message, m => m.Execute());
            }
        }
    }
}
