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
            var sym = new SymSpell(1);
            while (true)
            {
                var word = Console.ReadLine();
                var length = sym.P(word);
                Console.WriteLine(length);
            }
        }
    }
}
