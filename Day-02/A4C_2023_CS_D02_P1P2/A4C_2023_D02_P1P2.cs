// Advent for code - 2023 - Day 2 - R. Bruce Pitt, 2023/12/02

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.ComponentModel;

namespace A4C_2023_D02_P1P2
{
    internal class A4C_2023_D02_P1P2
    {
        public class SOLUTION
        {
            String _filePath = "";
            List<String> gameLines = new List<string>();
            // GameNo -> RunNo -> Color -> Count
            Dictionary<int, Dictionary<int, Dictionary<String, int>>> games = new Dictionary<int, Dictionary<int, Dictionary<String, int>>>();

            int partOneAnswer = 0;
            int partTwoAnswer = 0;

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputForGamesPlayed();
                ProcessGames();
                DeterminePartOneAnswer();
                DeterminePartTwoAnswer();
            }

            private void ReadInputForGamesPlayed()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        gameLines.Add(line);
                    }
                    lineCounter++;
                }

            }

            private void ProcessGames()
            {
                for (int i = 0; i < gameLines.Count; i++)
                {
                    // Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
                    // Game Number, then game turns ";"

                    String [] gameNumberAndRuns = gameLines[i].Split(':');
                    String gameNo = gameNumberAndRuns[0].Substring("Game ".Length);
                    int gameNumber = Convert.ToInt32(gameNo);

                    // Now Do the individual game runs
                    String[] gameRuns = gameNumberAndRuns[1].Trim().Split(';');
                    Dictionary<int, Dictionary<String, int>> runTracker = new Dictionary<int, Dictionary<string, int>>();
                    for (int runLoop = 0; runLoop < gameRuns.Length; runLoop++)
                    {
                        // Now we do each of the cube colors in this run
                        String[] cubeCounts = gameRuns[runLoop].Trim().Split(',');
                        Dictionary<String, int> thisColorCount = new Dictionary<String, int>();
                        for (int cubeLoop = 0; cubeLoop < cubeCounts.Length; cubeLoop++)
                        {
                            String[] countColor = cubeCounts[cubeLoop].Trim().Split(' ');
                            thisColorCount.Add(countColor[1].Trim().ToLower(), Convert.ToInt32(countColor[0]));
                        }
                        runTracker.Add(runLoop, thisColorCount);
                    }
                    games.Add(gameNumber, runTracker);
                }
            }
            private void DeterminePartOneAnswer()
            {
                int sumOfValidGames = 0;

                // Maximums
                int maxRed = 12;
                int maxGreen = 13;
                int maxBlue = 14;

                // Run through the games and see if a run's cubes exceeds our maximums
                // if no, ignore, if not, add Game No into sum.

                for (int gameLoop = 0; gameLoop < games.Count; gameLoop++)
                {
                    bool validOrInvalid = true; // true is valid
                    int gameNo = games.ElementAt(gameLoop).Key;
                    Dictionary<int, Dictionary<String, int>> runTrack = games.ElementAt(gameLoop).Value;
                    for (int runLoop = 0; runLoop < runTrack.Count; runLoop++)
                    {
                        Dictionary<String, int> cubeValues = runTrack[runLoop];
                        for (int cubeLoop = 0; cubeLoop < cubeValues.Count; cubeLoop++)
                        {
                            if (cubeValues.ElementAt(cubeLoop).Key == "green")
                            {
                                if (cubeValues["green"] > maxGreen)
                                {
                                    validOrInvalid = false;
                                    break;
                                }
                            }
                            if (cubeValues.ElementAt(cubeLoop).Key == "red")
                            {
                                if (cubeValues["red"] > maxRed)
                                {
                                    validOrInvalid = false;
                                    break;
                                }
                            }
                            if (cubeValues.ElementAt(cubeLoop).Key == "blue")
                            {
                                if (cubeValues["blue"] > maxBlue)
                                {
                                    validOrInvalid = false;
                                    break;
                                }
                            }
                            if (!validOrInvalid)
                                break;
                        }
                        if (!validOrInvalid)
                            break;
                    }
                    if (validOrInvalid)
                    {
                        sumOfValidGames += gameNo;
                    }
                }

                partOneAnswer = sumOfValidGames;
            }


            private void DeterminePartTwoAnswer()
            {
                int sumOfPowersFromGames = 0;

                // Run through the games and see if a run's cubes exceeds our maximums
                // if no, ignore, if not, add Game No into sum.

                for (int gameLoop = 0; gameLoop < games.Count; gameLoop++)
                {
                    // For determining the power
                    int neededRed = 0;
                    int neededGreen = 0;
                    int neededBlue = 0;

                    int gameNo = games.ElementAt(gameLoop).Key;
                    Dictionary<int, Dictionary<String, int>> runTrack = games.ElementAt(gameLoop).Value;
                    for (int runLoop = 0; runLoop < runTrack.Count; runLoop++)
                    {
                        Dictionary<String, int> cubeValues = runTrack[runLoop];
                        for (int cubeLoop = 0; cubeLoop < cubeValues.Count; cubeLoop++)
                        {
                            if (cubeValues.ElementAt(cubeLoop).Key == "green")
                            {
                                if (cubeValues["green"] > neededGreen)
                                    neededGreen = cubeValues["green"];
                            }
                            if (cubeValues.ElementAt(cubeLoop).Key == "red")
                            {
                                if (cubeValues["red"] > neededRed)
                                    neededRed = cubeValues["red"];
                            }
                            if (cubeValues.ElementAt(cubeLoop).Key == "blue")
                            {
                                if (cubeValues["blue"] > neededBlue)
                                    neededBlue = cubeValues["blue"];
                            }
                        }
                    }

                    int power = neededGreen * neededRed * neededBlue;
                    sumOfPowersFromGames += power;
                }

                partTwoAnswer = sumOfPowersFromGames;
            }

            public int GetPartOneAnswer()
            {
                return partOneAnswer;
            }

            public int GetPartTwoAnswer()
            {
                return partTwoAnswer;
            }

        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2023_D02_P1P2 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d1 = new SOLUTION(inputPath);
            //d1.DumpAllLines_PartOne();
            int get_part1 = d1.GetPartOneAnswer();
            Console.WriteLine("The solution to Part1 is : " + get_part1.ToString());
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();

            //d1.DumpAllLines_PartTwo();
            int get_part2 = d1.GetPartTwoAnswer();
            Console.WriteLine("The solution to Part2 is : " + get_part2.ToString());

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
