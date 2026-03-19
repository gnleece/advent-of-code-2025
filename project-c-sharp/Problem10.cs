using System.Diagnostics;

namespace advent_of_code_2025;

internal partial class Program
{
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
            for (int i = 0; i < buttonWiring.Buttons.Length; i++)
            {
                if (buttonWiring.Buttons[i] == 1)
                {
                    LightStates[i] = !LightStates[i];
                }
            }
        }

        public IndicatorLightDiagram DeepCopy()
        {
            var newList = new List<bool>(LightStates);
            return new IndicatorLightDiagram(newList);
        }

        public string StateString => string.Join("", LightStates.Select(x => x ? "1" : "0"));
    }

    public class ButtonWiring
    {
        public int[] Buttons;

        public ButtonWiring(int[] buttons)
        {
            Buttons = buttons;
        }

        public string DebugString => string.Join(",", Buttons);
    }

    public class JoltageRequirements : IEquatable<JoltageRequirements>
    {
        public IReadOnlyList<int> Joltages => _joltages;

        private List<int> _joltages;
        private int _hashCode = 0;

        public JoltageRequirements(List<int> joltages)
        {
            _joltages = joltages;
            _hashCode = ComputeHashCode();
        }

        public JoltageRequirements DeepCopy()
        {
            var newList = new List<int>(Joltages);
            return new JoltageRequirements(newList);
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        private int ComputeHashCode()
        {
            var hashCode = new HashCode();
            foreach (var item in Joltages)
            {
                hashCode.Add(item);
            }
            return hashCode.ToHashCode();
        }

        public override bool Equals(object? obj) => obj is JoltageRequirements other && Equals(other);

        public static JoltageRequirements ApplyButtonPresses(JoltageRequirements initialState, ButtonWiring buttonWiring, bool forward)
        {
            var newJoltages = new List<int>(initialState.Joltages);
            for (int i = 0; i < buttonWiring.Buttons.Length; i++)
            {
                if (buttonWiring.Buttons[i] == 1)
                {
                    newJoltages[i] += forward ? 1 : -1;
                }
            }
            return new JoltageRequirements(newJoltages);
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
                int[] buttons = new int[targetLightState.Count];
                var strippedString = buttonWiringString.Replace("(", "");
                strippedString = strippedString.Replace(")", "");
                var wiringTokens = strippedString.Split(",");
                foreach (var wiringToken in wiringTokens)
                {
                    var index = int.Parse(wiringToken);
                    if (index < 0 || index >= targetLightState.Count)
                    {
                        Console.WriteLine($"Error: button wiring index {index} is out of bounds for number of machines {targetLightState.Count}");
                        return;
                    }
                    buttons[index] = 1;
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
        var fileName = "../../../../input/input-10.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        long totalNumPushes = 0;
        int testCaseNumber = 1;

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

            // Parse the joltage requirements
            var strippedJoltageString = joltageString.Replace("{", "");
            strippedJoltageString = strippedJoltageString.Replace("}", "");
            var joltageTokens = strippedJoltageString.Split(",");
            List<int> joltageList = new();
            foreach (var joltageToken in joltageTokens)
            {
                joltageList.Add(int.Parse(joltageToken));
            }
            JoltageRequirements joltageRequirements = new JoltageRequirements(joltageList);

            var numMachines = joltageRequirements.Joltages.Count;

            // Parse the button wirings
            List<ButtonWiring> buttonWirings = new();
            foreach (var buttonWiringString in buttonWiringStrings)
            {
                int[] buttons = new int[numMachines];
                var strippedString = buttonWiringString.Replace("(", "");
                strippedString = strippedString.Replace(")", "");
                var wiringTokens = strippedString.Split(",");
                foreach (var wiringToken in wiringTokens)
                {
                    var index = int.Parse(wiringToken);
                    if (index < 0 || index >= numMachines)
                    {
                        Console.WriteLine($"Error: button wiring index {index} is out of bounds for number of machines {numMachines}");
                        return;
                    }
                    buttons[index] = 1;
                }
                var buttonWiring = new ButtonWiring(buttons);
                buttonWirings.Add(buttonWiring);
            }

            var minNumPushes = GetMinNumberOfPushesToReachJoltage(joltageRequirements, buttonWirings, testCaseNumber);
            Console.WriteLine($"Min pushes: {minNumPushes}");
            totalNumPushes += minNumPushes;
            testCaseNumber++;
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

    private static int GetMinNumberOfPushesToReachJoltage(JoltageRequirements targetState, List<ButtonWiring> buttonWirings, int testCaseNumber)
    {
        var numJoltages = targetState.Joltages.Count;
        var initialState = new JoltageRequirements(Enumerable.Repeat(0, numJoltages).ToList());

        if (initialState.Equals(targetState))
        {
            return 0;
        }

        Dictionary<JoltageRequirements, int> forwardVisitedStates = new();
        Dictionary<JoltageRequirements, int> backwardVisitedStates = new();

        Queue<JoltageSearchNode> searchNodes = new();
        AddJoltageButtonWiringsToQueue(searchNodes, 1, initialState, targetState, buttonWirings, true);
        AddJoltageButtonWiringsToQueue(searchNodes, 1, targetState, initialState, buttonWirings, false);

        Stopwatch totalStopwatch = new Stopwatch();
        totalStopwatch.Start();
        Stopwatch debugPrintStopwatch = new Stopwatch();
        debugPrintStopwatch.Start();

        var debugUpdateIntervalms = 1000;

        while (searchNodes.Count > 0)
        {
            var node = searchNodes.Dequeue();

            if (debugPrintStopwatch.ElapsedMilliseconds > debugUpdateIntervalms)
            {
                //Console.WriteLine($"[Test {testCaseNumber}] Queue size: {searchNodes.Count}, current depth: {node.Depth}, elapsed time: {totalStopwatch.Elapsed.TotalSeconds}");
                var directionString = node.Forward ? "forward" : "backward";
                Console.WriteLine($"... testing node with state {node.CurrentState.DebugString}, direction = {directionString}");
                debugPrintStopwatch.Restart();
            }

            var newState = JoltageRequirements.ApplyButtonPresses(node.CurrentState, node.ButtonWiring, node.Forward);
            if ((node.Forward && newState.Equals(targetState)) ||
                (!node.Forward && newState.Equals(initialState)))
            {
                totalStopwatch.Stop();
                Console.WriteLine($"Done. Total elapsed time: {totalStopwatch.Elapsed.TotalSeconds} seconds");
                return node.Depth;
            }

            if (node.Forward && backwardVisitedStates.TryGetValue(newState, out var backwardsDepth))
            {
                totalStopwatch.Stop();
                Console.WriteLine($"Done. Total elapsed time: {totalStopwatch.Elapsed.TotalSeconds} seconds");
                return node.Depth + backwardsDepth - 1;
            }
            else if (!node.Forward && forwardVisitedStates.TryGetValue(newState, out var forwardsDepth))
            {
                totalStopwatch.Stop();
                Console.WriteLine($"Done. Total elapsed time: {totalStopwatch.Elapsed.TotalSeconds} seconds");
                return node.Depth + forwardsDepth - 1;
            }

            if (node.Forward && !forwardVisitedStates.ContainsKey(newState))
            {
                AddJoltageButtonWiringsToQueue(searchNodes, node.Depth + 1, newState, targetState, buttonWirings, node.Forward);
                forwardVisitedStates.Add(newState, node.Depth + 1);
            }
            else if (!node.Forward && !backwardVisitedStates.ContainsKey(newState))
            {
                AddJoltageButtonWiringsToQueue(searchNodes, node.Depth + 1, newState, initialState, buttonWirings, node.Forward);
                backwardVisitedStates.Add(newState, node.Depth + 1);
            }
        }

        Console.WriteLine("Error: unable to find push count");
        return -1;
    }

    private struct JoltageSearchNode
    {
        public int Depth;
        public JoltageRequirements CurrentState;
        public ButtonWiring ButtonWiring;
        public bool Forward;

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
        bool forward)
    {
        // Check whether any of the joltages in this state are already beyond the target
        // If so, this node is invalid since the joltages can never go back down
        for (int i = 0; i < currentState.Joltages.Count; i++)
        {
            if ((forward && currentState.Joltages[i] > targetState.Joltages[i]) ||
                !forward && currentState.Joltages[i] < targetState.Joltages[i])
            {
                //Console.WriteLine("Skipped node with overflowed state");
                return;
            }
        }

        foreach (var buttonWiring in buttonWirings)
        {
            var node = new JoltageSearchNode { Depth = depth, CurrentState = currentState, ButtonWiring = buttonWiring, Forward = forward };
            searchNodes.Enqueue(node);
        }
    }

    #region Problem 10 version B

    private static void Problem10Part2VersionB()
    {
        var fileName = "../../../../input/input-10.txt";
        var lines = File.ReadLines(fileName).ToArray();

        if (lines.Count() == 0)
        {
            Console.WriteLine("Empty input");
            return;
        }

        long totalNumPushes = 0;
        int testCaseNumber = 1;

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

            // Parse the joltage requirements
            var strippedJoltageString = joltageString.Replace("{", "");
            strippedJoltageString = strippedJoltageString.Replace("}", "");
            var joltageTokens = strippedJoltageString.Split(",");
            var numMachines = joltageTokens.Length;
            int[] targetJoltageArray = new int[numMachines];
            for (int i = 0; i  < numMachines; i++)
            {
                targetJoltageArray[i] = int.Parse(joltageTokens[i]);
            }
            JoltageVector targetVector = new JoltageVector(targetJoltageArray);

            // Parse the button wirings
            List<JoltageVector> buttonVectors = new();
            foreach (var buttonWiringString in buttonWiringStrings)
            {
                int[] buttonVectorArray = new int[numMachines];
                var strippedString = buttonWiringString.Replace("(", "");
                strippedString = strippedString.Replace(")", "");
                var wiringTokens = strippedString.Split(",");
                foreach (var wiringToken in wiringTokens)
                {
                    var index = int.Parse(wiringToken);
                    if (index < 0 || index >= numMachines)
                    {
                        Console.WriteLine($"Error: button wiring index {index} is out of bounds for number of machines {numMachines}");
                        return;
                    }
                    buttonVectorArray[index] = 1;
                }
                var buttonVector = new JoltageVector(buttonVectorArray);
                buttonVectors.Add(buttonVector);
            }

            var minNumPushes = GetMinNumberOfPushesToReachJoltageV2(targetVector, buttonVectors, testCaseNumber);
            Console.WriteLine($"Min pushes: {minNumPushes}");
            totalNumPushes += minNumPushes;
            testCaseNumber++;
        }

        Console.WriteLine($"RESULT: Total num pushes = {totalNumPushes}");
    }

    private static int GetMinNumberOfPushesToReachJoltageV2(JoltageVector targetStateVector, List<JoltageVector> buttonVectors, int testCaseNumber)
    {
        int maxButtonVectorSize = buttonVectors.Max(v => v.Size);
        int minPossiblePushes = (int)Math.Ceiling((double)targetStateVector.Size / maxButtonVectorSize);

        var stateVectorLength = targetStateVector.JoltageValues.Length;
        var initialStateVector = new JoltageVector(new int[stateVectorLength]);

        if (initialStateVector.Equals(targetStateVector))
        {
            return 0;
        }

        var numButtonWeights = buttonVectors.Count();
        int[] initialButtonWeights = new int[numButtonWeights];

        Queue<JoltageSearchNodeV2> searchNodes = new();
        searchNodes.Enqueue(new JoltageSearchNodeV2(initialButtonWeights));

        HashSet<JoltageVector> visitedStateVectors = new();

        Stopwatch totalStopwatch = new Stopwatch();
        Stopwatch debugPrintStopwatch = new Stopwatch();
        totalStopwatch.Start();
        debugPrintStopwatch.Start();

        var debugUpdateIntervalms = 1000;

        while (searchNodes.Count > 0)
        {
            var node = searchNodes.Dequeue();

            if (debugPrintStopwatch.ElapsedMilliseconds > debugUpdateIntervalms)
            {
                Console.WriteLine($"[Test {testCaseNumber}] Queue size: {searchNodes.Count}, current depth: {node.Depth}, elapsed time: {totalStopwatch.Elapsed.TotalSeconds}");
                debugPrintStopwatch.Restart();
            }


            var newStateVector = node.ApplyWeightsToButtonVectors(buttonVectors);
            if (node.Depth >= minPossiblePushes && newStateVector.Equals(targetStateVector))
            {
                totalStopwatch.Stop();
                Console.WriteLine($"Done. Total elapsed time: {totalStopwatch.Elapsed.TotalSeconds} seconds");
                return node.Depth;
            }

            if (!visitedStateVectors.Contains(newStateVector))
            {
                AddChildSearchNodesToQueue(searchNodes, node.VectorWeights);
                visitedStateVectors.Add(newStateVector);
            }
        }

        Console.WriteLine("Error: unable to find push count");
        return -1;
    }

    private static void AddChildSearchNodesToQueue(
        Queue<JoltageSearchNodeV2> searchNodes,
        int[] vectorWeights)
    {
        for (int i = 0; i < vectorWeights.Length; i++)
        {
            int[] clonedArray = (int[])vectorWeights.Clone();
            clonedArray[i]++;
            var searchNode = new JoltageSearchNodeV2(clonedArray);
            searchNodes.Enqueue(searchNode);
        }
    }

    public class JoltageSearchNodeV2
    {
        public int[] VectorWeights;
        public int Depth;

        public JoltageSearchNodeV2(int[] vectorWeights)
        {
            VectorWeights = vectorWeights;
            Depth = VectorWeights.Sum();
        }

        public JoltageVector ApplyWeightsToButtonVectors(List<JoltageVector> buttonVectors)
        {
            var resultArray = new int[buttonVectors[0].JoltageValues.Length];
            for (int i = 0; i < buttonVectors.Count; i++)
            {
                var buttonVector = buttonVectors[i];
                var weight = VectorWeights[i];
                for (int j = 0; j < buttonVector.JoltageValues.Length; j++)
                {
                    resultArray[j] += buttonVector.JoltageValues[j] * weight;
                }
            }
            return new JoltageVector(resultArray);
        }

        public string DebugString
        {
            get
            {
                var weightString = string.Join(",", VectorWeights);
                return $"Weights: [{weightString}]";
            }
        }
    }

    public struct JoltageVector : IEquatable<JoltageVector>
    {
        public readonly int[] JoltageValues;

        private int _hashCode = 0;

        public JoltageVector(int[] joltageValues)
        {
            JoltageValues = joltageValues;
            _size = joltageValues != null ? joltageValues.Sum() : 0;
            _hashCode = ComputeHashCode();
        }

        private int _size;
        public int Size => _size;

        public override int GetHashCode()
        {
            return _hashCode;
        }

        private int ComputeHashCode()
        {
            var hashCode = new HashCode();
            foreach (var item in JoltageValues)
            {
                hashCode.Add(item);
            }
            return hashCode.ToHashCode();
        }

        public override bool Equals(object? obj) => obj is JoltageRequirements other && Equals(other);

        public bool Equals(JoltageVector other)
        {
            if (JoltageValues == null && other.JoltageValues == null)
            {
                return true;
            }
            else if (JoltageValues == null || other.JoltageValues == null)
            {
                return false;
            }

            if (other.JoltageValues.Count() != JoltageValues.Count())
            {
                return false;
            }

            for (int i = 0; i < JoltageValues.Count(); i++)
            {
                if (other.JoltageValues[i] != JoltageValues[i])
                {
                    return false;
                }
            }

            return true;
        }

        public string DebugString => String.Join(",", JoltageValues);
    }

    #endregion
}
