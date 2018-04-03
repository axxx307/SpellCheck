using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using ShellProgressBar;

namespace spell_check.SpelingClasses
{
    public class SymSpell
    {
        private int _editDistance;
        private Dictionary<string, List<string>> _generatedEdtis;
        private Dictionary<string, long> _generatedFrequencies;
        public SymSpell(int editDistance)
        {
            _editDistance = editDistance;
            _generatedEdtis = new Dictionary<string, List<string>>();
            _generatedFrequencies = new Dictionary<string, long>();
            LoadFrequencies();
            for (int i = 0; i < _generatedFrequencies.Count; i++)
            {
                AddWordToDictionary(_generatedFrequencies.ElementAt(i).Key, null);
            } 
            Console.WriteLine("SymSpell files loaded successfully");
        }

        public string LookUp(string word)
        {
            if (_generatedFrequencies.ContainsKey(word)) return word;
            var size = word.Length * (word.Length - 1) / 2;
            var deletes = GenerateEditsForWord(word, null, new string[size], 0);
            for (int i = 0; i < deletes.Length; i++)
            {
                if (_generatedEdtis.ContainsKey(deletes[i]))
                {
                    var values = _generatedEdtis[deletes[i]];
                    if (values.Count > 1)
                    {
                        //
                    }
                    else
                    {
                        return _generatedEdtis[deletes[i]].First();
                    }
                }
            }
            return "Not found";
        }

        private string[] GenerateEditsForWord(string word, string editedWord, string[] arr, int k)
        {
            var deletes = editedWord != null ? Deletes(editedWord) : Deletes(word);
            for (int i = 0; i < deletes.Length; i++)
            {
                var distance = DamerauLevenshtein.Distance(word, deletes[i]);
                if (distance > _editDistance)
                {
                    break;
                }

                arr[k] = deletes[i];
                k++;
                GenerateEditsForWord(word, deletes[i], arr, k);
            }
            return arr;
        }
        
        private void AddWordToDictionary(string word, string editedWord)
        {
            var deletes = editedWord != null ? Deletes(editedWord) : Deletes(word);
            for (int i = 0; i < deletes.Length; i++)
            {
                var distance = DamerauLevenshtein.Distance(word, deletes[i]);
                if (distance > _editDistance)
                {
                    break;
                }

                if (_generatedEdtis.ContainsKey(deletes[i]))
                {
                    _generatedEdtis[deletes[i]].Add(word);
                }
                else
                {
                    _generatedEdtis.Add(deletes[i], new List<string>(){word});
                }
                AddWordToDictionary(word, deletes[i]);
            }
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
            var lines = File.ReadAllLines(@"TextFiles\frequency_dictionary.txt");
            for (long i = 0; i < lines.Length; i++)
            {
                var wordF = lines[i].Split(" ");
                if (wordF.Length < 2) continue;
                _generatedFrequencies.Add(wordF[0], long.Parse(wordF[1]));
            }
        }
    }
}