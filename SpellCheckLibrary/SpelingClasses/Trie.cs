using System;
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
            //Find needed word iterating over letters
            while(modified.Length != 0)
            {
                nodeWord =  _currentNode.Children.First(x=>x.Value == modified[0].ToString());
                modified = modified.Remove(0, 1);
                _currentNode = nodeWord;
            }
            _currentNode = _node;
            try
            {
                HashSet<TrieNodeWord> set = new HashSet<TrieNodeWord>();
                //Find closest set of words
                set.UnionWith(nodeWord.Children.SelectMany(f=>f.Children).SelectMany(f=>f.Words).ToHashSet());
                //Find closest child`s set of words
                set.UnionWith(nodeWord.Children.SelectMany(z=>z.Words).ToHashSet());
                return set.OrderByDescending(f=>f.Frequency).Select(z=>z.Word).ToHashSet();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}