using System;
using System.Linq;

namespace spell_check.SpelingClasses
{
    public static class DamerauLevenshtein
    {
        public static int Distance(string word, string edited)
        {
            var array = new int[word.Length+1, edited.Length+1];
            for (int i = 1; i <= word.Length; i++)
            {
                array[i, 0] = i;
                for (int j = 1; j <= edited.Length; j++)
                {
                    int cost;
                    if (word[i-1] == edited[j-1])
                    { 
                        cost = 0;
                    }
                    else 
                    {
                        cost = 1;
                        array[0, j] = j;
                    }
                    array[i, j] = new[] 
                    {
                        array[i-1, j] + 1,
                        array[i, j-1] + 1,
                        array[i-1, j-1] + cost
                    }.Min();
                    if (i > 1 && j > 1 && word[i-1] == edited[j-2] && word[i-2] == edited[j-1])
                    {
                        array[i, j] = new[] {array[i, j], array[i-2, j-2] + cost}.Min();
                    }
                }
            }
            return array[word.Length, edited.Length];
        }
    }
}