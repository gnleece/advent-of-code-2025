namespace advent_of_code_2025;

internal partial class Program
{
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
}
