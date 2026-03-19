namespace advent_of_code_2025;

internal partial class Program
{
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
}
