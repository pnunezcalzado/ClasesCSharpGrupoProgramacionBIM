using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    _messages.Enqueue(new MockMessage(num, $"numero {num.ToString()}"));
                }
            }

            Console.WriteLine("desordenados:");
            Console.WriteLine(String.Join(", ", _messages.Select(m => m.Num)));
            Console.WriteLine("ordenados:");

            // Invocacion solo en caso necesario (aqui no hacia falta). Es posible resetear con numero inicial
            MessageManager<MockMessage>.Reset();

            // Timer para crear mock de la recepcion de mensajes
            timer.Elapsed += Launch;
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Start();

            Console.ReadLine();
        }

        private static Timer timer = new Timer(50);

        private static Queue<MockMessage> _messages = new Queue<MockMessage>();

        private static void Launch(object sender, ElapsedEventArgs e)
        {
            if (!_messages.Any())
            {
                timer.Stop();
            }
            else
            {
                var message = _messages.Dequeue();

                // Invocacion para cada mensaje recibido
                MessageManager<MockMessage>
                    .ProcessMessage(message.Num, message, m =>
                    {
                        var text = string.Join(" ", m, DateTime.Now.ToString("mm:ss.fff"));
                        Console.WriteLine(text);
                    });
            }
        }
    }
    public class MockMessage
    {
        public MockMessage(int num, string message)
        {
            Num = num;
            Message = message;
        }

        public int Num { get; }
        public string Message { get; }

        public override string ToString()
        {
            return Message;
        }
    }

    public static class MessageManager<TManager>
        where TManager : class
    {
        private class MessageWrapper<TWrapper>
            where TWrapper : class
        {
            public MessageWrapper(int number, TWrapper message)
            {
                Number = number;
                Message = message;
            }

            public int Number { get; }
            public TWrapper Message { get; }

            public override string ToString()
            {
                return Message.ToString();
            }
        }

        private static int _currentNumber = 1;

        private static List<MessageWrapper<TManager>> _receivedMessages = new List<MessageWrapper<TManager>>();

        public static void ProcessMessage(int number, TManager message, Action<TManager> execute)
        {
            var messageContainer = new MessageWrapper<TManager>(number, message);
            _receivedMessages.Add(messageContainer);

            var messagesToSend = _receivedMessages
                .OrderBy(m => m.Number)
                .Where((m, i) => (m.Number - i) == _currentNumber)
                .ToList();

            messagesToSend.ForEach(m =>
            {
                _receivedMessages.Remove(m);
                execute(m.Message);
                _currentNumber++;
            });
        }

        public static void Reset(int num = 1)
        {
            _currentNumber = num;
            _receivedMessages = new List<MessageWrapper<TManager>>();
        }
    }
}
