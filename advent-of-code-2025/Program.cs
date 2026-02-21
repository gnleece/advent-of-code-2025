using System.Diagnostics.CodeAnalysis;

namespace advent_of_code_2025;

internal class Program
{
    static void Main(string[] args)
    {
        Problem10Part2();
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

    record struct RollCoord(int x, int y);

    private static int RemoveAccessibleRolls(bool[,] rollGrid, int lineCount, int lineLength, int maxNeighbors)
    {
        List<RollCoord> rollsToRemove = new List<RollCoord>();

        for (int i = 0; i < lineCount; i++)
        {
            for (int j = 0; j < lineLength; j++)
            {
                if (IsRollAccessible(rollGrid, i, j, lineCount, lineLength, maxNeighbors))
                {
                    rollsToRemove.Add(new RollCoord(i, j));
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
            string currentOutputLine = string.Empty;
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

    record struct IdRange(long start, long end);

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

    private class MathProblem
    {
        public List<long> numbers;
        public MathOperation operation;

        public MathProblem()
        {
            numbers = new();
            operation = MathOperation.None;
        }

        public long GetValue()
        {
            var numberString = String.Join(",", numbers);
            Console.WriteLine($"Get value on: {numberString}");
            if (operation == MathOperation.Add)
            {
                return numbers.Sum();
            }
            else if (operation == MathOperation.Multiply)
            {
                return numbers.Aggregate(1L, (acc, val) => acc * val);
            }
            else
            {
                return 0;
            }
        }
    }

    private static void Problem6Part1()
    {
        var fileName = "../../../../input/input-6.txt";
        var lines = File.ReadLines(fileName);

        if (lines.Count() == 0)
        {
            Console.WriteLine("Input is empty");
            return;
        }

        Dictionary<int, MathProblem> mathProblems = new();
             
        foreach (var line in lines)
        {
            Console.WriteLine($"Line: {line}");
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0;  i < tokens.Length; i++)
            {
                
                if (!mathProblems.TryGetValue(i, out var mathProblem))
                {
                    mathProblem = new MathProblem();
                    mathProblems[i] = mathProblem;
                }

                var token = tokens[i];
                Console.WriteLine($"    {token}");

                if (string.Equals(token, "+"))
                {
                    Console.WriteLine($"    Add!");
                    mathProblem.operation = MathOperation.Add;
                }
                else if (string.Equals(token, "*"))
                {
                    Console.WriteLine($"    Multiply!");
                    mathProblem.operation = MathOperation.Multiply;
                }
                else
                {
                    try
                    {
                        var number = long.Parse(token);
                        mathProblem.numbers.Add(number);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        long total = 0;
        foreach (var mathProblem in mathProblems)
        {
            var value = mathProblem.Value.GetValue();
            Console.WriteLine($"Value returned: {value}");
            total += value;
        }

        Console.WriteLine($"Total: {total}");
    }

    private class CephalopodProblem
    {
        public MathOperation operation;
        public int startColumnIndex;
        public int endColumnIndex;

        public CephalopodProblem(MathOperation operation, int startColumnIndex, int endColumnIndex)
        {
            this.operation = operation;
            this.startColumnIndex = startColumnIndex;
            this.endColumnIndex = endColumnIndex;
        }
    }

    private class CephalopodWorksheet
    {
        public string[]? lines;

        public CephalopodWorksheet(string[]? lines)
        {
            this.lines = lines;
        }

        public char? GetCharacter(int x, int y)
        {
            if (lines == null)
            {
                return null;
            }

            if (y >= lines.Length)
            {
                return null;
            }
            var line = lines[y];

            if (x >= line.Length)
            {
                return null;
            }

            return line[x];
        }
    }

    private static void Problem6Part2()
    {
        var fileName = "../../../../input/input-6.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Input is empty");
            return;
        }

        List<CephalopodProblem> problemsList = new List<CephalopodProblem>();

        var operatorsLine = lines.Last();

        var startIndex = 0;
        var currentOperation = MathOperation.None;
        for (int i = 0; i < operatorsLine.Length; i++)
        {
            var token = operatorsLine[i];
            if (token == '+' || token == '*')
            {
                if (currentOperation != MathOperation.None)
                {
                    var newProblem = new CephalopodProblem(currentOperation, startIndex, i-2);
                    problemsList.Add(newProblem);
                }

                currentOperation = token == '+' ? MathOperation.Add : MathOperation.Multiply;
                startIndex = i;
            }
        }

        var maxLineLength = lines.Max(l => l.Length);
        var lastProblem = new CephalopodProblem(currentOperation, startIndex, maxLineLength - 1);
        problemsList.Add(lastProblem);

        foreach (var problem in problemsList)
        {
            Console.WriteLine($"Operator: {problem.operation}, Start: {problem.startColumnIndex}, End: {problem.endColumnIndex}");
        }

        var worksheet = new CephalopodWorksheet(lines);
        var numberLinesCount = lines.Count() - 1;

        long grandTotal = 0;
        foreach (var problem in problemsList)
        {
            long problemTotal = problem.operation == MathOperation.Add ? 0 : 1;
            for (int x = problem.startColumnIndex; x <= problem.endColumnIndex; x++)
            {
                string accumulatedNumberString = string.Empty;
                for (int y = 0; y < numberLinesCount; y++)
                {
                    char? token = worksheet.GetCharacter(x, y);
                    if (token != null)
                    {
                        accumulatedNumberString += token;
                    }
                }

                long parsedNumber = long.Parse(accumulatedNumberString);
                Console.WriteLine($"    Parsed number: {parsedNumber}");

                if (problem.operation == MathOperation.Add)
                {
                    problemTotal += parsedNumber;
                }
                else if (problem.operation == MathOperation.Multiply)
                {
                    problemTotal *= parsedNumber;
                }
            }

            Console.WriteLine($"Problem total: {problemTotal}");
            grandTotal += problemTotal;
        }

        Console.WriteLine($"Grand total: {grandTotal}");
    }

    #endregion

    #region Problem 7

    private static void Problem7Part1()
    {
        var fileName = "../../../../input/input-7.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        var firstLine = lines.First();
        var beamStartColumn = -1;
        for (int i = 0; i < firstLine.Length; i++)
        {
            if (firstLine[i] == 'S')
            {
                beamStartColumn = i;
                break;
            }
        }

        if (beamStartColumn == -1)
        {
            Console.WriteLine("Input missing initial bean location");
            return;
        }

        int splitCount = 0;
        HashSet<int> previousRowBeamColumns = new();
        previousRowBeamColumns.Add(beamStartColumn);
        HashSet<int> currentRowBeamColumns = new();
        for (int y = 1; y < lines.Count(); y++)
        {
            var currentLine = lines[y];
            foreach (var previousRowBeamColumn in previousRowBeamColumns)
            {
                if (currentLine[previousRowBeamColumn] == '^')
                {
                    splitCount++;
                    if (previousRowBeamColumn > 0)
                    {
                        currentRowBeamColumns.Add(previousRowBeamColumn - 1);
                    }
                    if (previousRowBeamColumn < currentLine.Length - 1)
                    {
                        currentRowBeamColumns.Add(previousRowBeamColumn + 1);
                    }
                }
                else
                {
                    currentRowBeamColumns.Add(previousRowBeamColumn);
                }
            }

            Console.WriteLine(currentLine);
            Console.WriteLine(string.Join(',', currentRowBeamColumns));
            previousRowBeamColumns.Clear();
            previousRowBeamColumns.UnionWith(currentRowBeamColumns);
            currentRowBeamColumns.Clear();
        }

        Console.WriteLine($"Split count: {splitCount}");
    }

    private record struct TachyonCoord(int x, int y);

    private class TachyonNode
    {
        public TachyonCoord coord;
        public List<TachyonNode> children = new();

        public TachyonNode(int x, int y)
        {
            this.coord = new TachyonCoord(x, y);
        }

        public TachyonNode(TachyonCoord coord)
        {
            this.coord = coord;
        }
    }

    private static void Problem7Part2()
    {
        var fileName = "../../../../input/input-7.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        var firstLine = lines.First();
        var beamStartColumn = -1;
        for (int i = 0; i < firstLine.Length; i++)
        {
            if (firstLine[i] == 'S')
            {
                beamStartColumn = i;
                break;
            }
        }

        if (beamStartColumn == -1)
        {
            Console.WriteLine("Input missing initial bean location");
            return;
        }

        int splitCount = 0;

        TachyonNode rootNode = new TachyonNode(beamStartColumn, 0);
        Dictionary<TachyonCoord, TachyonNode> nodesDict = new();
        nodesDict.Add(rootNode.coord, rootNode);

        List<TachyonNode> previousRowNodes = new();
        previousRowNodes.Add(rootNode);
        List<TachyonNode> currentRowNodes = new();

        for (int y = 1; y < lines.Count(); y++)
        {
            var currentLine = lines[y];
            Console.WriteLine(currentLine);
            foreach (var previousRowNode in previousRowNodes)
            {
                var x = previousRowNode.coord.x;
                if (currentLine[x] == '^')
                {
                    splitCount++;
                    if (x > 0)
                    {
                        var coord = new TachyonCoord(x - 1, y);
                        AddNodeHelper(coord, previousRowNode, nodesDict, currentRowNodes);
                    }
                    if (x < currentLine.Length - 1)
                    {
                        var coord = new TachyonCoord(x + 1, y);
                        AddNodeHelper(coord, previousRowNode, nodesDict, currentRowNodes);
                    }
                }
                else
                {
                    var coord = new TachyonCoord(x, y);
                    AddNodeHelper(coord, previousRowNode, nodesDict, currentRowNodes);
                }
            }

            previousRowNodes.Clear();
            previousRowNodes.AddRange(currentRowNodes);
            currentRowNodes.Clear();
        }

        Console.WriteLine($"Split count: {splitCount}");

        Dictionary<TachyonCoord, long> cachedPathCounts = new();

        var totalPaths = CountPaths(rootNode, cachedPathCounts);
        Console.WriteLine($"Total paths: {totalPaths}");
    }

    private static long CountPaths(TachyonNode node, Dictionary<TachyonCoord, long> cachedPathCounts)
    {
        if (node.children.Count == 0)
        {
            return 1;
        }

        long totalPaths = 0;
        foreach (var child in node.children)
        {
            if (cachedPathCounts.TryGetValue(child.coord, out var cachedChildPathCount))
            {
                totalPaths += cachedChildPathCount;
            }
            else
            {
                var childPathCount = CountPaths(child, cachedPathCounts);
                cachedPathCounts[child.coord] = childPathCount;
                totalPaths += childPathCount;
            } 
        }
        return totalPaths;
    }

    private static void AddNodeHelper(
        TachyonCoord coord,
        TachyonNode parent,
        Dictionary<TachyonCoord, TachyonNode> nodesDict,
        List<TachyonNode> currentRowNodes)
    {
        if (!nodesDict.TryGetValue(coord, out var newNode))
        {
            newNode = new TachyonNode(coord);
            nodesDict.Add(coord, newNode);
            Console.WriteLine($"    Adding NEW node: {coord}, parent {parent.coord}");
        }
        else
        {
            Console.WriteLine($"    Found OLD node: {coord}, parent {parent.coord}");
        }
        
        parent.children.Add(newNode);

        if (!currentRowNodes.Contains(newNode))
        {
            currentRowNodes.Add(newNode);
        }
    }

    #endregion

    #region Problem 8


    public class JunctionBox
    {
        public int x;
        public int y;
        public int z;

        public Circuit? ParentCircuit;

        public JunctionBox(int x, int y, int z, Circuit? parentCircuit = null)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            ParentCircuit = parentCircuit;
        }

        public float DistanceToOtherBox(JunctionBox otherBox)
        {
            var distanceSqrd = MathF.Pow((this.x - otherBox.x), 2) + 
                               MathF.Pow((this.y - otherBox.y), 2) +
                               MathF.Pow((this.z - otherBox.z), 2);
            return MathF.Sqrt(distanceSqrd);
        }

        public string DebugString => $"[{x},{y},{z}]";
    }

    public struct JunctionBoxDistance
    {
        public JunctionBox BoxA;
        public JunctionBox BoxB;

        public float Distance;

        public JunctionBoxDistance(JunctionBox boxA, JunctionBox boxB, float distance)
        {
            BoxA = boxA;
            BoxB = boxB;
            Distance = distance;
        }

        public string DebugString => $"Distance = {Distance}, {BoxA.DebugString} {BoxB.DebugString}";
    }

    public class Circuit
    {
        public List<JunctionBox> Boxes;

        public Circuit()
        {
            Boxes = new List<JunctionBox>();
        }

        public string DebugString => string.Join(",", Boxes.Select(x => x.DebugString));
    }

    private static void Problem8Part1()
    {
        var fileName = "../../../../input/input-8.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        List<JunctionBox> junctionBoxes = new();

        foreach (var line in lines)
        {
            var tokens = line.Split(",");
            var box = new JunctionBox(
                int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                int.Parse(tokens[2]));

            junctionBoxes.Add(box);
        }

        foreach (var position in junctionBoxes)
        {
            Console.WriteLine($"Junction box: {position.x}, {position.y}, {position.z}");
        }

        // Calculate distance between all pairs
        List<JunctionBoxDistance> distances = new();
        for (int a = 0; a <  junctionBoxes.Count; a++)
        {
            var boxA = junctionBoxes[a];
            for (int b = a + 1; b < junctionBoxes.Count; b++)
            {
                var boxB = junctionBoxes[b];
                var distance = boxA.DistanceToOtherBox(boxB);
                var boxDistance = new JunctionBoxDistance(boxA, boxB, distance);
                distances.Add(boxDistance);
            }
        }

        // Sort the distances
        distances.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        /*
        // Debug print the sorted distances
        foreach (var sortedDistance in distances)
        {
            Console.WriteLine($"Sorted distance: {sortedDistance.DebugString}");
        }
        */

        // Make circuits of size 1 for each junction box
        List<Circuit> circuits = new();
        foreach (var box in junctionBoxes)
        {
            var circuit = new Circuit();
            circuit.Boxes.Add(box);
            box.ParentCircuit = circuit;
            circuits.Add(circuit);
        }

        // Merge circuits for N closest pairs
        int numPairs = 1000;
        for (int i = 0; i < numPairs; i++)
        {
            var boxA = distances[i].BoxA;
            var boxB = distances[i].BoxB;
            Console.WriteLine($"MERGING, i = {i}, {boxA.DebugString} {boxB.DebugString}");

            if (boxA.ParentCircuit == null || boxB.ParentCircuit == null)
            {
                Console.WriteLine($"Error: box has null parent circuit, this should not happen");
                return;
            }

            if (boxA.ParentCircuit == boxB.ParentCircuit)
            {
                // Boxes are already connected
                continue;
            }

            // Move all the boxes from Box B's circuit over into Box A's circuit
            var otherCircuit = boxB.ParentCircuit;
            var otherCircuitList = new List<JunctionBox>(boxB.ParentCircuit.Boxes);
            foreach (var box in otherCircuitList)
            {
                boxA.ParentCircuit.Boxes.Add(box);
                box.ParentCircuit = boxA.ParentCircuit;
            }
            circuits.Remove(otherCircuit);

            Console.WriteLine("--------------------------------------");
        }

        // Sort circuits by size
        circuits.Sort((x, y) => x.Boxes.Count.CompareTo(y.Boxes.Count));
        circuits.Reverse();

        // Debug print sorted circuits
        foreach (var circuit in circuits)
        {
            Console.WriteLine(circuit.DebugString);
        }

        // Multiply size of 3 largest circuits
        var result = circuits[0].Boxes.Count * circuits[1].Boxes.Count * circuits[2].Boxes.Count;

        Console.WriteLine($"RESULT: {result}");
    }

    private static void Problem8Part2()
    {
        var fileName = "../../../../input/input-8.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        List<JunctionBox> junctionBoxes = new();

        foreach (var line in lines)
        {
            var tokens = line.Split(",");
            var box = new JunctionBox(
                int.Parse(tokens[0]),
                int.Parse(tokens[1]),
                int.Parse(tokens[2]));

            junctionBoxes.Add(box);
        }

        foreach (var position in junctionBoxes)
        {
            Console.WriteLine($"Junction box: {position.x}, {position.y}, {position.z}");
        }

        // Calculate distance between all pairs
        List<JunctionBoxDistance> distances = new();
        for (int a = 0; a < junctionBoxes.Count; a++)
        {
            var boxA = junctionBoxes[a];
            for (int b = a + 1; b < junctionBoxes.Count; b++)
            {
                var boxB = junctionBoxes[b];
                var distance = boxA.DistanceToOtherBox(boxB);
                var boxDistance = new JunctionBoxDistance(boxA, boxB, distance);
                distances.Add(boxDistance);
            }
        }

        // Sort the distances
        distances.Sort((a, b) => a.Distance.CompareTo(b.Distance));

        // Make circuits of size 1 for each junction box
        List<Circuit> circuits = new();
        foreach (var box in junctionBoxes)
        {
            var circuit = new Circuit();
            circuit.Boxes.Add(box);
            box.ParentCircuit = circuit;
            circuits.Add(circuit);
        }

        // Merge circuits for N closest pairs
        for (int i = 0; i < distances.Count; i++)
        {
            var boxA = distances[i].BoxA;
            var boxB = distances[i].BoxB;
            Console.WriteLine($"MERGING, i = {i}, {boxA.DebugString} {boxB.DebugString}");

            if (boxA.ParentCircuit == null || boxB.ParentCircuit == null)
            {
                Console.WriteLine($"Error: box has null parent circuit, this should not happen");
                return;
            }

            if (boxA.ParentCircuit == boxB.ParentCircuit)
            {
                // Boxes are already connected
                continue;
            }

            // Move all the boxes from Box B's circuit over into Box A's circuit
            var otherCircuit = boxB.ParentCircuit;
            var otherCircuitList = new List<JunctionBox>(boxB.ParentCircuit.Boxes);
            foreach (var box in otherCircuitList)
            {
                boxA.ParentCircuit.Boxes.Add(box);
                box.ParentCircuit = boxA.ParentCircuit;
            }
            circuits.Remove(otherCircuit);

            if (circuits.Count == 1)
            {
                Console.WriteLine($"DONE! Last connection made: {boxA.DebugString}, {boxB.DebugString}");
                long result = (long)boxA.x * (long)boxB.x;
                Console.WriteLine($"Result: {result}");
                break;
            }

            Console.WriteLine("--------------------------------------");
        }
    }

    #endregion

    #region Problem 9

    private struct TileLocation : IEquatable<TileLocation>
    {
        public int x;
        public int y;

        public TileLocation(int  x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(TileLocation other)
        {
            return this.x == other.x && this.y == other.y;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    private enum TileColor
    {
        Unknown,
        White,
        Red,
        Green
    }

    private static char TileColorDebugChar(TileColor color)
    {
        return color switch
        {
            TileColor.Unknown => '?',
            TileColor.White => '.',
            TileColor.Red => '#',
            TileColor.Green => 'X',
            _ => '?'
        };
    }

    private static void Problem9Part1()
    {
        var fileName = "../../../../input/input-9.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        // Parse all the red tile locations
        List<TileLocation> tileLocations = new();
        foreach(var line in lines)
        {
            var tokens = line.Split(',');
            var x = int.Parse(tokens[0]);
            var y = int.Parse(tokens[1]);

            tileLocations.Add(new TileLocation(x, y));
        }

        long maxArea = long.MinValue;
        foreach (var tileA in tileLocations)
        {
            foreach (var tileB in tileLocations)
            {
                if (tileA.Equals(tileB))
                {
                    continue;
                }

                var area = ((long)Math.Abs(tileA.x - tileB.x) + 1) * ((long)Math.Abs(tileA.y - tileB.y) + 1);
                if (area > maxArea)
                {
                    maxArea = area;
                }
            }
        }

        Console.WriteLine($"Result: {maxArea}");
    }

    public class Packed2Bit2DArray
    {
        private readonly uint[] _data;
        private readonly int _width;
        private readonly int _height;
        private readonly long _numElements;

        public Packed2Bit2DArray(int width, int height)
        {
            _width = width;
            _height = height;

            // Calculate number of elements needed
            long numInputElements = (long)width * (long)height;
            _numElements = ((long)numInputElements / 16) + 1;    // 32 bits per uint / 2 bits per value = 16
            _data = new uint[_numElements];
        }

        public uint GetValue(int x, int y)
        {
            // Turn input coordinates into 1D
            var position1D = (long)y * _width + x;

            // Turn 1D position into packed space
            var packedY = position1D / 16;
            var packedX = position1D % 16;

            // Get the number that contains the needed bits, and determine the indices of those bits
            var packedData = _data[packedY];
            var packedXBit1 = packedX * 2;
            var packedXBit2 = packedXBit1 + 1;

            // Get the bits
            var bit1 = GetNthBit(packedData, (int)packedXBit1);
            var bit2 = GetNthBit(packedData, (int)packedXBit2);

            // Turn the bits into a single return value
            var value = bit1 * 2 + bit2;
            return value;
        }

        public void SetValue(int x, int y, uint value)
        {
            if (value >= 4)
            {
                Console.WriteLine($"Error: trying to set Packed2Bit2DArray value to {value}. Max value is 3");
            }

            // Turn input coordinates into 1D
            long position1D = (long)y * _width + x;

            // Turn 1D position into packed space
            var packedY = position1D / 16;
            var packedX = position1D % 16;

            // Get the number that contains the needed bits, and determine the indices of those bits
            var packedData = _data[packedY];
            var packedXBit1 = packedX * 2;
            var packedXBit2 = packedXBit1 + 1;

            // Turn the value into bits
            var firstBitValue = (value / 2) == 1;
            var secondBitValue = (value % 2) == 1;

            // Set the bit values
            var newPackedData = SetNthBit(packedData, (int)packedXBit1, firstBitValue);
            newPackedData = SetNthBit(newPackedData, (int)packedXBit2, secondBitValue);
            _data[packedY] = newPackedData;
        }

        public static uint GetNthBit(uint number, int bitIndex)
        {
            return ((number >> bitIndex) & 1);
        }

        public static uint SetNthBit(uint number, int bitIndex, bool bitValue)
        {
            uint mask = 1u << bitIndex;

            if (bitValue)
            {
                // Set bit to 1
                return number | mask;
            }
            else
            {
                // Set bit to 0
                return number & ~mask;
            }
        }
    }

    private static void Problem9Part2()
    {
        var fileName = "../../../../input/input-9.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        // Parse all the red tile locations
        List<TileLocation> redTileLocations = new();
        foreach (var line in lines)
        {
            var tokens = line.Split(',');
            var x = int.Parse(tokens[0]) + 1; // Shift all positions by 1 to account for the border
            var y = int.Parse(tokens[1]) + 1;

            redTileLocations.Add(new TileLocation(x, y));
        }

        // Get the dimensions of the floor by finding the max tile x and y values
        // Add a border (two extra rows on the top and bottom, and two extra columns
        // on the left and right) to make the fill algorithm simpler
        var gridWidth = redTileLocations.Max(t => t.x) + 2;
        var gridHeight = redTileLocations.Max(t => t.y) + 2;

        // Initialize the grid of tiles - all tiles are Unknown to start
        //TileColor[,] tileGrid = new TileColor[gridWidth, gridHeight];
        Packed2Bit2DArray tileGrid = new Packed2Bit2DArray(gridWidth, gridHeight);

        // Add the red tiles
        foreach (var redTileLocation in redTileLocations)
        {
            tileGrid.SetValue(redTileLocation.x, redTileLocation.y, (uint)TileColor.Red);
        }

        // Add the green tiles that are in between the red tiles (draw the edge of the "loop")
        for (int i = 0; i < redTileLocations.Count; i++)
        {
            var currentRedTile = redTileLocations[i];
            var nextRedTile = redTileLocations[(i + 1) % redTileLocations.Count];

            if (currentRedTile.x == nextRedTile.x)
            {
                int yStart = Math.Min(currentRedTile.y, nextRedTile.y) + 1;
                int yEnd = Math.Max(currentRedTile.y, nextRedTile.y) - 1;

                for (int y = yStart; y <= yEnd; y++)
                {
                    tileGrid.SetValue(currentRedTile.x, y, (uint)TileColor.Green);
                }
            }
            else if (currentRedTile.y == nextRedTile.y)
            {
                int xStart = Math.Min(currentRedTile.x, nextRedTile.x) + 1;
                int xEnd = Math.Max(currentRedTile.x, nextRedTile.x) - 1;

                for (int x = xStart; x <= xEnd; x++)
                {
                    tileGrid.SetValue(x, currentRedTile.y, (uint)TileColor.Green);
                }
            }
            else
            {
                Console.WriteLine("Error: consecutive red tiles are not the same row or column");
                return;
            }
        }

        // Fill in the white tiles (tiles that are known to be *outside* the loop)
        // Start with the upper left corner - due to the added border, it's guaranteed
        // to be outside the loop. Recursively fill any neighboring tiles that are not
        // already red or green. 
        FillFromTile(new TileLocation(0, 0), tileGrid, gridWidth, gridHeight);

        // Now that all the outside tiles are marked as white, any remaining unknown tiles must be
        // inside the loop - mark them all as green.
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if ((TileColor)tileGrid.GetValue(x, y) == TileColor.Unknown)
                {
                    tileGrid.SetValue(x, y, (uint)TileColor.Green);
                }
            }
        }

        // Debug print a diagram of the floor
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                Console.Write(TileColorDebugChar((TileColor)tileGrid.GetValue(x, y)));
            }
            Console.WriteLine();
        }

        // Check all pairs of red tile locations to see which are valid rectangles
        // (containing only red or green tiles), and find the max valid area
        long maxArea = long.MinValue;
        foreach (var tileA in redTileLocations)
        {
            foreach (var tileB in redTileLocations)
            {
                if (tileA.Equals(tileB))
                {
                    continue;
                }

                var area = ((long)Math.Abs(tileA.x - tileB.x) + 1) * ((long)Math.Abs(tileA.y - tileB.y) + 1);
                if (area > maxArea)
                {
                    if (IsRectangleValid(tileA, tileB, tileGrid))
                    {
                        maxArea = area;
                    }
                }
            }
        }

        Console.WriteLine($"Max valid area: {maxArea}");
    }

    private static void FillFromTile(TileLocation tile, Packed2Bit2DArray tileGrid, int gridWidth, int gridHeight)
    {
        List<TileLocation> unknownNeighbors = new();
        unknownNeighbors.Add(tile);

        long numTilesFilled = 0;
        long totalTiles = (long)gridWidth * gridHeight;

        while (unknownNeighbors.Count > 0)
        {
            var currentTile = unknownNeighbors.First();

            // If this tile is currently unknown, we can fill it. Otherwise, do nothing
            if ((TileColor)tileGrid.GetValue(currentTile.x, currentTile.y) == TileColor.Unknown)
            {
                // Mark the tile as known (white)
                tileGrid.SetValue(currentTile.x, currentTile.y, (uint)TileColor.White);
                numTilesFilled++;

                if (numTilesFilled % 50000 == 0)
                {
                    Console.WriteLine($"Tiles filled: {numTilesFilled} / {totalTiles}");
                }

                // Get the list of unknown neighbors

                for (int y = currentTile.y - 1; y <= currentTile.y + 1; y++)
                {
                    for (int x = currentTile.x - 1; x <= currentTile.x + 1; x++)
                    {
                        var color = SafeGetColor(x, y, tileGrid, gridWidth, gridHeight);
                        if (color == TileColor.Unknown)
                        {
                            unknownNeighbors.Add(new TileLocation(x, y));
                        }
                    }
                }
            }

            unknownNeighbors.Remove(currentTile);
        }
        

    }

    private static TileColor SafeGetColor(int x, int y, Packed2Bit2DArray tileGrid, int gridWidth, int gridHeight)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
        {
            return TileColor.White;
        }
        return (TileColor)tileGrid.GetValue(x, y);
    }

    private static bool IsRectangleValid(TileLocation tileA, TileLocation tileB, Packed2Bit2DArray tileGrid)
    {
        var minX = Math.Min(tileA.x, tileB.x);
        var maxX = Math.Max(tileA.x, tileB.x);

        var minY = Math.Min(tileA.y, tileB.y);
        var maxY = Math.Max(tileA.y, tileB.y);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                var color = (TileColor)tileGrid.GetValue(x, y);
                if (color != TileColor.Red && color != TileColor.Green)
                {
                    return false;
                }
            }
        }

        return true;
    }

    #endregion

    #region Problem 10

    public struct IndicatorLightDiagram : IEquatable<IndicatorLightDiagram>
    {
        public List<bool> LightStates;

        public IndicatorLightDiagram(List<bool> lightStates)
        {
            LightStates = lightStates;
        }

        public bool Equals(IndicatorLightDiagram other)
        {
            if (other.LightStates.Count != LightStates.Count)
            {
                return false;
            }

            for (int i = 0; i < LightStates.Count; i++)
            {
                if (other.LightStates[i] != LightStates[i])
                {
                    return false;
                }
            }

            return true;
        }

        public void ApplyButtonPresses(ButtonWiring buttonWiring)
        {
            foreach(var button in buttonWiring.Buttons)
            {
                LightStates[button] = !LightStates[button];
            }
        }

        public IndicatorLightDiagram DeepCopy()
        {
            var newList = new List<bool>(LightStates);
            return new IndicatorLightDiagram(newList);
        }

        public string StateString => string.Join("", LightStates.Select(x => x ? "1" : "0"));
    }

    public struct ButtonWiring
    {
        public HashSet<int> Buttons;

        public ButtonWiring(HashSet<int> buttons)
        {
            Buttons = buttons;
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var item in Buttons)
            {
                hashCode.Add(item);
            }
            return hashCode.ToHashCode();
        }

        public string DebugString => string.Join(",", Buttons);
    }

