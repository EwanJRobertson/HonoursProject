﻿/*
 * Author: Ewan Robertson
 * Implementation of Evolutionary Algorithm for solving benchmark Travelling
 * Salesman Problems (TSP).
 */

using System.Text.RegularExpressions;

namespace TSPAlgorithm
{
    /// <summary>
    /// Evolutionary Algorithm for TSP.
    /// </summary>
    internal class EvolutionaryAlgorithm : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Population of permutations.
        /// </summary>
        private Permutation[] _population;

        /// <summary>
        /// Number of permutations in the population.
        /// </summary>
        private int _populationSize = 100;

        public int PopulationSize
        {
            get { return _populationSize; }
            set { _populationSize = value; }
        }

        /// <summary>
        /// Tournament size for tournament selection.
        /// </summary>
        private int _tournamentSize = 3;

        /// <summary>
        /// Tournament size for tournament selection.
        /// </summary>
        public int TournamentSize
        {
            get { return _tournamentSize; }
            set { _tournamentSize = value; }
        }

        /// <summary>
        /// Chance for the crossover operator to be applied to two parent 
        /// permutations (0.0-1.0).
        /// </summary>
        private double _crossoverRate = 1.0;

        /// <summary>
        /// Chance for the crossover operator to be applied to two parent 
        /// permutations (0.0-1.0).
        /// </summary>
        public double CrossoverRate
        {
            get { return _crossoverRate; }
            set { _crossoverRate = value; }
        }

        /// <summary>
        /// Chance for the mutation function to be applied to a child 
        /// permutation (0.0-1.0).
        /// </summary>
        private double _mutationRate = 1.0;

