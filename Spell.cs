using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.DynamicProxy;

namespace spell_check
{
    [Intercept(typeof(LoggerInterceptor))]
    public  class Spell
    {
        private const string Letters = "abcdefghijklmnopqrstuvwxyz";
        
        private List<string> Words;

        public Spell(List<string> words)
        {
            Words = words;
        }

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

        public List<string> Known(List<string> l)
        {
            return l.SelectMany(x=>Words.Where(z=>z == x)).ToList();
        }
        public  List<string> EditsForWord(string word)
        {
            var deletes = Deletes(word);
            var transposes = Transposes(word);
            var replaces = Replaces(word);
            var inserts = Inserts(word);
            var lst = new List<string>();
            lst.AddRange(deletes);
            lst.AddRange(transposes);
            lst.AddRange(replaces);
            lst.AddRange(inserts);
            return lst;
        }

        private Dictionary<string, string> Splits(string word)
        {
            var dict = new Dictionary<string, string>();
            for (int i = 0; i < word.Length + 1; i++)
            {
                var subLeft = word.Substring(0, i);
                var subRight = word.Substring(i, word.Length - i);
                dict.Add(subLeft, subRight);
            }
            return dict;
        }

        private  List<string> Deletes(string word)
        {
            var list = new List<string>();
            foreach (var letter in word)
            {
                list.Add(word.Remove(word.IndexOf(letter), 1));
            }
            return list;
        }

        private  List<string> Transposes(string word)
        {
            var list = new List<string>();
            var w = word.ToList();
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
                    list.Add(str);    
                }
            }
            return list;
        }

        private  List<string> Replaces(string word)
        {
            var list = new List<string>();
            word.ToList().ForEach(x=>
            {
                var index = word.IndexOf(x);
                for (int i = 0; i < Letters.Length; i++)
                {
                    var str = word;
                    str = str.Remove(index, 1);
                    list.Add(str.Insert(index, Letters[i].ToString()));
                }
            });
            return list;
        }

        private  List<string> Inserts(string word)
        {
            var lst = new List<string>();
            word.ToList().ForEach(x=>
            {
                var index = word.IndexOf(x);
                for (int i = 0; i < Letters.Length; i++)
                {
                    lst.Add(word.Insert(index, Letters[i].ToString()));
                }
            });
            return lst;
        }
    }
}