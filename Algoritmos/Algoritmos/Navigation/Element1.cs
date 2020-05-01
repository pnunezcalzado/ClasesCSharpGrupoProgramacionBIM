namespace Navigation
{
    public class Element1 : ISelect
    {
        private string content = "O";

        public bool IsSelected { get; set; } = false;

        public override string ToString()
        {
            return IsSelected ? ($"[{content}]") : ($" {content} ");
        }
    }
}