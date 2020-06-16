using System;
using System.Collections.Generic;
using System.Text;

namespace Lab0612
{
    class WordsContainer
    {
        private Dictionary<string, int> _wordToCount = new Dictionary<string, int>();
        public int TotalWords { get; private set; } = 0;
        public string MostFrequentWord { get; private set; }
        private int _mostFreqWordCount = 0;

        public void Add(string word)
        {
            int count = _wordToCount.GetValueOrDefault(word, 0);
            if (count + 1 > _mostFreqWordCount)
            {
                MostFrequentWord = word;
                _mostFreqWordCount = count + 1;
            }
            _wordToCount.Remove(word);
            _wordToCount.Add(word, count + 1);
            TotalWords++;
        }

        public int GetCountOf(string word)
        {
            return _wordToCount.GetValueOrDefault(word, 0);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{ ");
            foreach (var pair in _wordToCount)
            {
                builder.Append("\n   ");
                builder.Append(pair.Key);
                builder.Append(": ");
                builder.Append(pair.Value);
                builder.Append(";");
            }
            builder.Append("\n}");

            return builder.ToString();
        }
    }
}
