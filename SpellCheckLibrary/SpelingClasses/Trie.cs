using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SpellCheckLibrary.SpelingClasses
{
    public class Trie
    {
        private readonly TrieNode _node;
        private TrieNode _currentNode;

        public Trie()
        {
            _node = new TrieNode();
            _currentNode = _node;
        }

        public class TrieNode
        {
            public HashSet<TrieNode> Children = new HashSet<TrieNode>();

            public string Value = string.Empty;

            public List<TrieNodeWord> Words = new List<TrieNodeWord>();
        }

        public class TrieNodeWord
        {
            public string Word = string.Empty;

            public long Frequency = 0;
        }

        public void Add(string originalWord, string word, long frequency)
        {
            if (_currentNode.Children?.FirstOrDefault(x=>x.Value == word[0].ToString()) == null)
            {
                var newNode = new TrieNode{ Value = word[0].ToString() };
                if (word.Length == 1) newNode.Words.Add(new TrieNodeWord{ Word = originalWord, Frequency = frequency });
                _currentNode.Children.Add(newNode);
            }
            if (word.Length > 1)
            {
                _currentNode = _currentNode.Children.First(x => x.Value == word[0].ToString());
                Add(originalWord, word.Remove(0, 1), frequency);
            }
            _currentNode = _node;
        }

        public HashSet<string> Get(string word)
        {
            TrieNode nodeWord = new TrieNode();
            var modified = word.Clone().ToString();
            while(modified.Length != 0)
            {
                nodeWord =  _currentNode.Children.First(x=>x.Value == modified[0].ToString());
                modified = modified.Remove(0, 1);
                _currentNode = nodeWord;
            }
            if (nodeWord.Children != null && nodeWord.Children.Any(x=>x.Words.Count > 0))
            {
                int depth = 0;
                HashSet<string> suggestions = new HashSet<string>() {word};
                while(depth != 2)
                {
                    depth++;
                    var childValues = _currentNode.Children.Where(f=>f.IsWord).Select(x=> x.Value);
                    foreach (var item in suggestions)
                    {
                        childValues.Select(f=> suggestions.Add(item + f));
                    }
                    
                }
                _currentNode = _node;
                return suggestions;
            }
            _currentNode = _node;
            return nodeWord.Children.Where(f=>f.IsWord).OrderBy(f=>f.Frequency).Select(z=> word +z.Value).Take(3).ToHashSet();
        }
    }
}