    public struct JoltageRequirements
    {
        public List<int> Joltages;

        public JoltageRequirements(List<int> joltages)
        {
            Joltages = joltages;
        }

        public JoltageRequirements DeepCopy()
        {
            var newList = new List<int>(Joltages);
            return new JoltageRequirements(newList);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            foreach (var item in Joltages)
            {
                hashCode.Add(item);
            }
            return hashCode.ToHashCode();
        }

        public void ApplyButtonPresses(ButtonWiring buttonWiring)
        {
            foreach (var button in buttonWiring.Buttons)
            {
                Joltages[button]++;
            }
        }

        public bool Equals(JoltageRequirements other)
        {
            if (other.Joltages.Count != Joltages.Count)
            {
                return false;
            }

            for (int i = 0; i < Joltages.Count; i++)
            {
                if (other.Joltages[i] != Joltages[i])
                {
                    return false;
                }
            }

            return true;
        }

        public string DebugString => String.Join(",", Joltages);
    }

    private static void Problem10Part1()
    {
        var fileName = "../../../../input/input-10.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        long totalNumPushes = 0;

        foreach (var line in lines)
        {
            var tokens = line.Split(' ');
            var lightDiagreamString = tokens[0];
            List<string> buttonWiringStrings = new();
            string joltageString = "";
            for (int i = 1; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token.StartsWith('('))
                {
                    buttonWiringStrings.Add(token);
                }
                else if (token.StartsWith('{'))
                {
                    if (i != tokens.Length - 1)
                    {
                        Console.WriteLine("Error: unexpected input string");
                        return;
                    }
                    joltageString = token;
                }
            }

            // Parse the machine indicator light diagram
            List<bool> targetLightState = new();
            foreach (var character in lightDiagreamString)
            {
                if (character == '.')
                {
                    targetLightState.Add(false);
                }
                else if (character == '#')
                {
                    targetLightState.Add(true);
                }
            }
            IndicatorLightDiagram targetLightDiagram = new IndicatorLightDiagram(targetLightState);


            // Parse the button wirings
            List<ButtonWiring> buttonWirings = new();
            foreach(var buttonWiringString in buttonWiringStrings)
            {
                HashSet<int> buttons = new();
                var strippedString = buttonWiringString.Replace("(", "");
                strippedString = strippedString.Replace(")", "");
                var wiringTokens = strippedString.Split(",");
                foreach (var wiringToken in wiringTokens)
                {
                    buttons.Add(int.Parse(wiringToken));
                }
                var buttonWiring = new ButtonWiring(buttons);
                buttonWirings.Add(buttonWiring);
            }

            var minNumPushes = GetMinNumberOfPushesToReachState(targetLightDiagram, buttonWirings);
            Console.WriteLine($"Min pushes: {minNumPushes}");
            totalNumPushes += minNumPushes;
        }

