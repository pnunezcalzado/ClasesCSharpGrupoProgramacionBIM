using System.Collections.Generic;
using System.Linq;

namespace Algoritmos.Speech
{
    class TextElement
    {
        public static IEnumerable<TextElement> ProcessSentence(string text, string[] keys)
        {
            TextElement.keys = keys;
            maxWords = keys.Max(k => k.Split(' ').Length);
            textSplit = text.Split(' ');

            textElements = textSplit
              .Select((word, index) => new TextElement(index))
              .Where(element => element.IsKeyword)
              .ToList();

            return textElements;
        }

        private TextElement(int index)
        {
            StartIndex = index;
        }

        private static IEnumerable<string> keys;
        private static int maxWords;
        private static IEnumerable<string> textSplit;
        private static IEnumerable<TextElement> textElements;
        public int StartIndex { get; }
        public int EndIndex {
            get 
            {
                var index = textElements
                    .OrderBy(e => e.StartIndex)
                    .ToList()
                    .IndexOf(this);
                return textElements.ElementAtOrDefault(index + 1)?.StartIndex ?? textSplit.Count();
            }
        }
        public int Range { get { return EndIndex - StartIndex; } }
        public IEnumerable<string> Comparators {
            get 
            {
                return Enumerable.Range(1, maxWords)
                    .Select(n => textSplit.Skip(StartIndex).Take(n))
                    .Select(c => string.Join(" ", c));
            }
        }
        public bool IsKeyword { get { return Comparators.Any(c => keys.Contains(c)); } }
        public string Sentence { get { return string.Join(" ", textSplit.Skip(StartIndex).Take(Range)); } }
    }
}
