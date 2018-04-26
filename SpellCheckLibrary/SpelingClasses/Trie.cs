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

            public bool IsWord = false; 

            public long Frequency = 0;
        }

        public void Add(string word, long frequency)
        {
            if (_currentNode.Children?.FirstOrDefault(x=>x.Value == word[0].ToString()) == null)
            {
                var newNode = new TrieNode
                {
                    Value = word[0].ToString(),
                    IsWord = word.Length == 1,
                    Frequency = word.Length == 1 ? 0 : frequency
                };
                _currentNode.Children.Add(newNode);
            }
            if (word.Length > 1)
            {
                _currentNode = _currentNode.Children.First(x => x.Value == word[0].ToString());
                Add(word.Remove(0, 1), frequency);
            }
            _currentNode = _node;
        }

        public HashSet<string> Get(string word)
        {
            TrieNode nodeWord = new TrieNode();
            var modified = word.Clone().ToString();
            while(!nodeWord.IsWord)
            {
                nodeWord =  _currentNode.Children.First(x=>x.Value == modified[0].ToString());
                modified = modified.Remove(0, 1);
                _currentNode = nodeWord;
            }
            if (nodeWord.Children != null && !nodeWord.Children.Any(x=>x.IsWord))
            {
                Debug.WriteLine("implement finding deaper");
                return new HashSet<string>();
            }
            _currentNode = _node;
            return nodeWord.Children.Where(f=>f.IsWord).OrderBy(f=>f.Frequency).Select(z=> word +z.Value).Take(3).ToHashSet();
        }
    }
}