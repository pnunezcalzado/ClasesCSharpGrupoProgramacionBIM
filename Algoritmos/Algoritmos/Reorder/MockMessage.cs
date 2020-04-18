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

        public override string ToString()
        {
            return string.Join("\t", Num, Message);
        }
    }
}
