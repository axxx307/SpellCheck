using System.Collections.Generic;
using System.Linq;

namespace SpellCheckLibrary.SpelingClasses
{
    public class Trie
    {
        private TrieNode _node;

        public Trie() => _node = new TrieNode();
        public class TrieNode
        {
            public HashSet<TrieNode> Children = null;

            public string Value = string.Empty;

            public bool IsWord = false; 

            public long Frequency = 0;
        }

        public void Add(TrieNode node, string word, long frequency)
        {
            if (node.Children?.FirstOrDefault(x=>x.Value == word[0].ToString()) == null)
            {
                var newNode = new TrieNode
                {
                    Value = word[0].ToString(),
                    IsWord = word.Length == 1,
                    Frequency = word.Length == 1 ? 0 : frequency
                };
                node.Children.Add(newNode);
            }
            else 
            {
                
            }
            if (word.Length > 1)
            {
                Add(node.Children.First(x=>x.Value == word[0].ToString()), word.Remove(0, 1)[0].ToString(), frequency);
            }
        }
    }
}