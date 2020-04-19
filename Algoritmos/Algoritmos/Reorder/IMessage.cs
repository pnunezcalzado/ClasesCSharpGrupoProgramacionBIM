namespace Algoritmos.Reorder
{
    public interface IMessage
    {
        int Num { get; }
        string Message { get; }

        void Execute();
    }
}