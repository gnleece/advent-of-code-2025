namespace advent_of_code_2025;

internal partial class Program
{
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
}
