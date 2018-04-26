using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;

namespace spell_check.SpelingClasses
{
    public class SymSpell
    {
        private int _editDistance;
        private int Threshold = 0;
        private Dictionary<int, string[]> _generatedEdtis;
        private Dictionary<string, long> _generatedFrequencies;

        public Dictionary<string, long> GeneratedFrequencies {get; private set;}

        public SymSpell(int editDistance)
        {
            _editDistance = editDistance;
            _generatedEdtis = new Dictionary<int, string[]>();
            _generatedFrequencies = new Dictionary<string, long>();
            LoadFrequencies();
            Console.WriteLine("SymSpell files loaded successfully");
            GeneratedFrequencies = _generatedFrequencies;
        }

        public string LookUp(string word)
        {
            if (_generatedFrequencies.ContainsKey(word)) return word;
            var size = word.Length * (word.Length - 1) / 2;
            var deletes = Edits(word, 0, new HashSet<string>());
            foreach (var delete in deletes)
            {
                var hash = delete.GetHashCode();
                string[] value = null;
                if (_generatedEdtis.TryGetValue(hash, out value))
                {
                    if (value.Length == 1)
                    {
                        return value[0];
                    }
                    else
                    {
                        var freqs = _generatedFrequencies.Where(f=>value.FirstOrDefault() == f.Key).Max();
                        return freqs.Key;
                    }
                }
            }
            return "Not found";
        }

        private HashSet<string> Edits(string word, int distance, HashSet<string> set)
        {
            distance++;
            for (int i = 0; i < word.Length; i++)
            {
                var edited = word.Remove(i, 1);
                if (set.Add(edited))
                {
                    if (distance < _editDistance) Edits(edited, distance, set);
                }
            }
            return set;
        }

        private void LoadFrequencies()
        {
            var lines = File.ReadAllLines(@"../SpellCheckLibrary/TextFiles/frequency_dictionary.txt");
            for (long i = 0; i < lines.Length; i++)
            {
                var wordF = lines[i].Split(" ");
                if (wordF.Length < 2) continue;
                if (long.Parse(wordF[1]) < Threshold) continue;
                _generatedFrequencies.Add(wordF[0], long.Parse(wordF[1]));
                var deletes = Edits(wordF[0], 0, new HashSet<string>());
                foreach (var delete in deletes)
                {
                    var hash = delete.GetHashCode();
                    string[] value = null;
                    if (_generatedEdtis.TryGetValue(hash, out value))
                    {
                        var extendedEdits = new string[value.Length + 1];
                        Array.Copy(value, extendedEdits, value.Length);
                        extendedEdits[value.Length] = wordF[0];
                        _generatedEdtis[hash] = extendedEdits;
                    }
                    else
                    {
                        _generatedEdtis.Add(hash, new string[1] {wordF[0]});
                    }
                }
            }
        }
    }
}