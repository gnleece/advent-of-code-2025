namespace advent_of_code_2025;

internal partial class Program
{
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
}
