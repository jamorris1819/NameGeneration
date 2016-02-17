using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NameGeneration;

namespace ExampleNameGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initialising the Name Generator. This may take a little while.");
            DateTime d = DateTime.Now;
            NameGenerator.Initialise();
            TimeSpan t = DateTime.Now - d;
            Console.WriteLine("Loaded in " + ((t.Seconds * 1000) + t.Milliseconds) + "ms");
            Console.WriteLine();
            Console.WriteLine("Name Generator Initialised");
            Console.WriteLine();
            Console.WriteLine("Welcome to my Name Generator");
            Console.WriteLine();
            while (true)
            {
                Console.WriteLine("Choose from the following commands:");
                Console.WriteLine("generate: Generates names");
                Console.WriteLine("analysis: Provides an analysis on the names database");
                Console.WriteLine("analyse [name]: returns a confidence rating on a name");
                string[] input = Console.ReadLine().ToLower().Trim().Split(' ');
                switch (input[0])
                { 
                    case "generate":
                        int countLetter = -1;
                        while (countLetter < 1)
                        {
                            try
                            {
                                Console.WriteLine("How many letters should your name have?");
                                countLetter = int.Parse(Console.ReadLine());
                            }
                            catch
                            {

                            }
                        }
                        int countNames = -1;
                        while (countNames < 1)
                        {
                            try
                            {
                                Console.WriteLine("How many rows of names would you like? (5 in a row)");
                                countNames = int.Parse(Console.ReadLine());
                            }
                            catch
                            {

                            }
                        }
                        for (int i = 0; i < countNames; i++)
                        {
                            for (int j = 0; j < 5; j++)
                            {

                                System.Threading.Thread.Sleep(100);
                                string name = NameGenerator.GenerateName(countLetter);
                                double score = Math.Round(NameGenerator.Analyse(name) * (double)100);
                                Console.Write(name + " (" +  score + ")  ");
                            }
                            Console.WriteLine();
                        }
                        break;
                    case "analysis":
                        Console.WriteLine(NameGenerator.Analysis());
                        break;
                    case "analyse":
                        string toAnalyse = input[1];
                        Console.WriteLine("Confidence level: " + Math.Round(NameGenerator.Analyse(toAnalyse) * 100));
                        Console.WriteLine();
                        break;
                }
            }
        }
    }
}
