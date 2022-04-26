using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace WordleSolver
{
    public class Program
    {
        public static WordleObject wordle = new WordleObject();
        static void Main(string[] args)
        {
            
            string[] words = File.ReadAllLines(System.Environment.CurrentDirectory + "/valid_solutions.csv");
            string[] extendedwords = File.ReadAllLines(System.Environment.CurrentDirectory + "/valid_guesses.csv");
            List<string> excludedLetters = new List<string>();

            string response = "";

            while (response != "6")
            {
                List<string> matchedWords = new List<string>();
                List<string> matchedExtendedWords = new List<string>();

                string letter = "";
                string letter_position = "";
                string position = "";

                switch (response)
                {
                    case "1":
                        Console.Write($"Enter Word String: ");
                        string wordStr = Console.ReadLine();

                        if (wordStr.Length == 10)
                        {
                            char[] wordArray = wordStr.ToCharArray();

                            if (Regex.IsMatch(wordArray[0].ToString(), "[a-z]"))
                            {
                                HandleMatchStringAction(wordArray[1], wordArray[0].ToString(), 1);
                                HandleMatchStringAction(wordArray[3], wordArray[2].ToString(), 2);
                                HandleMatchStringAction(wordArray[5], wordArray[4].ToString(), 3);
                                HandleMatchStringAction(wordArray[7], wordArray[6].ToString(), 4);
                                HandleMatchStringAction(wordArray[9], wordArray[8].ToString(), 5);
                            }
                            else
                            {
                                HandleMatchStringAction(wordArray[0], wordArray[1].ToString(), 1);
                                HandleMatchStringAction(wordArray[2], wordArray[3].ToString(), 2);
                                HandleMatchStringAction(wordArray[4], wordArray[5].ToString(), 3);
                                HandleMatchStringAction(wordArray[6], wordArray[7].ToString(), 4);
                                HandleMatchStringAction(wordArray[8], wordArray[9].ToString(), 5);
                            }
                        }

                        break;
                    case "2":
                        Console.Write($"Enter Letter: ");
                        letter = Console.ReadLine();

                        if (letter.Length == 1)
                            AddExcludedLetter(letter);

                        break;
                    case "3":
                        Console.Write($"Enter Letter/Excluded Position: ");
                        letter_position = Console.ReadLine();

                        if (letter_position.Length == 2)
                        {
                            letter = letter_position.Substring(0, 1);
                            position = letter_position.Substring(1, 1);

                            AddIncludedLetter(letter, Convert.ToInt32(position));
                        }
                        break;
                    case "4":
                        Console.Write($"Enter Letter/Position: ");
                        letter_position = Console.ReadLine();

                        if (letter_position.Length == 2)
                        {
                            letter = letter_position.Substring(0, 1);
                            position = letter_position.Substring(1, 1);

                            AddFoundLetter(letter, Convert.ToInt32(position));
                        }
                        break;

                    case "5":
                        string exclusionPattern = wordle.extractToExclusionRegex();
                        string inclusionPattern = wordle.extractToInclusionRegex();
                        Console.WriteLine($"Exclusion Pattern is : {exclusionPattern}");
                        Console.WriteLine($"Inclusion Pattern is : {inclusionPattern}");
                        Console.WriteLine($"Inclusion Pattern is : (?=[{wordle.includedLetters}]).*");

                        matchedWords.Clear();
                        matchedExtendedWords.Clear();

                        foreach (string word in words)
                        {
                            if (Regex.IsMatch(word, exclusionPattern) && Regex.IsMatch(word, inclusionPattern))
                            {
                                if (wordle.includedLetters != "")
                                {
                                    char[] includedLetterArr = wordle.includedLetters.ToCharArray();

                                    bool excludeWord = false;

                                    foreach (char includedLetter in includedLetterArr)
                                    {
                                        if (!Regex.IsMatch(word, includedLetter.ToString()))
                                        {
                                            excludeWord = true;
                                            break;
                                        }
                                    }

                                    if (!excludeWord)
                                    {
                                        matchedWords.Add(word);
                                    }
                                }
                                else
                                {
                                    matchedWords.Add(word);
                                }
                            }
                        }

                        foreach (string word in extendedwords)
                        {
                            if (!matchedWords.Contains(word) && Regex.IsMatch(word, exclusionPattern) && Regex.IsMatch(word, inclusionPattern))
                            {
                                if (wordle.includedLetters != "")
                                {
                                    if (Regex.IsMatch(word, "(?=[" + wordle.includedLetters + "]).*"))
                                    {
                                        matchedExtendedWords.Add(word);
                                    }
                                }
                                else
                                {
                                    matchedExtendedWords.Add(word);
                                }
                            }
                        }

                        Console.WriteLine("Standard Words");
                        int counter = 0;
                        foreach (string matchedword in matchedWords)
                        {
                            Console.WriteLine("   " + matchedword);
                            counter++;

                            if (counter > 40)
                                break;
                        }
                        Console.WriteLine($"Showing {(matchedWords.Count < 40 ? matchedWords.Count : 40)} of {matchedWords.Count} standard words.");
                        Console.WriteLine("");

                        // Show extended words if less than 10 standard words match
                        if (matchedWords.Count < 10)
                        {
                            Console.WriteLine("Extended Words");
                            foreach (string matchedword in matchedExtendedWords)
                            {
                                Console.WriteLine("   " + matchedword);
                                counter++;

                                if (counter > 20)
                                    break;
                            }
                            Console.WriteLine($"Showing {(matchedExtendedWords.Count < 20 ? matchedExtendedWords.Count : 20)} of {matchedExtendedWords.Count} extended words.");
                        }
                        break;
                }
                Console.WriteLine("");
                Console.WriteLine($"Enter Action: (1) Match String, (2) Excluded Letters, (3) Included Letters, (4) Matched Letters, (5) Show Words, (6) Exit");
                response = Console.ReadLine();
            }  
        }

        private static void AddExcludedLetter(string letter)
        {
            wordle.position1_excludedLetters += letter;
            wordle.position2_excludedLetters += letter;
            wordle.position3_excludedLetters += letter;
            wordle.position4_excludedLetters += letter;
            wordle.position5_excludedLetters += letter;
            wordle.position1_includedLetters = wordle.position1_includedLetters.Replace(letter, "");
            wordle.position2_includedLetters = wordle.position2_includedLetters.Replace(letter, "");
            wordle.position3_includedLetters = wordle.position3_includedLetters.Replace(letter, "");
            wordle.position4_includedLetters = wordle.position4_includedLetters.Replace(letter, "");
            wordle.position5_includedLetters = wordle.position5_includedLetters.Replace(letter, "");
        }

        private static void AddIncludedLetter(string letter, int position)
        {
            switch (position)
            {
                case 1:
                    wordle.position1_includedLetters = wordle.position1_includedLetters.Replace(letter, "");
                    break;

                case 2:
                    wordle.position2_includedLetters = wordle.position2_includedLetters.Replace(letter, "");
                    break;

                case 3:
                    wordle.position3_includedLetters = wordle.position3_includedLetters.Replace(letter, "");
                    break;

                case 4:
                    wordle.position4_includedLetters = wordle.position4_includedLetters.Replace(letter, "");
                    break;

                case 5:
                    wordle.position5_includedLetters = wordle.position5_includedLetters.Replace(letter, "");
                    break;
            }

            wordle.includedLetters += letter;
        }

        private static void AddFoundLetter(string letter, int position)
        {
            switch (position)
            {
                case 1:
                    wordle.position1_foundLetter = letter;
                    break;

                case 2:
                    wordle.position2_foundLetter = letter;
                    break;

                case 3:
                    wordle.position3_foundLetter = letter;
                    break;

                case 4:
                    wordle.position4_foundLetter = letter;
                    break;

                case 5:
                    wordle.position5_foundLetter = letter;
                    break;
            }
        }

        private static void HandleMatchStringAction(char action, string letter, int position)
        {
            if (letter.Length == 1)
            {
                switch (action)
                {
                    case '!':
                        AddExcludedLetter(letter);

                        break;

                    case '+':
                        AddIncludedLetter(letter, position);

                        break;

                    case '*':
                        AddFoundLetter(letter, position);

                        break;
                    default:
                        Console.WriteLine("Unknown Action: " + action);

                        break;
                }
            }
        }
    }
}

