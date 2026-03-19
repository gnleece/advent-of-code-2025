namespace advent_of_code_2025;

internal partial class Program
{
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
}
