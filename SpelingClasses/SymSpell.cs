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
        private int Threshold = 300000;
        private Dictionary<int, string[]> _generatedEdtis;
        private Dictionary<string, long> _generatedFrequencies;
        public SymSpell(int editDistance)
        {
            _editDistance = editDistance;
            _generatedEdtis = new Dictionary<int, string[]>();
            _generatedFrequencies = new Dictionary<string, long>();
            LoadFrequencies();
            Console.WriteLine("SymSpell files loaded successfully");
        }

        public string LookUp(string word)
        {
            if (_generatedFrequencies.ContainsKey(word)) return word;
            var size = word.Length * (word.Length - 1) / 2;
            var deletes = GenerateEditsForWord(word, null, new HashSet<string>());
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
                        return value[0];
                    }
                }
            }
            return "Not found";
        }

        private HashSet<string> GenerateEditsForWord(string word, string editedWord, HashSet<string> set)
        {
            var deletes = editedWord != null ? Deletes(editedWord) : Deletes(word);
            for (int i = 0; i < deletes.Length; i++)
            {
                var distance = DamerauLevenshtein.Distance(word, deletes[i]);
                if (distance > _editDistance || string.IsNullOrWhiteSpace(deletes[i]))
                {
                    break;
                }

                set.Add(deletes[i]);
                GenerateEditsForWord(word, deletes[i], set);
            }
            return set;
        }

        private string[] Deletes(string word)
        {
            var array = new string[word.Length];
            for (int i = 0; i < word.Length; i++)
            {
                array[i] = word.Remove(i, 1);
            }
            return array;
        }

        private void LoadFrequencies()
        {
            var lines = File.ReadAllLines(@"TextFiles/frequency_dictionary.txt");
            for (long i = 0; i < lines.Length; i++)
            {
                var wordF = lines[i].Split(" ");
                if (wordF.Length < 2) continue;
                if (long.Parse(wordF[1]) < Threshold) continue;
                _generatedFrequencies.Add(wordF[0], long.Parse(wordF[1]));
                var deletes = GenerateEditsForWord(wordF[0], null, new HashSet<string>());
                foreach (var delete in deletes)
                {
                    var hash = delete.GetHashCode();
                    string[] value = null;
                    if (_generatedEdtis.TryGetValue(hash, out value))
                    {
                        var extendedEdits = new string[value.Length + 1];
                        Array.Copy(value, extendedEdits, value.Length);
                        extendedEdits[value.Length] = delete;
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