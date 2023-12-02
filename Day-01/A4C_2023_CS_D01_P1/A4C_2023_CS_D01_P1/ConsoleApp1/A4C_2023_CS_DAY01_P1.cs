// Advent for code - 2023 - Day 1 - R. Bruce Pitt, 2023/12/01

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.ComponentModel;

namespace A4C_2023_CS_DAY01_P1
{
    internal class A4C_2023_D01_P1
    {
        public class TEXT_CHECKER
        {
            private Dictionary<String, int> textNumbers = new Dictionary<String, int>();

            public TEXT_CHECKER()
            {
                textNumbers.Add("one", 1);
                textNumbers.Add("two", 2);
                textNumbers.Add("three", 3);
                textNumbers.Add("four", 4);
                textNumbers.Add("five", 5);
                textNumbers.Add("six", 6);
                textNumbers.Add("seven", 7);
                textNumbers.Add("eight", 8);
                textNumbers.Add("nine", 9);
                textNumbers.Add("zero", 0);
            }

            public int check(String currentLine, out String remainingLine)
            {
                int value = -1;
                remainingLine = currentLine;

                for (int i = 0; i < textNumbers.Count; i++)
                {
                    if (currentLine.IndexOf(textNumbers.ElementAt(i).Key) == 0)
                    {
                        value = textNumbers.ElementAt(i).Value;
                        remainingLine = currentLine.Substring(textNumbers.ElementAt(i).Key.Length);
                        break;
                    }
                }

                return value;
            }
        }


        public class SOLUTION
        {
            String _filePath = "";
            List<String> calibrationLines = new List<string>();
            List<int> calibrationValues_PartOne = new List<int>();
            List<int> calibrationValues_PartTwo = new List<int>();

            int partOneAnswer = 0;
            UInt64 partTwoAnswer = 0;

            public SOLUTION(String filePath)
            {
                _filePath = filePath;
                ReadInputForCalibrationValues();
                DeterminePartOneAnswer();
                DeterminePartTwoAnswer();
            }

            private void ReadInputForCalibrationValues()
            {
                StreamReader inHandle = new StreamReader(_filePath);
                String line = "";
                int lineCounter = 0;

                while ((line = inHandle.ReadLine()) != null)
                {
                    if (line != "")
                    {
                        calibrationLines.Add(line);
                    }
                    lineCounter++;
                }

            }

            private int processLine_NumbersOnly(int lineNo)
            {
                int num1 = -1;
                int num2 = -1;

                for (int i = 0; i < calibrationLines[lineNo].Length; i++)
                {
                    if ((calibrationLines[lineNo].ElementAt(i) >= '0') && 
                        (calibrationLines[lineNo].ElementAt(i) <= '9'))
                    {
                        if (num1 == -1)
                            num1 = Convert.ToInt32(calibrationLines[lineNo].ElementAt(i)-'0');
                        if (num2 == -1)
                            num2 = num1;
                        else
                            num2 = Convert.ToInt32(calibrationLines[lineNo].ElementAt(i)-'0');
                    }
                }

                return ((num1 * 10) + num2);
            }

            private int processLine_TextAndNumber(int lineNo)
            {
                int num1 = -1;
                int num2 = -1;
                TEXT_CHECKER tc = new TEXT_CHECKER();

                string calibrationLine = calibrationLines[lineNo];

                while (calibrationLine.Length > 0)
                {
                    int valueFound = -1;

                    if ((calibrationLine.ElementAt(0) >= '0') &&
                        (calibrationLine.ElementAt(0) <= '9'))
                    {
                        valueFound = Convert.ToInt32(calibrationLine.ElementAt(0) - '0');
                        calibrationLine = calibrationLine.Substring(1);
                    }
                    else
                    {
                        string remainingLine = calibrationLine;
                        // Can't move forward by the remaining line because of
                        // weird overlaps like "twone".
                        valueFound = tc.check(calibrationLine, out remainingLine);
                        if (valueFound != -1)
                        {
                            // calibrationLine = remainingLine;
                            calibrationLine = calibrationLine.Substring(1);
                        }
                        else
                        {
                            calibrationLine = calibrationLine.Substring(1);
                        }
                    }

                    if (valueFound != -1)
                    {
                        if (num1 == -1)
                            num1 = valueFound;
                        if (num2 == -1)
                            num2 = valueFound;
                        else
                            num2 = valueFound;
                    }
                }

                return ((num1 * 10) + num2);
            }

            private void DeterminePartOneAnswer()
            {
                int sumOfLines = 0;
                for (int i = 0; i < calibrationLines.Count; i++)
                {
                    int thisValue = processLine_NumbersOnly(i);
                    calibrationValues_PartOne.Add(thisValue);
                    sumOfLines += thisValue;
                }

                partOneAnswer = sumOfLines;
            }


            private void DeterminePartTwoAnswer()
            {
                UInt64 sumOfLines = 0;
                for (int i = 0; i < calibrationLines.Count; i++)
                {
                    int thisValue = processLine_TextAndNumber(i);
                    calibrationValues_PartTwo.Add(thisValue);
                    sumOfLines += Convert.ToUInt64(thisValue);
                }

                partTwoAnswer = sumOfLines;
            }

            public int GetPartOneAnswer()
            {
                return partOneAnswer;
            }

            public UInt64 GetPartTwoAnswer()
            {
                return partTwoAnswer;
            }

            public void DumpAllLines_PartOne()
            {
                for (int i = 0; i < calibrationValues_PartOne.Count; i++)
                {
                    Console.WriteLine((i + 1).ToString() + ": " + calibrationValues_PartOne.ElementAt(i).ToString() + " " + calibrationLines.ElementAt(i));
                }
            }

            public void DumpAllLines_PartTwo()
            {
                for (int i = 0; i < calibrationValues_PartTwo.Count; i++)
                {
                    Console.WriteLine((i + 1).ToString() + ": " + calibrationValues_PartTwo.ElementAt(i).ToString() + " " + calibrationLines.ElementAt(i));
                }
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Syntax: A4D_2023_D01_P1 <input_data_file_path>");
                return;
            }

            String inputPath = args[0];
            if (!File.Exists(inputPath))
            {
                Console.WriteLine("File " + inputPath + " does not exist");
                return;
            }

            SOLUTION d1 = new SOLUTION(inputPath);
            d1.DumpAllLines_PartOne();
            int get_part1 = d1.GetPartOneAnswer();
            Console.WriteLine("The solution to Part1 is : " + get_part1.ToString());
            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();

            d1.DumpAllLines_PartTwo();
            UInt64 get_part2 = d1.GetPartTwoAnswer();
            Console.WriteLine("The solution to Part2 is : " + get_part2.ToString());

            Console.WriteLine("\n\nPress any key...");
            Console.ReadKey();
        }
    }
}
