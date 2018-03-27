using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace spell_check
{
    class SpellDecorator: DynamicDecorator<Spell>
    {
        public SpellDecorator(Spell component): base(component)
        {
            
        }
    }
}