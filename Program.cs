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

<<<<<<< HEAD
            Console.WriteLine("Enter word");
            // var input = Console.ReadLine();
            var sym = new SymSpell(1);
            while (true)
            {
                var word = Console.ReadLine();
                var length = sym.P(word);
                Console.WriteLine(length);
            }
=======
            Console.WriteLine("Files were successfully loaded");
            var isRunning = true;
            Console.WriteLine("Enter mode");
            while (isRunning)
            {
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
                    case @"\type":
                    {
                        Console.WriteLine("Enter the word");
                        var word = Console.ReadLine();
                        var spelling = spells.Candidates(word, words);
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
>>>>>>> 7d62d8c1869ce8aeba9b64cf4b70904548909318
        }
    }
}