public class WordleObject
{
    public WordleObject()
    {
        position1_foundLetter = "";
        position1_excludedLetters = "";
        position1_includedLetters = "abcdefghijklmnopqrstuvwxyz";
        position2_foundLetter = "";
        position2_excludedLetters = "";
        position2_includedLetters = "abcdefghijklmnopqrstuvwxyz";
        position3_foundLetter = "";
        position3_excludedLetters = "";
        position3_includedLetters = "abcdefghijklmnopqrstuvwxyz";
        position4_foundLetter = "";
        position4_excludedLetters = "";
        position4_includedLetters = "abcdefghijklmnopqrstuvwxyz";
        position5_foundLetter = "";
        position5_excludedLetters = "";
        position5_includedLetters = "abcdefghijklmnopqrstuvwxyz";
        includedLetters = "";
    }

    public string position1_foundLetter { get; set; }
    public string position1_excludedLetters { get; set; }
    public string position1_includedLetters { get; set; }
    public string position2_foundLetter { get; set; }
    public string position2_excludedLetters { get; set; }
    public string position2_includedLetters { get; set; }
    public string position3_foundLetter { get; set; }
    public string position3_excludedLetters { get; set; }
    public string position3_includedLetters { get; set; }
    public string position4_foundLetter { get; set; }
    public string position4_excludedLetters { get; set; }
    public string position4_includedLetters { get; set; }
    public string position5_foundLetter { get; set; }
    public string position5_excludedLetters { get; set; }
    public string position5_includedLetters { get; set; }
    public string includedLetters { get; set; }

