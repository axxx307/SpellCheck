using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using spell_check.SpelingClasses;

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
            var symSpell = new SymSpell(2);

            var isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("Enter mode");
                var mode = Console.ReadLine();
                switch (mode)
                {
                    case @"\test":
                    {
                        Console.WriteLine("Starting tests");
                        var data = CorrectionTest.TestAgainstWords();
                        Console.WriteLine($"Right: {data.Item1}; Wrong: {data.Item2}");
                        continue;
                    }
                    case @"\norvig":
                    {
                        Console.WriteLine("Enter the word");
                        var word = Console.ReadLine();
                        var spelling = spells.Candidates(word, words);
                        Console.WriteLine($"Corrected to: {spelling}");
                        continue;   
                    }
                    case @"\symspell":
                    {
                        Console.WriteLine("Enter the word");
                        var word = Console.ReadLine();
                        var spelling = symSpell.LookUp(word);
                        Console.WriteLine($"Corrected to: {spelling}");
                        continue;   
                    }
                    case @"\exit":
                    {
                        isRunning = false;
                        break;
                    }
                }
            }            
        }
    }
}
