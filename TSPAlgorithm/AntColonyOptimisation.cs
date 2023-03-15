﻿/*
 * Author: Ewan Robertson
 * Implementation of Ant Colony Opimisation algorithm for solving benchmark
 * Travelling Salesman Problems (TSP).
 */

namespace TSPAlgorithm
{
    /// <summary>
    /// Ant Colony Optimisation algorithm for TSP.
    /// </summary>
    internal class AntColonyOptimisation : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Population of permutations (ants).
        /// </summary>
        private Permutation[] _population;

        /// <summary>
        /// Matrix of pheromones for each edge in the graph.
        /// </summary>
        private double[][] _pheromones;

        /// <summary>
        /// Probability to move from current node to node numbered index.
        /// </summary>
        private double[] _probabilities;
        
        /// <summary>
        /// Size of the population.
        /// </summary>
        private readonly int _populationSize = 100;

        /// <summary>
        /// Chance for a random move (not affected by pheromones). 0-1.
        /// </summary>
        private readonly double _mutationRate = 0.01;

        /// <summary>
        /// Factor to multiple pheromones by after entire population has moved. 0-1.
        /// </summary>
        private readonly double _evaporationFactor = 0.5;

        /// <summary>
        /// Pheromone importance factor.
        /// </summary>
        private readonly int alpha = 1;
        
        /// <summary>
        /// Distance importance factor.
        /// </summary>
        private readonly int beta = 8;

        /// <summary>
        /// Starting pheromone values.
        /// </summary>
        private readonly double c = 1.0;

        /// <summary>
        /// Total number of pheromones left by each ant.
        /// </summary>
        private readonly int q = 500;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Algorithm name.</param>
        /// <param name="problem">Problem algorithm to be run on.</param>
        public AntColonyOptimisation(string name, Problem problem) : base(name, problem)
        {
            _population = new Permutation[_populationSize];
            _probabilities = new double[Problem.Dimension];
            _pheromones = new double[Problem.Dimension][];
        }

        /// <summary>
        /// Intialise arrays.
        /// </summary>
        private void Init()
        {
            _probabilities = new double[Problem.Dimension];

            // set all pheromones to initial value c
            _pheromones = new double[Problem.Dimension][];
            for (int i = 0; i < Problem.Dimension; i++)
            {
                _pheromones[i] = new double[Problem.Dimension];
                for (int j = 0; j < Problem.Dimension; j++)
                {
                    _pheromones[i][j] = c;
                }
            }
        }

        /// <summary>
        /// Ant tour. Moves ants from a random starting node round a full tour.
        /// </summary>
        /// <returns>A Hamiltonian cycle round the nodes.</returns>
        public Permutation Move()
        {
            // return ant
            Permutation ant = new Permutation(Problem);
            // add random initial node
            ant.Add(Parameters.random.Next(Problem.Dimension));

            // move ant
            while (ant.Length != Problem.Dimension)
            {
                // chance to move to a random node
                if (Parameters.random.NextDouble() < _mutationRate)
                {
                    int randomNode = Parameters.random.Next(Problem.Dimension);
                    while (ant.Contains(randomNode))
                    {
                        randomNode = Parameters.random.Next(Problem.Dimension);
                    }
                    ant.Add(randomNode);
                }
                else    // move based on pheromones and distance
                {
                    // calculate denominator for probability
                    // sum (pheromone to i)^alpha * (distance to i)^beta
                    double denominator = 0.0;
                    for (int i = 0; i < Problem.Dimension; i++)
                    {
                        if (!ant.Contains(i))
                        {
                            double pheromone = Math.Pow(_pheromones[ant.Last][i], alpha);
                            double distanceValue = Math.Pow(1.0 / Problem.EdgeLengths[ant.Last][i], beta);
                            denominator += pheromone * distanceValue;
                        }
                    }

                    // calculate probability for ant to move to each node
                    for (int i = 0; i < Problem.Dimension; i++)
                    {
                        // if ant has already visited node then probability is zero
                        if (ant.Contains(i))
                        {
                            _probabilities[i] = 0;
                        }
                        else
                        {
                            double numerator = Math.Pow(_pheromones[ant.Last][i], alpha) *
                                Math.Pow(1.0 / Problem.EdgeLengths[ant.Last][i], beta);

                            _probabilities[i] = numerator / denominator;
                        }
                    }
                    // move to next city based on probabilities
                    double r = Parameters.random.NextDouble();
                    double total = 0.0;
                    for (int i = 0; i < Problem.Dimension; i++)
                    {
                        total += _probabilities[i];
                        if (total >= r)
                        {
                            ant.Add(i);
                            break;
                        }
                    }
                }
            }
            return ant;
        }

        /// <summary>
        /// Update the pheromones matrix with new ant paths.
        /// </summary>
        public void UpdatePheromones()
        {
            // reduce pheromone trails by multiplying evaporation factor
            for (int i = 0; i < Problem.Dimension; i++)
            {
                for (int j = 0; j < Problem.Dimension; j++)
                {
                    _pheromones[i][j] *= _evaporationFactor;
                }
            }

            // for each ant distribute contribution accross path
            foreach (Permutation ant in _population)
            {
                double contribution = q / ant.Length;
                for (int i = 0; i < Problem.Dimension - 1; i++)
                {
                    _pheromones[ant.GetNode(i)][ant.GetNode(i + 1)] += contribution;
                }
                _pheromones[ant.GetNode(ant.Length - 1)][ant.GetNode(0)] += contribution;
            }
        }

        /// <summary>
        /// Implementation of TSP class run.
        /// </summary>
        /// <returns>Result object containing information on the run.</returns>
        public override Result Run()
        {
            // save best result found
            Best = new Permutation(Problem);

            // initialise algorithm
            Init();

            // main loop
            for (int Evaluations = 0; Evaluations < Parameters.EvaluationBudget; Evaluations += _populationSize)
            {
                // move each ant around a tour
                for (int j = 0; j < _populationSize; j++)
                {
                    _population[j] = Move();
                }

                // order population by fitness
                _population = _population.OrderBy(x => x.Fitness).ToArray();

                // update best solution found
                if (_population[0].Fitness < Best.Fitness)
                {
                    Best = _population[0].Clone();
                }

                // update pheromones
                UpdatePheromones();

                // write best solution to console
                Console.WriteLine($"{Evaluations + 1} {Best.Fitness}");
            }

            // return result
            return Result();
        }
    }
}