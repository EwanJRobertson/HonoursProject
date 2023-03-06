using System.Linq;
using System.Collections.Immutable;

namespace TSPAlgorithm
{
    internal class ACO : TravellingSalesmanAlgorithm
    {
        private readonly string _name = "AntColonyOptimsation";

        private Permutation[] _population;
        private double[][] _pheromones;
        private double[] _probabilities;
        private int _populationSize;
        private double _mutationRate = 0.01;
        private double _evaporationFactor = 0.8;
        private int _evaluationBudget = 200;

        private int alpha = 1;
        private int beta = 8;
        private double c = 1.0;
        private int q = 200;

        private Random random = new Random();

        public ACO()
        {
            _populationSize = 100;

            _population = new Permutation[_populationSize];
        }

        private void Init(Problem problem)
        {
            for (int i = 0; i < _populationSize; i++)
            {
                _population[i] = new Permutation(problem);
            }

            _probabilities = new double[problem.Dimension];
            _pheromones = new double[problem.Dimension][];
            for (int i = 0; i < problem.Dimension; i++)
            {
                _pheromones[i] = new double[problem.Dimension];
                for (int j = 0; j < problem.Dimension; j++)
                {
                    _pheromones[i][j] = c;
                }
            }
        }

        private Permutation Clear(Permutation ant)
        {
            ant.Nodes.Clear();
            ant.Add(GetRandomNode(ant.Problem.Dimension));
            return ant;
        }

        private int GetRandomNode(int dimension)
        {
            return random.Next(dimension);
        }

        public Permutation Move(Permutation a)
        {
            Permutation ant = new Permutation(a.Problem);
            ant.Add(GetRandomNode(a.Problem.Dimension));
            while (ant.Length != ant.Problem.Dimension)
            {
                // chance for random move
                if (random.NextDouble() < _mutationRate)
                {
                    int randomNode = GetRandomNode(ant.Problem.Dimension);
                    while (ant.Contains(randomNode))
                    {
                        randomNode = GetRandomNode(ant.Problem.Dimension);
                    }
                    ant.Add(randomNode);
                }
                else
                {
                    double denominator = 0.0;
                    for (int i = 0; i < ant.Problem.Dimension; i++)
                    {
                        if (ant.Contains(i))
                        {
                            continue;
                        }
                        double pheromone = Math.Pow(_pheromones[ant.Last][0], alpha);
                        double distanceValue = Math.Pow(1.0 / ant.Problem.EdgeLengths[ant.Last][i], beta);
                        denominator += pheromone * distanceValue;
                    }

                    for (int i = 0; i < ant.Problem.Dimension; i++)
                    {
                        if (ant.Contains(i))
                        {
                            _probabilities[i] = 0;
                        }
                        else
                        {
                            double numerator = Math.Pow(_pheromones[ant.Last][i], alpha) *
                                Math.Pow(1.0 / ant.Problem.EdgeLengths[ant.Last][i], beta);
                            _probabilities[i] += numerator / denominator;
                        }
                    }
                    double r = random.NextDouble();
                    double total = 0.0;
                    for (int i = 0; i < ant.Problem.Dimension; i++)
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

        public void UpdatePheromones(Problem problem)
        {
            for (int i = 0; i < problem.Dimension; i++)
            {
                for (int j = 0; j < problem.Dimension; j++)
                {
                    _pheromones[i][j] *= _evaporationFactor;
                }
            }

            foreach (Permutation ant in _population)
            {
                double contribution = q / ant.Length;
                for (int i = 0; i < problem.Dimension - 1; i++)
                {
                    _pheromones[ant.Nodes[i]][ant.Nodes[i + 1]] += contribution;
                }
                _pheromones[ant.Nodes[ant.Length - 1]][ant.Nodes[0]] += contribution;
            }
        }

        public Result Run(Problem problem)
        {
            Init(problem);

            Permutation best = new Permutation(problem);

            for (int i = 0; i < _evaluationBudget; i++)
            {
                // _population = new Permutation[_populationSize];
                for (int j = 0; j < _populationSize; j++)
                {
                    _population[j] = Move(_population[j]);
                }

                Array.Sort(_population, delegate (Permutation x, Permutation y) { return x.Fitness.CompareTo(y.Fitness); });

                if (best.Fitness == -1 || _population[0].Fitness < best.Fitness)
                {
                    best = _population[0];
                }

                UpdatePheromones(problem);
                Console.WriteLine($"{i} {_population[0].Fitness}");
            }

            return ResultFactory.FactoryMethod(problem.Name, "ACO", best.Fitness,
                best.Path(), 20000);
        }
    }
}
