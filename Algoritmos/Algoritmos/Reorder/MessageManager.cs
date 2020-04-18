using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Algoritmos.Reorder
{
    public static class MessageManager<T>
        where T : IMessage
    {
        private static int currentNumber = 1;

        private static List<T> receivedMessages = new List<T>();

        public static void ProcessMessage(T message, Action<T> execute)
        {
            lock (receivedMessages)
            {
                receivedMessages.Add(message);

                var messagesToSend = receivedMessages
                    .OrderBy(m => m.Num)
                    .Where((m, i) => (m.Num - i) == currentNumber)
                    .ToList();

                if (messagesToSend.Any())
                {
                    //Console.WriteLine("*** Enviar");
                }

                messagesToSend.ForEach(m =>
                {
                    receivedMessages.Remove(m);

                    execute(m);
                    Thread.Sleep(50);

                    currentNumber++;
                });
            }
        }
    }
}
