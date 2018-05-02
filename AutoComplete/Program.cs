using System;
using System.Diagnostics;
using Autofac;
using spell_check;
using spell_check.SpelingClasses;
using SpellCheckLibrary.SpelingClasses;

namespace AutoComplete
{
    class Program
    {
        static void Main(string[] args)
        {
            var symSpell = new SymSpell(2);

            var trie = new Trie();
            foreach (var word in symSpell.GeneratedFrequencies)
            {
                trie.Add(word.Key, word.Key, word.Value);
            }
            Console.WriteLine("autocomplete loaded succesfully");
            while(true)
            {
                Console.WriteLine("enter word");
                var word = Console.ReadLine();
                Console.WriteLine(string.Join(" ", trie.Get(word)));
            }

            //var isRunning = true;
            //while (isRunning)
            //{
            //    Console.WriteLine("Enter mode");
            //    var mode = Console.ReadLine();
            //    switch (mode)
            //    {
            //        case @"\test":
            //        {
            //            Console.WriteLine("Starting tests");
            //            var data = CorrectionTest.TestAgainstWords(symSpell.LookUp);
            //            Console.WriteLine($"Right: {data.Item1}; Wrong: {data.Item2}");
            //            continue;
            //        }
            //        case @"\norvig":
            //        {
            //            var autofac = new AutoFac();
            //            var _container = autofac.AutoFacInit();
            //            var spells = _container.Resolve<Spell>();
            //            var words = new Words();
            //            Console.WriteLine("Enter the word");
            //            var word = Console.ReadLine();
            //            var spelling = spells.Candidates(word, words);
            //            Console.WriteLine($"Corrected to: {spelling}");
            //            continue;
            //        }
            //        case @"\symspell":
            //        {
            //            Console.WriteLine("You can start typing");
            //            var word = Console.ReadLine();
            //            var spelling = symSpell.LookUp(word);
            //            Console.WriteLine($"Corrected to: {spelling}");
            //            continue;
            //        }
            //        case @"\exit":
            //        {
            //            isRunning = false;
            //            break;
            //        }
            //    }
            //}
        }
    }
}
