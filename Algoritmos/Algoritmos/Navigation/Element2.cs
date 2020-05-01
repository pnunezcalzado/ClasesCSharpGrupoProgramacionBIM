namespace Navigation
{
    public class Element2 : ISelect
    {
        private string content = "X";

        public bool IsSelected { get; set; } = false;

        public override string ToString()
        {
            return IsSelected ? ($"[{content}]") : ($" {content} ");
        }
    }
}