        /// <summary>
        /// Chance for the mutation function to be applied to a child 
        /// permutation (0.0-1.0).
        /// </summary>
        public double MutationRate
        {
            get { return _mutationRate; }
            set { _mutationRate = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Algorithm name.</param>
        /// <param name="problem">Problem algorithm to be run on.</param>
        public EvolutionaryAlgorithm(string name, Problem problem) : 
            base(name, problem)
        {
            _population = new Permutation[_populationSize];
        }

        /// <summary>
        /// Initialise population with random, valid permutations.
        /// </summary>
        public void InitPopulation()
        {
            _population = new Permutation[_populationSize];
            for (int i = 0; i < _populationSize; i++)
            {
                _population[i] = new Permutation(Problem);
                while (_population[i].Length < Problem.Dimension)
                {
                    int nextNode = Parameters.random.Next(Problem.Dimension);
                    while (_population[i].Contains(nextNode))
                    {
                        nextNode = Parameters.random.Next(Problem.Dimension);
                    }
                    _population[i].Add(nextNode);
                }
            }
        }

        /// <summary>
        /// Initialise population using the modified Nearest Neighbour algorithm.
        /// </summary>
        public void InitNN()
        {
            _population = new Permutation[_populationSize];
            for (int i = 0; i < _populationSize; i++)
            {
                _population[i] = new Permutation(Problem);
                Permutation newP = new Permutation(Problem);
                newP.Add(Parameters.random.Next(0, Problem.Dimension));
                while (newP.Length != Problem.Dimension)
                {
                    int nextNode = Parameters.random.Next(Problem.Dimension);
                    if (Parameters.random.NextDouble() < 2 / Problem.Dimension)
                    {
                        while (_population[i].Contains(nextNode))
                        {
                            nextNode = Parameters.random.Next(Problem.Dimension);
                        }
                    }
                    else
                    {
                        // get first index not in permutation
                        nextNode = 0;
                        while (newP.Contains(nextNode))
                        {
                            nextNode++;
                        }

                        // get index of nearest neighbour
                        for (int j = 0; j < Problem.Dimension; j++)
                        {
                            if (!newP.Contains(j) &&
                                Problem.EdgeWeights[newP.Last][j] <
                                Problem.EdgeWeights[newP.Last][nextNode])
                            {
                                nextNode = j;
                            }
                        }
                    }
                    // add next node to permutation
                    newP.Add(nextNode);
                }
                _population[i] = newP.Clone();
            }
        }

        /// <summary>
        /// Picks n random members of the population and returns the permutation
        /// with the best fitness.
        /// </summary>
        /// <returns>A member of the population.</returns>
        public Permutation TournamentSelection()
        {
            // select initial permutation
            int bestIndex = Parameters.random.Next(_populationSize);

            // iterate through tournament size - 1 number of random
            // permutations, if selected permutation is more fit then select it
            for (int i = 1; i < _tournamentSize; i++)
            {
                int selected = Parameters.random.Next(_populationSize);
                if (_population[selected].Fitness < _population[bestIndex].Fitness)
                {
                    bestIndex = selected;
                }
            }

            return _population[bestIndex];
        }

        /// <summary>
        /// Picks member of the population based on fitness ranking within the
        /// population.
        /// </summary>
        /// <returns>A member of the population.</returns>
        public Permutation RankedSelection()
        {
            _population = _population.OrderBy(x => x.Fitness).ToArray();
            double[] probabilities = new double[_populationSize];
            double sumProbabilities = 0.0;
            int sum = Enumerable.Range(0, _populationSize).Sum();

            for (int i = 0; i < _populationSize; i++)
            {
                // sum of [0 .. PopulationSize]
                // probability i = (population size - ranked of i)
                // / population size
                probabilities[i] = sumProbabilities;
                sumProbabilities += (double)(_populationSize - i) / 
                    (double)sum;
            }

            double probability = Parameters.random.NextDouble();
            int selectedIndex = 0;
            for (int i = 0; i < _populationSize - 1; i++)
            {
                if (probability >= probabilities[i] && probability < 
                    probabilities[i + 1])
                {
                    selectedIndex = i;
                }
            }

            return _population[selectedIndex];
        }

        /// <summary>
        /// Picks random n. Takes all points from parent1 up to n, then adds 
        /// nodes from parent2. If a node is already present in the child node
        /// then swap index with parent1.
        /// </summary>
        /// <param name="parent1">Parent permutation for crossover.</param>
        /// <param name="parent2">Parent permutation for crossover.</param>
        /// <returns>A new permutation.</returns>
        public Permutation OnePointCrossover(Permutation parent1, 
            Permutation parent2)
        {
            // select random index for crossover point
            int n = Parameters.random.Next(parent1.Length);

            // new child to for return
            Permutation child = new Permutation(Problem);

            // add nodes 0 -> n from parent1
            for (int i = 0; i < n; i++)
            {
                child.Add(parent1.GetNode(i));
            }

            // add nodes n -> length-1 from parent2
            for (int i = n; n < Problem.Dimension; i++)
            {
                child.Add(parent2.GetNode(i));
            }    

            return child;
        }

        /// <summary>
        /// Copies a sequence from parent 1 into a new permutation then adds
        /// the remaining nodes in the order that they appear in parent 2.
        /// </summary>
        /// <param name="parent1">Parent permutation for crossover.</param>
        /// <param name="parent2">Parent permutation for crossover.</param>
        /// <returns>A new permutation.</returns>
        private Permutation OrderedCrossover(Permutation parent1, 
            Permutation parent2)
        {
            if (Parameters.random.NextDouble() > _crossoverRate)
            {
                return parent1;
            }

            int sequenceStart = Parameters.random.Next(Problem.Dimension - 1);
            int sequenceEnd = Parameters.random.Next(sequenceStart, 
                Problem.Dimension);

            Permutation child = new Permutation(Problem);

            List<int> sequence = new List<int>();
            for (int i = sequenceStart; i < sequenceEnd; i++)
            {
                sequence.Add(parent1.GetNode(i));
            }

            int count = 0;
            while (child.Length < sequenceStart)
            {
                if (!sequence.Contains(parent2.GetNode(count)))
                {
                    child.Add(parent2.GetNode(count));
                }
                count++;
            }

            foreach (int i in sequence)
            {
                child.Add(i);
            }

            while (child.Length < Problem.Dimension)
            {
                if (!sequence.Contains(parent2.GetNode(count)))
                {
                    child.Add(parent2.GetNode(count));
                }
                count++;
            }

            return child;
        }

        /// <summary>
        /// Randomly swap two genes in the permutation.
        /// </summary>
        /// <param name="child">Permutation to be mutated.</param>
        /// <returns>Mutatant child.</returns>
        public Permutation Mutate(Permutation child)
        {
            if (Parameters.random.NextDouble() > _mutationRate)
            {
                return child;
            }

            int index1 = Parameters.random.Next(Problem.Dimension);
            int index2 = Parameters.random.Next(Problem.Dimension);

            int swap = child.GetNode(index1);
            child.SetNode(index1, child.GetNode(index2));
            child.SetNode(index2, swap);

            return child;
        }

        /// <summary>
        /// Reverses random sequence in the permutation.
        /// </summary>
        /// <param name="child">Permutation to be mutated.</param>
        /// <returns>Mutatant child.</returns>
        private Permutation ReverseSequenceMutation(Permutation child)
        {
            if (Parameters.random.NextDouble() > _mutationRate)
            {
                return child.Clone();
            }

            Permutation newChild = child.Clone();

            int sequenceStart = Parameters.random.Next(Problem.Dimension - 1);
            int sequenceEnd = Parameters.random.Next(sequenceStart, 
                Problem.Dimension);

            while (sequenceStart < sequenceEnd)
            {
                int temp = newChild.GetNode(sequenceEnd);
                newChild.SetNode(sequenceEnd, child.GetNode(sequenceStart));
                newChild.SetNode(sequenceStart, temp);

                sequenceStart++;
                sequenceEnd--;
            }

            return newChild;
        }

        /// <summary>
        /// Has a chance to swap nodes for every gene in the geneom.
        /// </summary>
        /// <param name="child">Permutation to be mutated.</param>
        /// <returns>Mutatant child.</returns>
        public Permutation PartialShuffleMutation(Permutation child)
        {
            if (Parameters.random.NextDouble() > _mutationRate)
            {
                return child;
            }

            for (int i = 0; i < Problem.Dimension; i++)
            {
                if (Parameters.random.NextDouble() < _mutationRate / 
                    Problem.Dimension )
                {
                    int j = Parameters.random.Next(Problem.Dimension);
                    int temp = child.GetNode(j);
                    child.SetNode(j, child.GetNode(i));
                    child.SetNode(i, temp);
                }
            }

            return child;
        }

        /// <summary>
        /// Revereses sequence in the permutation with a chance to swap with a
        /// random node.
        /// </summary>
        /// <param name="child">Permutation to be mutated.</param>
        /// <returns>Mutatant child.</returns>
        public Permutation HybridMutation(Permutation child)
        {
            if (Parameters.random.NextDouble() > _mutationRate)
            {
                return child;
            }

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
        /// Replace the weakest member of the population if child fitness is 
        /// lower.
        /// </summary>
        /// <param name="child">Prospective permutation.</param>
        public void Replace(Permutation child)
        {
            _population = _population.OrderByDescending(x => x.Fitness).
                ToArray();
            if (_population[0].Fitness > child.Fitness)
            {
                _population[0] = child.Clone();
            }
        }

        /// <summary>
        /// Executes the algorithm on the given problem.
        /// </summary>
        /// <returns>Run results.</returns>
        public override Result Run()
        {
            // best each generation
            string[] bests = new string[Parameters.EvaluationBudget];

            // initialise population
            //InitPopulation();
            InitNN();
            // initialise best
            Best = _population.OrderBy(x => x.Fitness).First();

            // main loop
            Permutation p1, p2, child = new Permutation(Problem);

            for (Evaluations = 0; Evaluations < Parameters.EvaluationBudget; 
                Evaluations++)
            {
                // selection
                //p1 = TournamentSelection();
                //p2 = TournamentSelection();
                p1 = RankedSelection();
                p2 = RankedSelection();

                // crossover
                // child = OnePointCrossover(p1, p2);
                child = OrderedCrossover(p1, p2);

                // mutation
                // child = Mutate(child);
                // child = ReverseSequenceMutation(child);
                // child = PartialShuffleMutation(child);
                child = HybridMutation(child);

                // replacement
                Replace(child);

                // update best
                _population = _population.OrderBy(x => x.Fitness).ToArray();
                if (Best.Fitness > _population[0].Fitness)
                {
                    Best = _population[0].Clone();
                    EvalsForBest = Evaluations;
                }

                // write best solution to console
                Console.WriteLine($"{Evaluations + 1} {Best.Fitness}"); 

                // add generation best to bests
                bests[Evaluations] = Best.Fitness.ToString();
            }

            // write bests to file if required
            if (Parameters.WriteAllBests)
            {
                FileIO.Write(Parameters.FilePathOutput + "EAEvals" +
                    Regex.Replace(DateTime.Now.TimeOfDay.ToString(), ":", ".")
                    + ".csv", bests);
            }

            return Result();
        }
    }
}
