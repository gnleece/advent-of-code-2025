namespace advent_of_code_2025;

internal partial class Program
{
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
}
