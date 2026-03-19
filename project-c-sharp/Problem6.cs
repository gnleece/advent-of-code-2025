namespace advent_of_code_2025;

internal partial class Program
{
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
}
