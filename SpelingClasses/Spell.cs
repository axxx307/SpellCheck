using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.DynamicProxy;

namespace spell_check
{
    [Intercept(typeof(SpellDecorator))]
    public  class Spell
    {
        private const string Letters = "abcdefghijklmnopqrstuvwxyz";

        public double Probability(string word, Dictionary<string, int> dict)
        {
            var sum = dict.Values.Sum();
            if (!dict.ContainsKey(word)) return 0;
            return (double) dict[word] / sum;
        }

        public List<string> Edits2(List<string> edits)
        {
            var el = edits.SelectMany(x=>EditsForWord(x)).ToList();
            return el;
        }

        public List<string> Known(List<string> l, List<string> words)
        {
            return l.SelectMany(x=>words.Where(z=>z == x)).ToList();
        }
        public List<string> EditsForWord(string word)
        {
            var lst = new List<string>();
            lst.AddRange(Deletes(word));
            lst.AddRange(Transposes(word));
            lst.AddRange(Replaces(word));
            lst.AddRange(Inserts(word));
            return lst;
        }
        
        public virtual string[] Deletes(string word)
        {
            var array = new string[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                array[i] = word.Remove(i, 1);
            }
            return array;
        }

        public virtual string[] Transposes(string word)
        {
            var arr = new string[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                var str = word;
                var left = word[i];
                if (i + 1 < word.Length)
                {
                    var right = word[i+1];
                    var indexL = word.IndexOf(left);
                    var indexR = word.IndexOf(right);
                    str = str.Remove(indexL, 1);
                    str = str.Remove(indexL, 1);
                    str = str.Insert(indexL, right.ToString());
                    str = str.Insert(indexR, left.ToString());
                    arr[i] = str;
                }
            }
            return arr;
        }

        public virtual string[] Replaces(string word)
        {
            var arr = new string[word.Length * Letters.Length];
            var index = 0;
            for (int i = 0; i < word.Length; i++)
            {
                var str = word.Clone().ToString();
                str = str.Remove(i, 1);
                for (int j = 0; j < Letters.Length; j++)
                {
                    arr[index] = str.Insert(i, Letters[j].ToString());
                    index++;
                }
            };
            return arr;
        }

        public virtual string[] Inserts(string word)
        {
            var arr = new string[word.Length * Letters.Length];
            var index = 0;
            for (int i = 0; i < word.Length; i++)
            {
                for (int j = 0; j < Letters.Length; j++)
                {
                    arr[index] = word.Insert(i, Letters[j].ToString());
                    index++;
                }
            };
            return arr;
        }

                // private Dictionary<string, string> Splits(string word)
        // {
        //     var dict = new Dictionary<string, string>();
        //     for (int i = 0; i < word.Length + 1; i++)
        //     {
        //         var subLeft = word.Substring(0, i);
        //         var subRight = word.Substring(i, word.Length - i);
        //         dict.Add(subLeft, subRight);
        //     }
        //     return dict;
        // }
    }
}