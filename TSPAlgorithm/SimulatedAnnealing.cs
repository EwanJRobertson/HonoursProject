namespace TSPAlgorithm
{
    internal class SimulatedAnnealing : TravellingSalesmanAlgorithm
    {
        private readonly double _initialTemperature = 2000;

        private readonly double _coolingRate = 0.005;

        public SimulatedAnnealing(string name, Problem problem) : base(name, problem)
        { }

        private Permutation NewRandomCycle()
        {
            Permutation cycle = new Permutation(Problem);
            while (cycle.Length < Problem.Dimension)
            {
                int nextNode = Parameters.random.Next(Problem.Dimension);
                while (cycle.Contains(nextNode))
                {
                    nextNode = Parameters.random.Next(Problem.Dimension);
                }
                cycle.Add(nextNode);
            }
            return cycle;
        }

        public Permutation Mutate(Permutation child)
        {
            int index1 = Parameters.random.Next(Problem.Dimension);
            int index2 = Parameters.random.Next(Problem.Dimension);

            int swap = child.GetNode(index1);
            child.SetNode(index1, child.GetNode(index2));
            child.SetNode(index2, swap);

            return child;
        }

        private Permutation ReverseSequenceMutation(Permutation cycle)
        {
            int sequenceStart = Parameters.random.Next(Problem.Dimension - 1);
            int sequenceEnd = Parameters.random.Next(sequenceStart, Problem.Dimension);

            List<int> newSequence = new List<int>();

            for (int i = sequenceStart; i < sequenceEnd; i++)
            {
                newSequence.Add(cycle.GetNode(i));
            }

            for (int i = 0; i < newSequence.Count; i++)
            {
                cycle.SetNode(sequenceEnd - 1 - i, newSequence[i]);
            }

            return cycle;
        }

        public override Result Run()
        {
            Permutation workingCycle = NewRandomCycle();
            Permutation localBestCycle = workingCycle.Clone();
            Best = workingCycle.Clone();

            double currentTemperature = _initialTemperature;

            for (Evaluations = 0; Evaluations < Parameters.EvaluationBudget; Evaluations++)
            {
                workingCycle = ReverseSequenceMutation(workingCycle);
                workingCycle.FitnessFunction();

                if ((workingCycle.Fitness < localBestCycle.Fitness) ||
                        (Parameters.random.NextDouble() > Math.Exp(-(localBestCycle.Fitness - workingCycle.Fitness) / currentTemperature)))
                {
                    localBestCycle = workingCycle.Clone();
                }

                if (localBestCycle.Fitness < Best.Fitness)
                {
                    Best = localBestCycle.Clone();
                }

                currentTemperature *= _coolingRate;
                Console.WriteLine($"{Evaluations + 1} {Best.Fitness}");
            }

            return Result();
        }
    }
}
