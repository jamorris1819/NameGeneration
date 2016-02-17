using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace NameGeneration
{
    public static class NameGenerator
    {
        // A complete list of all names (with common names repeated).
        private static string[] names;
        // A list of all names without repeats.
        private static string[] analysisNames;
        private static string alphabet = "abcdefghijklmnopqrstuvwxyz";
        private static string forbiddenAlphabet = "q";
        // Each character has a list of possible characters attached.
        private static Dictionary<char, List<char>> charFinder;
        // This is the 'most confident' the program is about a name being good.
        private static double upperConfidence;
        private static string bestName;

        /// <summary>
        /// Initialises all the properties and behaviours of the class.
        /// </summary>
        public static void Initialise()
        {
            // By setting the upper confidence to 1, we can ignore it until it's needed.
            upperConfidence = 1;
            LoadNames();
            InitialiseAlphabet();
            ProcessNames();
            // We calculate the perfect name here, and hence the upper confidence
            Perfect();
        }

        /// <summary>
        /// Load all the names to an array.
        /// </summary>
        private static void LoadNames()
        {
            // Store each line in the names file.
            string[] tempLines;
            List<string> namesList = new List<string>();
            List<string> analysisNamesList = new List<string>();
            // Each line is structured so: NAME,GENDER,NUMBER
            // IE, JACOB,M,36748
            tempLines = File.ReadAllLines(@"yob1996.txt");
            foreach (string dataPiece in tempLines)
            {
                // We want specific parts of our data
                string[] parts = dataPiece.Split(',');
                string name = parts[0];
                int count = int.Parse(parts[2]);
                // If a certain name appears more than 1 time, add it again and again to 'dilute' the pool and make it more common.
                for (int i = 0; i < count; i++)
                    namesList.Add(name.ToLower());
                analysisNamesList.Add(name.ToLower());
            }
            names = namesList.ToArray();
            analysisNames = analysisNamesList.ToArray();
        }

        /// <summary>
        /// Creates an analysis of all the names in the database
        /// </summary>
        /// <returns>Data from the analysis</returns>
        public static string Analysis()
        {
            string data = "Data:\n";
            data += "Number of names: " + analysisNames.Length + "\n";
            data += "Number of people: " + names.Length + "\n";
            data += "Statistically best name: " + bestName + "\n";
            data += "Statistically best confidence: " + upperConfidence + "\n";
            // Calculate the average length of a name.
            float avgLength = 0;
            foreach (string name in analysisNames)
                avgLength += name.Length;
            avgLength /= analysisNames.Length;
            data += "Average name length: " + avgLength + " characters\n";
            // Calculate average consonant to vowel ratio.
            float avgConsonantVowelRatio = 0;
            // We must exclude certain names which contain no vowels, so as to preserve the integrity of our data.
            int excluded = 0;
            foreach (string name in analysisNames)
            {
                int vowels = 0;
                int consonants = 0;
                foreach (char character in name)
                {
                    if ("aeiou".Contains(character))
                        vowels++;
                    else
                        consonants++;
                }
                if (vowels > 0)
                    avgConsonantVowelRatio += consonants / vowels;
                else
                    excluded++;
            }
            avgConsonantVowelRatio /= (analysisNames.Length - excluded);
            data += "Average consonant to vowel ratio: " + avgConsonantVowelRatio + "\n";
            return data;
        }

        /// <summary>
        /// Analyses and creates a confidence score for the name
        /// </summary>
        /// <param name="name">The name to analyse</param>
        /// <returns>A score between 0 and 1</returns>
        public static double Analyse(string name)
        {
            name = name.ToLower();
            double confidence = 0;
            for (int i = 0; i < name.Length - 1; i++)
            {
                char currentChar = name[i];
                char nextChar = name[i + 1];
                // We're going to cycle through every possible following character, and count how many times our actual one appeared.
                int appearanceCounter = 0;
                foreach (char possibleCharacter in charFinder[currentChar])
                {
                    if (possibleCharacter == nextChar)
                        appearanceCounter++;
                }
                if (appearanceCounter == 0)
                    return 0;
                confidence += (double)appearanceCounter / (double)charFinder[currentChar].Count;
            }
            // We can't have a confidence limit on the last character, and we divide by the upperConfidence to create more widely rated results.
            return (confidence/(name.Length-1)) / upperConfidence;
        }

        /// <summary>
        /// Populate the character finder array.
        /// </summary>
        private static void InitialiseAlphabet()
        {
            charFinder = new Dictionary<char, List<char>>();
            foreach (char character in alphabet.ToArray<char>())
            {
                charFinder.Add(character, new List<char>());
            }
        }

        /// <summary>
        /// Processes and populates the name data fields.
        /// </summary>
        private static void ProcessNames()
        {
            foreach (string name in names)
            {
                // We want to go through every character in the string which has a successor - ie all but the last.
                // Then we add the following character to the list attached to the current character.
                for (int i = 0; i < name.Length - 1; i++)
                    charFinder[name[i]].Add(name[i + 1]);
            }
        }

        /// <summary>
        /// Calculates the next character to use for a name.
        /// </summary>
        /// <param name="lastChar">The last character used</param>
        /// <returns>The next character to use</returns>
        private static char FetchChar(char lastChar)
        {
            // There will be a degree of randomness in this, however it's biased towards commmon 'trends'.
            Random random = new Random();
            TimeOut();
            // Create a reference to make code neater.
            List<char> cFind = charFinder[lastChar];
            // Initialise nextChar - the value doesn't matter as it will be deleted.
            char nextChar = ' ';
            // We want to forbid certain letters.
            bool isValid = false;
            while (isValid == false)
            {
                nextChar = cFind[random.Next(cFind.Count)];
                isValid = !forbiddenAlphabet.Contains(nextChar);
            }
            return nextChar;
        }

        /// <summary>
        /// Provides a random character from our alphabet
        /// </summary>
        /// <returns>A random character</returns>
        private static char RandomChar()
        {
            Random random = new Random();
            TimeOut();
            // We want to remove forbidden characters.
            bool valid = false;
            char character = ' ';
            while (valid == false)
            {
                character = alphabet[random.Next(alphabet.Length)];
                valid = !forbiddenAlphabet.Contains(character);
            }
            return character;
        }

        /// <summary>
        /// Times out so the Random class can stay Random
        /// </summary>
        private static void TimeOut()
        {
            // The Random class uses the time to choose a random character. We must temporarily pause so the Random class can take a new string from the time.
            Thread.Sleep(25);
        }

        /// <summary>
        /// Generates a name
        /// </summary>
        /// <param name="length">The desired length of the name</param>
        /// <returns>A name</returns>
        public static string GenerateName(int length)
        {
            // Initialise our name with a random character - there's no logic needed here, we just need a base to start from.
            char currentChar = RandomChar();
            string name = currentChar.ToString().ToUpper();
            
            for (int i = 0; i < length - 1; i++) {
                // We want to strategically, but randomly choose the next character.
                // Here we prevent a name begining with two consonants.
                bool isValid = false;
                if (i == 0)
                {
                    while (isValid == false)
                    {
                        currentChar = FetchChar(currentChar);
                        if(!(IsConsonant(name.ToLower()[0]) && IsConsonant(currentChar)))
                            isValid = true;
                    }
                }
                else if (i > 1)
                {
                    // Here we're going to prevent 3 characters occuring in a row.
                    char lastChar = name[i];
                    char lastCharTwice = name[i - 1];
                    isValid = false;
                    while (isValid == false)
                    {
                        currentChar = FetchChar(currentChar);
                        if (lastChar == lastCharTwice)
                        {
                            if (lastChar != currentChar)
                                isValid = true;
                        }
                        else
                            isValid = true;

                    }
                }
                else
                    currentChar = FetchChar(currentChar);
                name += currentChar.ToString();
            }
            return name;
        }

        /// <summary>
        /// Generates the 'perfect' name that our system is most confident with.
        /// </summary>
        /// <returns></returns>
        public static string Perfect()
        {
            double confidenceTop = 0;
            string nameTop = "";
            foreach (char letter in alphabet)
            {
                if (forbiddenAlphabet.Contains(letter))
                    continue;
                string name = "";
                char lastChar;
                char currentChar = letter;
                name += currentChar.ToString();
                for (int i = 0; i < 6; i++)
                {
                    lastChar = currentChar;
                    currentChar = charFinder[lastChar].MostCommon<char>();
                    name += currentChar.ToString();
                }
                if (Analyse(name) > confidenceTop)
                {
                    confidenceTop = Analyse(name);
                    nameTop = name;
                }
            }
            upperConfidence = confidenceTop;
            bestName = nameTop;
            return nameTop;
        }

        private static bool IsConsonant(char c)
        {
            return "bcdfghjklmnpqrstvwxyz".Contains(c);
        }

        public static T MostCommon<T>(this IEnumerable<T> list)
        {
            return (from i in list
                           group i by i into grp
                           orderby grp.Count() descending
                           select grp.Key).First();
        }
    }
}
