namespace advent_of_code_2025;

internal partial class Program
{
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
}
