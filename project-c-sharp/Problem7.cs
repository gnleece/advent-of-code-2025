namespace advent_of_code_2025;

internal partial class Program
{
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
}
