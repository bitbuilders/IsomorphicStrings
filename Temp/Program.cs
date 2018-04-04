using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Temp
{
    class Program
    {
        static Dictionary<string, List<string>> exactIsomorphIds = new Dictionary<string, List<string>>();
        static Dictionary<string, List<string>> looseIsomorphIds = new Dictionary<string, List<string>>();
        static List<String> nonIsomorphs = new List<string>();

        static string filePath = "";

        static void Main(string[] args)
        {
            RunAllFunctions();
        }

        static void RunAllFunctions()
        {
            filePath = PromptUser("Enter in a file path to find all isomorphs in it:");
            List<string> words = ParseFile(filePath);
            FindIsomorphs(words);
            FindNonIsomorphs();
            PrintIsomorphs(exactIsomorphIds, "\nExact Isomorphs:");
            PrintIsomorphs(looseIsomorphIds, "\nLoose Isomorphs:");
            PrintNonIsomorphs();
        }

        static string PromptUser(string question)
        {
            string input = "";

            do
            {
                Console.WriteLine(question);
                input = Console.ReadLine();

                if (input == null || input == "")
                {
                    Console.WriteLine("\nWoops, that's not what I was looking for.\nPlease enter valid input pertaining to the quesiton.\n");
                }
            } while (input == null || input == "");
            
            return input;
        }

        static List<string> ParseFile(string path)
        {
            IEnumerable<string> words = File.ReadLines(path);

            return words.ToList();
        }

        static void SaveFile()
        {
            string path = filePath;
            int startSplit = -1;
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '\\')
                {
                    startSplit = i;
                    break;
                }
            }

            path = filePath.Substring(0, startSplit);
            Console.WriteLine(path);

            StreamWriter writer = File.CreateText(path);

            //TODO: save file

            writer.Close();
        }

        static void FindIsomorphs(List<string> words)
        {
            foreach (string word in words)
            {
                //Console.WriteLine("'" + word + "'");
                string iso = CalculateExactIsomorph(word);
                CreateNewKey(exactIsomorphIds, iso, word);

                string looseIso = CalculateLooseIsomorph(word);
                CreateNewKey(looseIsomorphIds, looseIso, word);
            }
        }

        static void FindNonIsomorphs()
        {
            RemoveNonIsomorphs(exactIsomorphIds);
            RemoveNonIsomorphs(looseIsomorphIds);
        }

        static void RemoveNonIsomorphs(Dictionary<string, List<string>> list)
        {
            var keys = list.Keys;
            for (int i = keys.Count - 1; i >= 0; i--)
            {
                var currentList = list[keys.ElementAt(i)];
                if (currentList.Count <= 1)
                {
                    if (!nonIsomorphs.Contains(currentList[0]))
                    {
                        nonIsomorphs.Add(currentList[0]);
                    }

                    list.Remove(keys.ElementAt(i));
                }
            }
        }

        static void PrintNonIsomorphs()
        {
            Console.WriteLine("\nNon-Isomorphs:");

            foreach (string s in nonIsomorphs)
            {
                Console.Write(s + ", ");
            }

            Console.WriteLine("\n");
        }

        static void CreateNewKey(Dictionary<string, List<string>> list, string key, string value)
        {
            if (!list.ContainsKey(key))
            {
                list[key] = new List<string>();
            }
            list[key].Add(value);
        }

        static void PrintIsomorphs(Dictionary<string, List<string>> list, string title)
        {
            Console.WriteLine(title);

            Dictionary<string, List<string>>.KeyCollection keys = list.Keys;
            foreach (string key in keys)
            {
                Console.Write(key + " -->");
                foreach (string word in list[key])
                {
                    Console.Write(" " + word + ",");
                }
                Console.WriteLine();
            }
        }

        static string CalculateExactIsomorph(string word)
        {
            string isomorph = "";

            int id = 0;
            Dictionary<char, int> letterValues = new Dictionary<char, int>();
            foreach (char c in word)
            {
                if (letterValues.ContainsKey(c))
                {
                    isomorph += letterValues[c];
                }
                else
                {
                    isomorph += id;
                    letterValues.Add(c, id);
                    ++id;
                }
            }

            return isomorph;
        }

        static string CalculateLooseIsomorph(string word)
        {
            string isomorph = "";

            Dictionary<char, int> letterCounts = new Dictionary<char, int>();
            foreach (char c in word)
            {
                if (!letterCounts.ContainsKey(c))
                {
                    letterCounts.Add(c, 1);
                }
                else
                {
                    letterCounts[c]++;
                }
            }

            SortListIntoString(letterCounts, ref isomorph);

            return isomorph;
        }

        static void SortListIntoString(Dictionary<char, int> list, ref string output)
        {
            var newList = list.OrderBy(i => i.Value);
            foreach (var item in newList)
            {
                output += item.Value;
            }
        }
    }
}