    public string extractToExclusionRegex()
    {
        string pattern = "";

        if (position1_foundLetter != "")
        {
            pattern += position1_foundLetter;
        }
        else if (position1_excludedLetters != "" || position1_includedLetters != "")
        {
            pattern += "[";

        /*    if (position1_includedLetters != "")
            {
                pattern += position1_includedLetters;
            }
        */
            if (position1_excludedLetters != "")
            {
                pattern += "^" +position1_excludedLetters;
            }

            pattern += "]";
        }

        if (position2_foundLetter != "")
        {
            pattern += position2_foundLetter;
        }
        else if (position2_excludedLetters != "" || position2_includedLetters != "")
        {
            pattern += "[";

        /*    if (position2_includedLetters != "")
            {
                pattern += position2_includedLetters;
            }
        */
            if (position2_excludedLetters != "")
            {
                pattern += "^" + position2_excludedLetters;
            }

            pattern += "]";
        }

        if (position3_foundLetter != "")
        {
            pattern += position3_foundLetter;
        }
        else if (position3_excludedLetters != "" || position3_includedLetters != "")
        {
            pattern += "[";

        /*    if (position3_includedLetters != "")
            {
                pattern += position3_includedLetters;
            }
        */
            if (position3_excludedLetters != "")
            {
                pattern += "^" + position3_excludedLetters;
            }

            pattern += "]";
        }

        if (position4_foundLetter != "")
        {
            pattern += position4_foundLetter;
        }
        else if (position4_excludedLetters != "" || position4_includedLetters != "")
        {
            pattern += "[";

        /*    if (position4_includedLetters != "")
            {
                pattern += position4_includedLetters;
            }
        */
            if (position4_excludedLetters != "")
            {
                pattern += "^" + position4_excludedLetters;
            }

            pattern += "]";
        }

        if (position5_foundLetter != "")
        {
            pattern += position5_foundLetter;
        }
        else if (position5_excludedLetters != "" || position5_includedLetters != "")
        {
            pattern += "[";

        /*    if (position5_includedLetters != "")
            {
                pattern += position5_includedLetters;
            }
        */
            if (position5_excludedLetters != "")
            {
                pattern += "^" + position5_excludedLetters;
            }

            pattern += "]";
        }

        return pattern;
    }

    public string extractToInclusionRegex()
    {
        string pattern = "";

        if (position1_foundLetter != "")
        {
            pattern += position1_foundLetter;
        }
        else if (position1_includedLetters != "")
        {
            pattern += "[" + position1_includedLetters + "]"; 
        }

        if (position2_foundLetter != "")
        {
            pattern += position2_foundLetter;
        }
        else if (position2_includedLetters != "")
        {
            pattern += "[" + position2_includedLetters + "]";
        }

        if (position3_foundLetter != "")
        {
            pattern += position3_foundLetter;
        }
        else if (position3_includedLetters != "")
        {
            pattern += "[" + position3_includedLetters + "]";
        }

        if (position4_foundLetter != "")
        {
            pattern += position4_foundLetter;
        }
        else if (position4_includedLetters != "")
        {
            pattern += "[" + position4_includedLetters + "]";
        }

        if (position5_foundLetter != "")
        {
            pattern += position5_foundLetter;
        }
        else if (position5_includedLetters != "")
        {
            pattern += "[" + position5_includedLetters + "]";
        }

        return pattern;
    }
}
 