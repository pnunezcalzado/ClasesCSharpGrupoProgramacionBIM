using System;
using System.Threading;

namespace Algoritmos.Reorder
{
    public class MockMessage : IMessage
    {
        public MockMessage(int num, string message)
        {
            Num = num;
            Message = message;
        }

        public int Num { get; }
        public string Message { get; }

        public void Execute()
        {
            var text = string.Join(" | ", this, DateTime.Now.ToString("mm:ss.fff"), "Thread: " + Thread.CurrentThread.ManagedThreadId);

            Thread.Sleep(100);

            Console.WriteLine(text);
        }

        public override string ToString()
        {
            return string.Join("\t", Num, Message);
        }
    }
}
