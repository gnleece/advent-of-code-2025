using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace advent_of_code_2025
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Problem6Part1();
        }

        #region Problem 1
        private static void Problem1Part1()
        {
            var fileName = "../../../input/input-1.txt";
            var lines = File.ReadLines(fileName);

            var dialPosition = 50;
            var dialMod = 100;
            var zeroCounter = 0;

            foreach (var line in lines)
            {

                var direction = line[0];
                var distance = int.Parse(line.Substring(1));

                if (direction == 'L')
                {
                    dialPosition -= distance;
                    while (dialPosition < 0)
                    {
                        dialPosition += dialMod;
                    }
                    dialPosition = dialPosition % dialMod;
                }
                else if (direction == 'R')
                {
                    dialPosition += distance;
                    dialPosition = dialPosition % dialMod;
                }

                if (dialPosition == 0)
                {
                    zeroCounter++;
                }
            }

            Console.WriteLine($"Zero count: {zeroCounter}");
        }

        private static void Problem1Part2()
        {
            var fileName = "../../../input/input-1.txt";
            var lines = File.ReadLines(fileName);

            var dialPosition = 50;
            var dialMod = 100;
            var zeroCounter = 0;

            foreach (var line in lines)
            {
                var prevDialPosition = dialPosition;

                var direction = line[0];
                var distance = int.Parse(line.Substring(1));

                var fullRotations = distance / dialMod;
                var remainderDistance = distance % dialMod;

                if (direction == 'L')
                {
                    dialPosition -= remainderDistance;
                    if (dialPosition <= 0 && prevDialPosition > 0)
                    {
                        zeroCounter++;
                    }
                    
                }
                else if (direction == 'R')
                {
                    dialPosition += remainderDistance;
                    if (dialPosition >= dialMod)
                    {
                        zeroCounter++;
                    }
                }

                while (dialPosition < 0)
                {
                    dialPosition += dialMod;
                }
                dialPosition = dialPosition % dialMod;

                zeroCounter += fullRotations;
            }

            Console.WriteLine($"Zero count: {zeroCounter}");
        }
        #endregion 

        #region Problem 2
        private static void Problem2()
        {
            var fileName = "../../../input/input-2.txt";
            var lines = File.ReadLines(fileName);

            long total = 0;

            foreach (var line in lines)
            {
                var ranges = line.Split(',');
                foreach (var range in ranges)
                {
                    var rangeValues = range.Split('-');
                    var rangeStart = long.Parse(rangeValues[0]);
                    var rangeEnd = long.Parse(rangeValues[1]);
                    Console.WriteLine($"Range start: {rangeStart}, Range end: {rangeEnd}");

                    for (long id = rangeStart; id <= rangeEnd; id++)
                    {
                        //if (!Problem2Part1_IsValidId(id))
                        if (!Problem2Part2_IsValidId(id))
                        {
                            total += id;
                        }
                    }
                }
            }

            Console.WriteLine($"Total: {total}");
        }

        private static bool Problem2Part1_IsValidId(long id)
        {
            string idString = id.ToString();
            var length = idString.Length;
            if (length % 2 != 0 )
            {
                //Console.WriteLine($"...Is valid {id}: odd length");
                return true;
            }
            var firstHalf = idString.Substring(0, length/2);
            var secondHalf = idString.Substring(length/2);
            if (firstHalf == secondHalf)
            {
                //Console.WriteLine($"...Is valid {id}: halves are EQUAL {firstHalf} {secondHalf}");
                return false;
            }
            //Console.WriteLine($"...Is valid {id}: halves are not equal {firstHalf} {secondHalf}");
            return true;
        }

        private static bool Problem2Part2_IsValidId(long id)
        {
            string idString = id.ToString();
            var idStringLength = idString.Length;
            var maxPatternLength = idStringLength / 2;
            
            for (int patternLength = 1;  patternLength <= maxPatternLength; patternLength++)
            {
                if (idStringLength % patternLength != 0)
                {
                    continue;
                }

                var pattern = idString.Substring(0, patternLength);
                var patternCount = idStringLength / patternLength;
                var allChunksMatchPattern = true;
                for (int i = 1; i < patternCount; i++)
                {
                    var currentChunk = idString.Substring(i * patternLength, patternLength);
                    if (currentChunk != pattern)
                    {
                        allChunksMatchPattern = false;
                        break;
                    }
                }

                if (allChunksMatchPattern)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region Problem 3
        private static void Problem3Part1()
        {
            var fileName = "../../../input/input-3.txt";
            var lines = File.ReadLines(fileName);

            var total = 0;

            foreach (var line in lines)
            {
                var maxFirstDigit = 0;
                var maxFirstDigitIndex = -1;
                for (int i = 0; i < line.Length-1; i++)
                {
                    var digit = int.Parse(line[i].ToString());
                    if (digit > maxFirstDigit)
                    {
                        maxFirstDigit = digit;
                        maxFirstDigitIndex = i;
                    }
                }

                var maxSecondDigit = 0;
                for (int i = maxFirstDigitIndex + 1; i < line.Length; i++)
                {
                    var digit = int.Parse(line[i].ToString());
                    if (digit > maxSecondDigit)
                    {
                        maxSecondDigit = digit;
                    }
                }

                var maxJoltage = (maxFirstDigit * 10) + maxSecondDigit;
                Console.WriteLine($"{line} First digit: {maxFirstDigit}, First digit index: {maxFirstDigitIndex}, Second digit: {maxSecondDigit}, Max joltage: {maxJoltage}");
                total += maxJoltage;
            }

            Console.WriteLine($"Total: {total}");
        }

        private static void Problem3Part2()
        {
            var fileName = "../../../input/input-3.txt";
            var lines = File.ReadLines(fileName);

            long total = 0;
            var numDigits = 12;

            foreach (var line in lines)
            {
                var maxJoltage = MaxJoltage(line, numDigits);

                Console.WriteLine($"Line: {line}, Max joltage: {maxJoltage}");
                total += maxJoltage;
            }

            Console.WriteLine($"Total: {total}");
        }

        private static long MaxJoltage(string bankString, int numDigits)
        {
            if (numDigits <= 1)
            {
                var maxDigit = 0;
                for (int i = 0; i < bankString.Length; i++)
                {
                    var digit = int.Parse(bankString[i].ToString());
                    if (digit > maxDigit)
                    {
                        maxDigit = digit;
                    }
                }
                return maxDigit;
            }
            else
            {
                var maxDigit = 0;
                var maxDigitIndex = -1;

                for (int i = 0; i < bankString.Length - numDigits + 1; i++)
                {
                    var digit = int.Parse(bankString[i].ToString());
                    if (digit > maxDigit)
                    {
                        maxDigit = digit;
                        maxDigitIndex = i;
                    }
                }
                var maxJoltageRemainder = MaxJoltage(bankString.Substring(maxDigitIndex + 1), numDigits - 1);
                var orderOfMagnitude = Math.Pow(10, numDigits - 1);
                var maxJoltage = (maxDigit * orderOfMagnitude) + maxJoltageRemainder;
                return (long)maxJoltage;
            }
        }
        #endregion

        #region Problem 4
        private static void Problem4Part1()
        {
            var rollGrid = BuildGridFromInput(out var lineCount, out var lineLength);

            int maxNeighbors = 3;
            long accessibleRollCount = 0;
            for (int i = 0; i < lineCount; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    if (IsRollAccessible(rollGrid, i, j, lineCount, lineLength, maxNeighbors))
                    {
                        accessibleRollCount++;
                    }
                }
            }

            Console.WriteLine($"Accessible roll count: {accessibleRollCount}");
        }

        private static void Problem4Part2()
        {
            var rollGrid = BuildGridFromInput(out var lineCount, out var lineLength);

            int maxNeighbors = 3;

            var currentRemovedCount = RemoveAccessibleRolls(rollGrid, lineCount, lineLength, maxNeighbors);
            var totalRemovedCount = currentRemovedCount;
            Console.WriteLine($"Current removed: {currentRemovedCount}, total removed: {totalRemovedCount}");

            while (currentRemovedCount > 0)
            {
                currentRemovedCount = RemoveAccessibleRolls(rollGrid, lineCount, lineLength, maxNeighbors);
                totalRemovedCount += currentRemovedCount;
                Console.WriteLine($"Current removed: {currentRemovedCount}, total removed: {totalRemovedCount}");
            }

            Console.WriteLine($"DONE - Total removed: {totalRemovedCount}");
        }

        struct RollCoord
        {
            public int x;
            public int y;
        }

        private static int RemoveAccessibleRolls(bool[,] rollGrid, int lineCount, int lineLength, int maxNeighbors)
        {
            List<RollCoord> rollsToRemove = new List<RollCoord>();

            for (int i = 0; i < lineCount; i++)
            {
                for (int j = 0; j < lineLength; j++)
                {
                    if (IsRollAccessible(rollGrid, i, j, lineCount, lineLength, maxNeighbors))
                    {
                        rollsToRemove.Add(new RollCoord { x = i, y = j });
                    }
                }
            }

            foreach (var coord in rollsToRemove)
            {
                rollGrid[coord.x, coord.y] = false;
            }

            return rollsToRemove.Count();
        }

        private static bool[,] BuildGridFromInput(out int rowCount, out int columnCount)
        {
            var fileName = "../../../input/input-4.txt";
            var lines = File.ReadLines(fileName);

            var lineLength = lines.First().Length;
            var lineCount = lines.Count();
            bool[,] rollGrid = new bool[lineCount, lineLength];
            Console.WriteLine($"Line length:{lineLength}, line count: {lineCount} ");

            int currentLine = 0;
            foreach (var line in lines)
            {
                for (int currentColumn = 0; currentColumn < lineLength; currentColumn++)
                {
                    var currentChar = line[currentColumn];
                    var occupied = currentChar == '@';
                    rollGrid[currentLine, currentColumn] = occupied;
                }
                currentLine++;
            }

            for (int i = 0; i < lineCount; i++)
            {
                string currentOutputLine = String.Empty;
                for (int j = 0; j < lineLength; j++)
                {
                    var occupied = rollGrid[i, j];
                    currentOutputLine += occupied ? '@' : '.';
                }
                Console.WriteLine(currentOutputLine);
            }

            rowCount = lineCount;
            columnCount = lineLength;
            return rollGrid;
        }

        private static bool IsRollAccessible(bool[,] rollGrid, int rowIndex, int columnIndex, int rowCount, int columnCount, int maxNeighbors)
        {
            //Console.WriteLine($"IsRollAccessible rowIndex: {rowIndex}, columnIndex: {columnIndex}");

            var isOccupied = rollGrid[rowIndex, columnIndex];
            if (!isOccupied)
            {
                return false;
            }

            var neighborCount = 0;
            for (int i = rowIndex - 1; i <= rowIndex + 1; i++)
            {
                for (int j = columnIndex - 1; j <= columnIndex + 1; j++)
                {
                    if (i == rowIndex && j == columnIndex)
                    {
                        //Console.WriteLine($"..... {i},{j}: continue");
                        continue;
                    }
                    
                    if (IsPositionOccupied(rollGrid, i, j, rowCount, columnCount))
                    {
                        //Console.WriteLine($"..... {i},{j}: neighbor++");
                        neighborCount++;
                    }
                    else
                    {
                        //Console.WriteLine($"..... {i},{j}: empty");
                    }
                }
            }

            var accessible = neighborCount <= maxNeighbors;
            //Console.WriteLine($"--> Accessible? {accessible}");
            return accessible;
        }

        private static bool IsPositionOccupied(bool[,] rollgrid,  int rowIndex, int columnIndex, int rowCount, int columnCount)
        {
            if (rowIndex < 0 || rowIndex >= rowCount || columnIndex < 0 || columnIndex >= columnCount)
            {
                return false;
            }
            return rollgrid[rowIndex, columnIndex];
        }

        #endregion

        #region Problem 5

        struct IdRange
        {
            public long start;
            public long end;

            public IdRange(long start, long end)
            {
                this.start = start;
                this.end = end;
            }
        }

        private static void Problem5Part1()
        {
            var fileName = "../../../input/input-5.txt";
            var lines = File.ReadLines(fileName);

            List<IdRange> validIdRanges = new List<IdRange>();
            List<long> ids = new List<long>();

            bool doneReadingRanges = false;
            foreach (var line in lines)
            {
                if (!doneReadingRanges)
                {
                    if (line == string.Empty)
                    {
                        doneReadingRanges = true;
                        continue;
                    }
                    var tokens = line.Split('-');
                    var rangeStart = long.Parse(tokens[0]);
                    var rangeEnd = long.Parse (tokens[1]);
                    validIdRanges.Add(new IdRange(rangeStart, rangeEnd));
                }
                else
                {
                    var id = long.Parse(line);
                    ids.Add(id);
                }
            }

            foreach (var range in  validIdRanges)
            {
                Console.WriteLine($"Range: {range.start}, {range.end}");
            }
            foreach (var id in ids)
            {
                Console.WriteLine($"Id: {id}");
            }

            validIdRanges = MergeOverlappingRanges(validIdRanges);
            foreach (var range in validIdRanges)
            {
                Console.WriteLine($"Merged range: {range.start}, {range.end}");
            }

            var validIdCount = 0;
            foreach (var id in ids)
            {
                foreach (var range in validIdRanges)
                {
                    if (id >= range.start && id <= range.end)
                    {
                        Console.WriteLine($"Valid id: {id}, range: {range.start}, {range.end}");
                        validIdCount++;
                        break;
                    }
                }
            }

            Console.WriteLine($"Total valid ids: {validIdCount}");
        }

        private static void Problem5Part2()
        {
            var fileName = "../../../input/input-5.txt";
            var lines = File.ReadLines(fileName);

            List<IdRange> validIdRanges = new List<IdRange>();

            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    break;
                }
                var tokens = line.Split('-');
                var rangeStart = long.Parse(tokens[0]);
                var rangeEnd = long.Parse(tokens[1]);
                validIdRanges.Add(new IdRange(rangeStart, rangeEnd));
            }

            foreach (var range in validIdRanges)
            {
                Console.WriteLine($"Range: {range.start}, {range.end}");
            }

            validIdRanges = MergeOverlappingRanges(validIdRanges);
            foreach (var range in validIdRanges)
            {
                Console.WriteLine($"Merged range: {range.start}, {range.end}");
            }

            long validIdCount = 0;
            foreach (var range in validIdRanges)
            {
                var rangeSize = range.end - range.start + 1;
                validIdCount += rangeSize;
            }

            Console.WriteLine($"Total valid ids: {validIdCount}");
        }

        private static List<IdRange> MergeOverlappingRanges(List<IdRange> ranges)
        {
            ranges.Sort((x, y) => x.start.CompareTo(y.start));

            List<IdRange> mergedRanges = new List<IdRange>();
            if (ranges.Count == 0)
            {
                return mergedRanges;
            }

            var currentRange = ranges[0];
            for (int i = 1; i < ranges.Count; i++)
            {
                var nextRange = ranges[i];
                if (nextRange.start <= currentRange.end)
                {
                    currentRange.end = Math.Max(currentRange.end, nextRange.end);
                }
                else
                {
                    mergedRanges.Add(currentRange);
                    currentRange = nextRange;
                }
            }
            mergedRanges.Add(currentRange);
            return mergedRanges;
        }

        #endregion

        #region Problem 6

        private enum MathOperation
        {
            None,
            Add,
            Multiply
        }

        private struct MathProblem
        {
            public List<long> numbers;
            public MathOperation operation;
        }

        private static void Problem6Part1()
        {
            var fileName = "../../../input/input-6-example.txt";
            var lines = File.ReadLines(fileName);

            if (lines.Count() == 0)
            {
                Console.WriteLine("Input is empty");
                return;
            }

            // Assume all lines have the same length - TODO add error handling if that's not the case
            var firstLine = lines.First();
            var lineLength = firstLine.Length;

            List<MathProblem> mathProblems = new List<MathProblem>(lineLength);
             

            foreach (var line in lines)
            {
                var tokens = line.Split(' ');
                for (int i = 0;  i < tokens.Length; i++)
                {
                    var mathProblem = mathProblems[i];

                }
            }
        }

        #endregion
    }
}
