using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;

namespace spell_check
{
    class Program
    {
        static void Main(string[] args)
        {
            var autofac = new AutoFac();
            var _container = autofac.AutoFacInit();
            var spells = _container.Resolve<Spell>();
            var words = new Words();

            Console.WriteLine("Enter word");
            var input = Console.ReadLine();
            var edits = spells.EditsForWord(input);
            var known = spells.Known(edits, words.words);
            var candidates = new List<string>();
            candidates.AddRange(known);
            candidates.AddRange(spells.Known(new List<string> { input }, words.words));
            candidates.Add(input);
            var f = new Dictionary<string, double>();
            foreach (var itemf in candidates)
            {
                if (!f.ContainsKey(itemf))
                {
                    f.Add(itemf, spells.Probability(itemf, words.counter));
                }
            }
            var item = f.FirstOrDefault(z=>z.Value == f.Values.Max()).Key;
            Console.WriteLine(item);
        }
    }
}
