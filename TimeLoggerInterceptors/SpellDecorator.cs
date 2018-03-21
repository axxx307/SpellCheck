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

        public virtual string[] Deletes(string word)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var deletes = Component.Deletes(word);
            stopWatch.Stop();
            WriteCharacters(stopWatch.Elapsed, "Deletes");
            return deletes;
        }

        private async void WriteCharacters(TimeSpan time, string methodName)
        {
            using (FileStream stream = new FileStream(@"Performance.txt", FileMode.Append, FileAccess.Write, FileShare.None, bufferSize:4096, useAsync: true))
            {
                var text = $"{methodName} -- {time} \n";
                await stream.WriteAsync(System.Text.Encoding.ASCII.GetBytes(text), 0, text.Length);
            }
        }
    }
}