        Console.WriteLine($"RESULT: Total num pushes = {totalNumPushes}");
    }

    private static void Problem10Part2()
    {
        var fileName = "../../../../input/input-10-example.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        long totalNumPushes = 0;

        foreach (var line in lines)
        {
            var tokens = line.Split(' ');
            var lightDiagreamString = tokens[0];
            List<string> buttonWiringStrings = new();
            string joltageString = "";
            for (int i = 1; i < tokens.Length; i++)
            {
                var token = tokens[i];
                if (token.StartsWith('('))
                {
                    buttonWiringStrings.Add(token);
                }
                else if (token.StartsWith('{'))
                {
                    if (i != tokens.Length - 1)
                    {
                        Console.WriteLine("Error: unexpected input string");
                        return;
                    }
                    joltageString = token;
                }
            }

            // Parse the button wirings
            List<ButtonWiring> buttonWirings = new();
            foreach (var buttonWiringString in buttonWiringStrings)
            {
                HashSet<int> buttons = new();
                var strippedString = buttonWiringString.Replace("(", "");
                strippedString = strippedString.Replace(")", "");
                var wiringTokens = strippedString.Split(",");
                foreach (var wiringToken in wiringTokens)
                {
                    buttons.Add(int.Parse(wiringToken));
                }
                var buttonWiring = new ButtonWiring(buttons);
                buttonWirings.Add(buttonWiring);
            }

            // Parse the joltage requirements
            var strippedJoltageString = joltageString.Replace("{", "");
            strippedJoltageString = strippedJoltageString.Replace("}", "");
            var joltageTokens = strippedJoltageString.Split(",");
            List<int> joltageList = new();
            foreach (var  joltageToken in joltageTokens)
            {
                joltageList.Add(int.Parse(joltageToken));
            }
            JoltageRequirements joltageRequirements = new JoltageRequirements(joltageList);

            var minNumPushes = GetMinNumberOfPushesToReachJoltage(joltageRequirements, buttonWirings);
            Console.WriteLine($"Min pushes: {minNumPushes}");
            totalNumPushes += minNumPushes;
        }

        Console.WriteLine($"RESULT: Total num pushes = {totalNumPushes}");
    }

    private struct SearchNode
    {
        public int Depth;
        public IndicatorLightDiagram CurrentState;
        public ButtonWiring ButtonWiring;

        public string DebugString
        {
            get
            {
                var buttonString = string.Join(",", ButtonWiring.Buttons);
                return $"Depth = {Depth}, State = {CurrentState.StateString}, Buttons = {buttonString}";
            }
        }
    }

    private static int GetMinNumberOfPushesToReachState(IndicatorLightDiagram targetState, List<ButtonWiring> buttonWirings)
    {
        var numLights = targetState.LightStates.Count;
        var initialState = new IndicatorLightDiagram(Enumerable.Repeat(false, numLights).ToList());

        if (initialState.Equals(targetState))
        {
            return 0;
        }

        Dictionary<string, HashSet<ButtonWiring>> visitedNodes = new();

        Queue<SearchNode> searchNodes = new();
        AddButtonWiringsToQueue(searchNodes, 1, initialState, buttonWirings, visitedNodes);

        while (searchNodes.Count > 0)
        {
            //Console.WriteLine($"Dequeing. Queue size: {searchNodes.Count} -------------------------------------");
            var node = searchNodes.Dequeue();

            if (searchNodes.Count % 10000 == 0)
            {
                Console.WriteLine($"Queue size: {searchNodes.Count}, current depth = {node.Depth}");
            }

            //Console.WriteLine($"Testing node: {node.DebugString}");

            //Console.WriteLine($"... old state: {node.CurrentState.StateString}");
            var newState = node.CurrentState.DeepCopy();
            newState.ApplyButtonPresses(node.ButtonWiring);
            //Console.WriteLine($"... new state: {newState.StateString}");
            if (newState.Equals(targetState))
            {
                return node.Depth;
            }
            AddButtonWiringsToQueue(searchNodes, node.Depth + 1, newState, buttonWirings, visitedNodes);
        }

        Console.WriteLine("Error: unable to find press count");
        return -1;
    }

    private static void AddButtonWiringsToQueue(
        Queue<SearchNode> searchNodes,
        int depth,
        IndicatorLightDiagram currentState,
        List<ButtonWiring> buttonWirings,
        Dictionary<string, HashSet<ButtonWiring>> visitedNodes)
    {
        foreach (var buttonWiring in buttonWirings)
        {
            // Check whether we've already explored this node
            if (visitedNodes.TryGetValue(currentState.StateString, out var buttonWiringsSet))
            {
                if (buttonWiringsSet.Contains(buttonWiring))
                {
                    continue;
                }
            }

            var node = new SearchNode { Depth = depth, CurrentState = currentState, ButtonWiring = buttonWiring };
            searchNodes.Enqueue(node);

            if (!visitedNodes.ContainsKey(currentState.StateString))
            {
                visitedNodes.Add(currentState.StateString, new HashSet<ButtonWiring> { buttonWiring });
            }
            else
            {
                visitedNodes[currentState.StateString].Add(buttonWiring);
            }
        }
    }

    private static int GetMinNumberOfPushesToReachJoltage(JoltageRequirements targetState, List<ButtonWiring> buttonWirings)
    {
        var numJoltages = targetState.Joltages.Count;
        var initialState = new JoltageRequirements(Enumerable.Repeat(0, numJoltages).ToList());

        if (initialState.Equals(targetState))
        {
            return 0;
        }

        Dictionary<JoltageRequirements, HashSet<ButtonWiring>> visitedNodes = new();

        Queue<JoltageSearchNode> searchNodes = new();
        AddJoltageButtonWiringsToQueue(searchNodes, 1, initialState, targetState, buttonWirings, visitedNodes);

        while (searchNodes.Count > 0)
        {
            //Console.WriteLine($"Dequeing. Queue size: {searchNodes.Count} -------------------------------------");
            var node = searchNodes.Dequeue();

            if (searchNodes.Count % 10000 == 0)
            {
                Console.WriteLine($"Queue size: {searchNodes.Count}, current depth = {node.Depth}");
            }

            //Console.WriteLine($"Testing node: {node.DebugString}");

            //Console.WriteLine($"... old state: {node.CurrentState.StateString}");
            var newState = node.CurrentState.DeepCopy();
            newState.ApplyButtonPresses(node.ButtonWiring);
            //Console.WriteLine($"... new state: {newState.StateString}");
            if (newState.Equals(targetState))
            {
                return node.Depth;
            }
            AddJoltageButtonWiringsToQueue(searchNodes, node.Depth + 1, newState, targetState, buttonWirings, visitedNodes);
        }

        Console.WriteLine("Error: unable to find press count");
        return -1;
    }

    private struct JoltageSearchNode
    {
        public int Depth;
        public JoltageRequirements CurrentState;
        public ButtonWiring ButtonWiring;

        public int[] CumulativePresses;

        public string DebugString
        {
            get
            {
                var buttonString = string.Join(",", ButtonWiring.Buttons);
                return $"Depth = {Depth}, State = {CurrentState.DebugString}, Buttons = {buttonString}";
            }
        }
    }

    private static void AddJoltageButtonWiringsToQueue(
        Queue<JoltageSearchNode> searchNodes,
        int depth,
        JoltageRequirements currentState,
        JoltageRequirements targetState,
        List<ButtonWiring> buttonWirings,
        Dictionary<JoltageRequirements, HashSet<ButtonWiring>> visitedNodes)
    {
        foreach (var buttonWiring in buttonWirings)
        {
            /*
            if (depth <= 3)
            {
                Console.WriteLine("--------------------------------------");
                Console.WriteLine($"currentState = {currentState.DebugString}, hash code = {currentState.GetHashCode()}");
                Console.WriteLine($"current wiring: {buttonWiring.DebugString}, hash code = {buttonWiring.GetHashCode()}");
                Console.WriteLine("visited nodes dict:");
            }
            */
                foreach (var key in visitedNodes.Keys)
                {
                    //Console.WriteLine($"... {key.DebugString} {key.GetHashCode()}");
                    if (key.GetHashCode() == currentState.GetHashCode())
                    {
                        //Console.WriteLine("KEY HASH CODE MATCH");
                        var matchingValue = visitedNodes[key];
                        //Console.WriteLine($"matching value: {matchingValue}");

                        if (matchingValue.Contains(buttonWiring))
                        {
                            //Console.WriteLine("Skipped previously visited node???????????????????????????");
                            continue;
                        }
                    }
                }
            

            // Check whether we've already explored this node
            if (visitedNodes.ContainsKey(currentState))
            {
                var buttonWiringSet = visitedNodes[currentState];

                /*
                if (depth <= 3)
                {
                    Console.WriteLine($"State key found!");
                    Console.WriteLine("visited wirings:");
                    foreach (var bw in buttonWiringSet)
                    {
                        Console.WriteLine($"... {bw.DebugString}, hash = {bw.GetHashCode()}");
                    }
                }
                */
                if (buttonWiringSet.Contains(buttonWiring))
                {
                    Console.WriteLine("Skipped previously visited node!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    continue;
                }
            }
            else
            {
                if (depth <= 3) Console.WriteLine($"key not found");
            }

            // Check whether any of the joltages in this state are already above the target
            // If so, this node is invalid since the joltages can never go back down
            for (int i = 0; i < currentState.Joltages.Count; i++)
            {
                if (currentState.Joltages[i] > targetState.Joltages[i])
                {
                    //Console.WriteLine("Skipped node with overflowed state");
                    continue;
                }
            }

            var node = new JoltageSearchNode { Depth = depth, CurrentState = currentState, ButtonWiring = buttonWiring };
            searchNodes.Enqueue(node);

            if (!visitedNodes.ContainsKey(currentState))
            {
                visitedNodes.Add(currentState, new HashSet<ButtonWiring> { buttonWiring });
            }
            else
            {
                visitedNodes[currentState].Add(buttonWiring);
            }
        }
    }
    #endregion
}
