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

            Console.WriteLine("Enter word");
            // var input = Console.ReadLine();
            CorrectionTest.TestAgainstWords();
            // var candidate = spells.Candidates(input, words);
            // Console.WriteLine(candidate);
        }
    }
}
