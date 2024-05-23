using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace main
{
    internal class Program
    {
        
        static void MainMenu()
        {   
            bool exit = false;
            do
            {
                ToClearScreen();
                Console.WriteLine("MAIN MENU\n");
                Console.WriteLine("1.\tDictionary list.\n2.\tCreate dictionary.\n\n'Q' to quit.\n");
                Console.Write("Your input:\t");
                string choice = Console.ReadLine().ToUpper();
                switch (choice)
                {
                    case "1":
                        ToClearScreen();
                        DictionaryListMenu();
                        break;
                    case "2":
                        ToClearScreen();
                        ToCreateDictionary();
                        break;
                    case "Q":
                        ToClearScreen();
                        Console.WriteLine("Have a nice day!");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Incorrect data.");
                        break;
                }
                Console.WriteLine();
            } while (!exit);
        }

        static void DictionaryListMenu()
        {
            Console.WriteLine("DICTIONARY LIST MENU\n");
            int counter = 0;
            try
            {
                var lines = File.ReadLines("dict_list.txt").ToList();
                foreach (string line in lines)
                {
                    counter++;
                    Console.WriteLine($"{counter}.\t{line.ToUpper()}");
                }
                Console.WriteLine("\n'P' to previous.\n");

                bool exit = false;
                int dictionaryNumber = 0;
                do
                {
                    Console.Write("Your input:\t");
                    string input = Console.ReadLine();
                    if (input.ToUpper() == "P")
                    {
                        ToClearScreen();
                        exit = true;
                    }
                    else if (int.TryParse(input, out dictionaryNumber))
                    {
                        if (dictionaryNumber > 0 && dictionaryNumber <= lines.Count)
                        {
                            exit = true;
                            ToClearScreen();
                            string dictionaryName = lines[dictionaryNumber - 1];

                            SortedList<string, string> dictionary = ToCallList();
                            dictionary = ToLoadDictionary(dictionaryName);
                            DictionaryMenu(dictionaryName, dictionary);
                        }
                        else
                        {
                            Console.WriteLine("Incorrect data.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                    Console.WriteLine();
                } while (!exit);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            ToClearScreen();
        }


        static SortedList<string, string> ToLoadDictionary(string dictionaryName)
        {
            string dictionaryPath = ToCreateDictionaryPath(dictionaryName);
            string json = ToReadFromFile(dictionaryPath);

            if (json.StartsWith("Error") || json == "File not found.")
            {
                Console.WriteLine(json);
                return null;
            }

            return ToDeserialize(json);
        }



        static void DictionaryMenu(string dictionaryName, SortedList<string, string> dictionary)
        {
            bool exit = false;
            do
            {
                ToClearScreen();
                Console.WriteLine($"{dictionaryName.ToUpper()} DICTIONARY MENU\n");
                Console.WriteLine("1.\tFind word\n2.\tShow words\n3.\tEdit\n\n'P' to previous.\n");
                Console.Write("Your input:\t");
                string choice = Console.ReadLine().ToUpper();
                switch (choice)
                {
                    case "1":
                        ToClearScreen();
                        FindWord(dictionaryName, dictionary);
                        break;
                    case "2":
                        ToClearScreen();
                        ShowWords(dictionaryName, dictionary);
                        break;
                    case "3":
                        ToClearScreen();
                        EditMenu(dictionaryName, dictionary);
                        break;
                    case "P":
                        ToClearScreen();
                        exit = true;
                        string dictionaryPath = ToCreateDictionaryPath(dictionaryName);
                        string file = ToSerialize(dictionary);
                        WriteToFile(file, dictionaryPath);
                        break;
                    default:
                        Console.WriteLine("Incorrect data.");
                        break;
                }
                Console.WriteLine();
            } while (!exit);
        }

        static void EditMenu(string dictionaryName, SortedList<string, string> dictionary)
        {
            bool exit = false;
            do
            {
                ToClearScreen();
                Console.WriteLine($"{dictionaryName.ToUpper()} DICTIONARY EDIT MENU\n");
                Console.WriteLine("1.\tAdd new word\n2.\tEdit word\n3.\tRemove word\n4.\tAdd new translate\n5.\tRemove translate\n\n'P' to previous.\n");
                Console.Write("Your input:\t");
                string choice = Console.ReadLine().ToUpper();
                switch (choice)
                {
                    case "1":
                        ToClearScreen();
                        ToAddWord(dictionary);
                        break;
                    case "2":
                        ToClearScreen();
                        ToEditWord(dictionary);
                        break;
                    case "3":
                        ToClearScreen();
                        ToRemoveWord(dictionary);
                        break;
                    case "4":
                        ToClearScreen();
                        ToAddTranslate(dictionary);
                        break;
                    case "5":
                        ToClearScreen();
                        ToRemoveTranslate(dictionary);
                        break;
                    case "P":
                        ToClearScreen();
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Incorrect data.");
                        break;
                }
                Console.WriteLine();
            } while (!exit);
        }


        static string ToCreateDictionaryName(string fromLanguage, string intoLanguage)
        {
            return $"{fromLanguage.Replace(" ", "_")}-{intoLanguage.Replace(" ", "_")}";
        }

        static string ToCreateDictionaryPath(string dictionaryName)
        {
            return $"{dictionaryName.Replace(" ", "_")}.txt";
        }

        static void ToCreateDictionary()
        {
            Console.WriteLine($"CREATE DICTIONARY MENU\n");
            Console.WriteLine($"New dictionary name:");
            Console.Write($"enter from which language:\t\t");
            string fromLanguage = Console.ReadLine().ToLower();
            Console.Write($"enter into which language:\t\t");
            string intoLanguage = Console.ReadLine().ToLower();
            string dictionaryName = ToCreateDictionaryName(fromLanguage, intoLanguage);
            string dictionaryPath = ToCreateDictionaryPath(dictionaryName);
            if (!File.Exists(dictionaryPath))
            {
                File.Create(dictionaryPath);
                try
                {
                    using (StreamWriter streamWriter = new StreamWriter("dict_list.txt", true))
                    {
                        streamWriter.WriteLine(dictionaryName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error: {ex.Message}");
                }
                Console.WriteLine($"{dictionaryName} dictionary created.\n(don`t forget fill it)");
            }
            else 
            {
                Console.WriteLine("dictionary with same name exist already");
            }      
        }
        static SortedList<string, string> ToCallList()
        {
            return new SortedList<string, string>();
        }

        static string ToSerialize(SortedList<string, string> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }


        static SortedList<string, string> ToDeserialize(string json)
        {
            return JsonConvert.DeserializeObject<SortedList<string, string>>(json);
        }


        static void WriteToFile(string file, string dictionaryPath)
        {
            try
            {
                using (StreamWriter stream_writer = new StreamWriter(dictionaryPath, false))
                {
                    stream_writer.Write($"{file}");
                    Console.WriteLine($"* file \"{dictionaryPath}\" created/updated");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
            };
        }

        static string ToReadFromFile(string dictionaryPath)
        {
            try
            {
                using (StreamReader stream_reader = new StreamReader(dictionaryPath))
                {
                    return stream_reader.ReadToEnd();
                }
            }
            catch (FileNotFoundException)
            {
                return "File not found.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        static void FindWord(string dictionaryName, SortedList<string, string> dictionary)
        {
            Console.WriteLine($"{dictionaryName.ToUpper()} DICTIONARY (find word)\n");
            List<string> words = new List<string>();
            while (true)
            {
                Console.Write($"enter a word:\t\t\t");
                string findWord = Console.ReadLine().ToLower();
                if (dictionary.ContainsKey(findWord))
                {
                    string translate = "";
                    dictionary.TryGetValue(findWord, out translate);
                    Console.WriteLine($"\nresult:\t\t\t\t{findWord}\t->\t{translate}\n");
                    string result = $"{DateTime.Now} : {findWord} -> {translate}";
                    words.Add(result);
                }
                else
                {
                    Console.WriteLine($"\'{findWord}\' not exist.");
                }

                    Console.WriteLine("\nadd another word? (Y/N)");
                string exit = Console.ReadLine().ToLower();
                if (exit != "y") { break; }
            }
            string file = string.Join(", ", words);

            WriteToFile(file, "log_list.txt");
        }

        static void ShowWords(string dictionaryName, SortedList<string, string> dictionary)
        {
            void PrintHeader()
            {
                Console.WriteLine($"{dictionaryName.ToUpper()} DICTIONARY ({dictionary.Count()} words)\n");
            }
            PrintHeader();
            var words = dictionary.Keys.ToList();
            for (int i = 0; i < words.Count; i++)
            {
                Console.WriteLine(words[i]);
                if ((i + 1) % 10 == 0 && i != 0)
                {
                    Console.WriteLine("\nNext . . .");
                    Console.ReadKey();
                    Console.Clear();
                    PrintHeader();
                }
            }
            Console.WriteLine("\nPress any key to continue . . .");
            Console.ReadKey();
            Console.Clear();
        }


        static SortedList<string, string> ToAddWord(SortedList<string, string> dictionary)
        {
            Console.WriteLine($"EDIT MENU (add new word)\n");
            while (true)
            {
                Console.Write($"enter a word:\t\t\t");
                string newWord = Console.ReadLine().ToLower();
                if (dictionary.ContainsKey(newWord))
                {
                    Console.WriteLine($"\'{newWord}\' already exists.");   
                }
                else
                {
                    Console.Write($"enter translation:\t\t");
                    string newTranslate = Console.ReadLine().ToLower();
                    dictionary.Add(newWord, newTranslate);
                }

                string translate = "";
                dictionary.TryGetValue(newWord, out translate);
                Console.WriteLine($"\nresult:\t\t\t\t{newWord}\t->\t{translate}\n");

                Console.WriteLine("\nadd another word? (Y/N)");
            string exit = Console.ReadLine().ToLower();
            if (exit != "y") { break; }
        }
            return dictionary;
        }

        static SortedList<string, string> ToEditWord(SortedList<string, string> dictionary)
        {
            Console.WriteLine($"EDIT MENU (word editing)\n");
            while (true)
            { 
                Console.Write($"enter a word:\t\t\t");
                string editWord = Console.ReadLine().ToLower();
                if (dictionary.ContainsKey(editWord))
                {
                    Console.Write($"enter the word to correct:\t");
                    string correctWord = Console.ReadLine().ToLower();

                    string translate = "";
                    dictionary.TryGetValue(editWord, out translate);
                    dictionary.Remove(editWord);
                    dictionary.Add(correctWord, translate);

                    dictionary.TryGetValue(correctWord, out translate);
                    Console.WriteLine($"\nresult:\t\t\t\t{correctWord}\t->\t{translate}\n");
                }
                else
                {
                    Console.WriteLine($"\'{editWord}\' not exist.");
                }

                Console.WriteLine("\nediting another word? (Y/N)");
                string exit = Console.ReadLine().ToLower();
                if (exit != "y") { break; }
            }
            return dictionary;
        }

        static SortedList<string, string> ToRemoveWord(SortedList<string, string> dictionary)
        {
            Console.WriteLine($"EDIT MENU (remove word)\n");
            while (true)
            {
                Console.Write($"enter a word:\t\t\t");
                string removeWord = Console.ReadLine().ToLower();
                Console.WriteLine();
                if (dictionary.ContainsKey(removeWord)) 
                {
                    dictionary.Remove(removeWord);
                    Console.WriteLine($"\'{removeWord}\' removed.");
                }
                else 
                { 
                    Console.WriteLine($"\'{removeWord}\' not exist."); 
                }
                Console.WriteLine("\nremove another word? (Y/N)");
                string exit = Console.ReadLine().ToLower();
                if (exit != "y") { break; }
            }
            return dictionary;
        }

        static SortedList<string, string> ToAddTranslate(SortedList<string, string> dictionary)
        {
            Console.WriteLine($"EDIT MENU (add translation)\n");
            while (true)
            {
                Console.Write($"enter a word:\t\t\t");
                string editWord = Console.ReadLine().ToLower();
                if (dictionary.ContainsKey(editWord))
                {
                    string translate = "";
                    dictionary.TryGetValue(editWord, out translate);
                    Console.WriteLine($"\nresult:\t\t\t\t{editWord}\t->\t{translate}\n");
                    Console.Write($"enter a new translation:\t");
                    string addTranslate = Console.ReadLine().ToLower();

                    List<string> translateList = translate.Split(',').ToList();
                    for (int i = 0; i < translateList.Count; i++)
                    {
                        translateList[i] = translateList[i].Trim();
                    }

                    if (translateList.Count == 1 && translateList[0] == "no_translation")
                    {
                        translateList.Clear();
                    }

                    translateList.Add(addTranslate);

                    string newTranslation = string.Join(", ", translateList);

                    dictionary[editWord] = newTranslation;

                    dictionary.TryGetValue(editWord, out translate);
                    Console.WriteLine($"\nnew result:\t\t\t{editWord}\t->\t{translate}\n");
                }
                else
                {
                    Console.WriteLine($"\'{editWord}\' not exist.");
                }
                Console.WriteLine("\nediting another word? (Y/N)");
                string exit = Console.ReadLine().ToLower();
                if (exit != "y") { break; }
            }
            return dictionary;
        }

        static SortedList<string, string> ToRemoveTranslate(SortedList<string, string> dictionary)
        {
            Console.WriteLine($"EDIT MENU (remove translation)\n");
            while (true)
            {
                Console.Write($"enter a word:\t\t\t");
                string editWord = Console.ReadLine().ToLower();
                if (dictionary.ContainsKey(editWord))
                {
                    string translate = "";
                    dictionary.TryGetValue(editWord, out translate);
                    Console.WriteLine($"\nresult:\t\t\t\t{editWord}\t->\t{translate}\n");

                    Console.Write($"enter a word to remove:\t\t");
                    string removeTranslate = Console.ReadLine().ToLower();

                    List<string> translateList = translate.Split(',').ToList();
                    translateList = translateList.Select(t => t.Trim()).ToList();

                    if (translateList.Remove(removeTranslate))
                    {
                        if (translateList.Count == 0)
                        {
                            translateList.Add("no_translation");
                        }

                        string newTranslation = string.Join(", ", translateList);

                        dictionary[editWord] = newTranslation;

                        dictionary.TryGetValue(editWord, out translate);
                        Console.WriteLine($"\nnew result:\t\t\t{editWord}\t->\t{translate}\n");
                    }
                    else
                    {
                        Console.WriteLine($"\'{removeTranslate}\' not found in translations.");
                    }
                }
                else
                {
                    Console.WriteLine($"\'{editWord}\' not exist.");
                }
                Console.WriteLine("\nediting another word? (Y/N)");
                string exit = Console.ReadLine().ToLower();
                if (exit != "y") { break; }
            }
            return dictionary;
        }

        static void ToClearScreen()
        {
            Console.Clear();
        }

        static void Main(string[] args)
        {
            MainMenu();

        }
    }
}
