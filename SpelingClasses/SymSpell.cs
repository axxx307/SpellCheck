using System;
using System.Collections.Generic;

namespace spell_check.SpelingClasses
{
    public class SymSpell
    {
        private int _editDistance;
        public SymSpell(int editDistance)
        {
            _editDistance = editDistance;
        }
        
        public Dictionary<string, int> P(string word)
        {
            var d = Deletes(word);
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < d.Length; i++)
            {
                var edit2 = Deletes(d[i]);
                for (int j = 0; i < edit2.Length; j++)
                {
                    dict.Add(edit2[j], DamerauLevenshtein.Distance(word, edit2[j]));    
                }
                dict.Add(d[i], DamerauLevenshtein.Distance(word, d[i]));
            }
            return dict;
        }

        private string[] Deletes(string word)
        {
            var array = new string[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                array[i] = word.Remove(i, 1);
            }
            return array;
        }
    }
}