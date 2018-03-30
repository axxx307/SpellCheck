using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace spell_check.SpelingClasses
{
    public static class CorrectionTest
    {
        private static Dictionary<string, string> LoadData()
        {
            var dict = new Dictionary<string, string>();
            var lines =  File.ReadAllLines(@"TextFiles/errors.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                var split = lines[i].Split(" ");
                if (split.Length < 3)
                {
                    split = split.SelectMany(x=>x.Split(@"\t")).ToArray();
                }
                dict.Add(split[1], split[2]);
            }
            return dict;
        }

        public static void TestAgainstWords()
        {
            var cases = LoadData();
            var spell = new Spell();
            var words = new Words();
            int wrong = 0;
            int right = 0;
            for(int i = 0; i < cases.Count; i++)
            {
                var element = cases.ElementAt(i);
                var candidate = spell.Candidates(element.Key, words);
                if (candidate == element.Value)
                {
                    // System.Console.WriteLine("RIGHT -- " + candidate);
                    right++;
                }
                else 
                {
                    // System.Console.WriteLine("WRONG -- " + candidate);
                    wrong++;
                }
            }
            System.Console.WriteLine($"wrongs are {wrong}, rights are {right}");
        }
    }
}