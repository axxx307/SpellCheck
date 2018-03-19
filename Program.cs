using System;
using System.Collections.Generic;
using System.Linq;

namespace spell_check
{
    class Program
    {
        static void Main(string[] args)
        {
            var word = Console.ReadLine();
            var words = new Words(word);
            var spell = new Spell(words.words);
            var edits = spell.EditsForWord(word);
            var known = spell.Known(edits);
            var candidates = new List<string>();
            candidates.AddRange(known);
            candidates.AddRange(spell.Known(new List<string> {word}));
            candidates.Add(word);
            var f = new Dictionary<string, double>();
            foreach (var itemf in candidates)
            {
                if (!f.ContainsKey(itemf))
                {
                    f.Add(itemf, spell.Probability(itemf, words.counter));
                }
            }
            var item = f.FirstOrDefault(z=>z.Value == f.Values.Max()).Key;
            Console.WriteLine(item);
            // way to much variants, need to check implementation
            // var edits2 = spell.Edits2(edits);
            // var know2 = spell.Known(edits2);
        }
    }
}
