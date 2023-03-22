/*
 * Author: Ewan Robertson
 * Implementation of Simulated Annealing Algorithm for solving benchmark 
 * Travelling Salesman Problems (TSP).
 */

namespace TSPAlgorithm
{
    /// <summary>
    /// Simulated Annealing (SA) algorithm for TSP.
    /// </summary>
    internal class SimulatedAnnealing : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Starting temperature for SA run.
        /// </summary>
        private double _initialTemperature = 2000;

        /// <summary>
        /// Starting temperature for SA run.
        /// </summary>
        public double InitialTemperature
        {
            get { return _initialTemperature; }
            set { _initialTemperature = value; }
        }

        /// <summary>
        /// Rate at which temperature decreases at each generation.
        /// </summary>
        private double _coolingRate = 0.25;

        /// <summary>
        /// Rate at which temperature decreases at each generation.
        /// </summary>
        public double CoolingRate
        { 
            get { return _coolingRate; } 
            set { _coolingRate = value; } 
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Algorithm name.</param>
        /// <param name="problem">Problem algorithm to be run on.</param>
        public SimulatedAnnealing(string name, Problem problem) : 
            base(name, problem)
        { 
        }

        /// <summary>
        /// Initialises new, valid, random permutation.
        /// </summary>
        /// <returns>Valid, random permutation for TSP.</returns>
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

        /// <summary>
        /// Swaps two random nodes.
        /// </summary>
        /// <param name="child">Permutation to be mutated.</param>
        /// <returns>Copy of input permutation with 2 nodes swapped (and 
        /// fitness reset).</returns>
        private Permutation Mutate(Permutation child)
        {
            int index1 = Parameters.random.Next(Problem.Dimension);
            int index2 = Parameters.random.Next(Problem.Dimension);

            int swap = child.GetNode(index1);
            child.SetNode(index1, child.GetNode(index2));
            child.SetNode(index2, swap);

            return child;
        }

        /// <summary>
        /// Reverses a randomly selected sequence of nodes in a permutation.
        /// </summary>
        /// <param name="cycle">Permutation to be mutated.</param>
        /// <returns>Copy of input permutation with a sequence of nodes 
        /// reversed (and fitness reset).</returns>
        private Permutation ReverseSequenceMutation(Permutation cycle)
        {
            int sequenceStart = Parameters.random.Next(Problem.Dimension - 1);
            int sequenceEnd = Parameters.random.Next(sequenceStart, 
                Problem.Dimension);

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

        /// <summary>
        /// Reverses a randomly selected sequence of nodes in a permutation 
        /// and has a chance to swap nodes. Is a hybrid of Reverse Sequence
        /// Mutation and Partial Shuffle Mutation.
        /// </summary>
        /// <param name="cycle">Permutation to be mutated.</param>
        /// <returns>Copy of input permutation with a sequence of nodes 
        /// reversed and potentially some swapped nodes (and fitness reset).
        /// </returns>
        private Permutation HybridMutation(Permutation child)
        {
            int sequenceStart = Parameters.random.Next(Problem.Dimension - 1);
            int sequenceEnd = Parameters.random.Next(sequenceStart, 
                Problem.Dimension);

            while (sequenceStart < sequenceEnd)
            {
                int temp = child.GetNode(sequenceEnd);
                child.SetNode(sequenceEnd, child.GetNode(sequenceStart));
                child.SetNode(sequenceStart, temp);
                if (Parameters.random.NextDouble() < 0.01)
                {
                    int pos = Parameters.random.Next(Problem.Dimension);
                    temp = child.GetNode(pos);
                    child.SetNode(pos, child.GetNode(sequenceStart));
                    child.SetNode(sequenceStart, temp);
                }
                sequenceStart++;
                sequenceEnd--;
            }

            return child;
        }

        /// <summary>
        /// Executes the algorithm on the given problem.
        /// </summary>
        /// <returns>Run results.</returns>
        public override Result Run()
        {
            // best each generation
            string[] bests = new string[Parameters.EvaluationBudget];

            // initialise permutations
            Permutation workingCycle = NewRandomCycle();
            Permutation localBestCycle = workingCycle.Clone();
            Best = workingCycle.Clone();

            // initialise temperature
            double currentTemperature = _initialTemperature;

            // for (Evaluations = 0; Evaluations < Parameters.EvaluationBudget; Evaluations++)
            for (Evaluations = 0; Evaluations < Parameters.EvaluationBudget; Evaluations++)
            {
                // get new cycle
                workingCycle = HybridMutation(localBestCycle.Clone());

                // update bests
                if ((workingCycle.Fitness <= localBestCycle.Fitness) ||
                        (Parameters.random.NextDouble() <= Math.Exp(
                            -(double)(workingCycle.Fitness - localBestCycle.Fitness) 
                            / (double)currentTemperature)))
                {
                    localBestCycle = workingCycle.Clone();
                }

                if (localBestCycle.Fitness < Best.Fitness)
                {
                    Best = localBestCycle.Clone();
                }

                // reduce temperature
                currentTemperature *= _coolingRate;

                // write best to console
                Console.WriteLine($"{Evaluations + 1} {workingCycle.Fitness}");

                // add generation best to bests
                bests[Evaluations] = Best.Fitness.ToString();
            }

            // write bests to file if required
            if (Parameters.WriteAllBests)
            {
                FileIO.Write(Parameters.FilePath + "SAEvals" + DateTime.Now + ".csv", bests);
            }

            return Result();
        }
    }
}
