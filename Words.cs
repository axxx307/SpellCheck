using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace spell_check
{
    public class Words
    {
        public List<string> words;
        public Dictionary<string, int> counter;

        public Words(string word) 
        {
            GetWords(word);
            Counter();
        } 

        private void GetWords(string word)
        {
            words =  File.ReadAllLines("words.txt").Select(x=>x.Replace(@"\", "")).ToList();
        }

        private void Counter()
        {
            var file = File.ReadAllLines("big.txt").SelectMany(x=>x.Split(" ")).ToArray();
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < file.Length; i++)
            {
                if (!dict.ContainsKey(file[i]))
                {
                    dict.Add(file[i], 0);
                }
                else
                {
                    dict[file[i]]++;
                }
            }
            counter = dict;
        }
    }
}