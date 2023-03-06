namespace TSPAlgorithm
{
    internal class AntColonyOptimisation : TravellingSalesmanAlgorithm
    {
        private readonly string _name = "AntColonyOptimsation";

        private Permutation[] _population;
        private double[][] _pheromones;
        private double[] _probabilities;
        private int _populationSize;
        private double _mutationRate = 0.1;
        private double _evaporationFactor = 0.5;
        private int _evaluationBudget = 20000;

        private int alpha = 1;
        private int beta = 5;
        private double c = 1.0;
        private int q = 500;

        private Random random = new Random();

        public AntColonyOptimisation()
        {
            _populationSize = 100;

            _population = new Permutation[_populationSize];
        }

        private void Init(Problem problem)
        {
            /*
            foreach (Permutation ant in _population)
            {
                ant.Add(random.Next(problem.Dimension));
            }
            */
            for (int i = 0; i < _populationSize; i++)
            {
                _population[i] = new Permutation(problem);
                _population[i].Add(random.Next(problem.Dimension));
            }

            _probabilities = new double[problem.Dimension];
            _pheromones = new double[problem.Dimension][];
            for (int i = 0; i < problem.Dimension; i++)
            {
                _pheromones[i] = new double[problem.Dimension];
                for (int j = 0; j < problem.Dimension; j++)
                {
                    _pheromones[i][j] = 1;
                }
            }
        }

        public Permutation Move(Permutation ant)
        {
            // chance for random move
            if (random.NextDouble() < _mutationRate)
            {
                int randomNode = random.Next(ant.Problem.Dimension);
                while (ant.Contains(randomNode))
                {
                    randomNode = random.Next(ant.Problem.Dimension);
                }
                return ant;
            }

            //
            int currentIndex = ant.Last;
            double pheromone = 0.0;
            for (int i = 0; i < ant.Problem.Dimension; i++)
            {
                if (!ant.Contains(i))
                {
                    pheromone += Math.Pow(_pheromones[ant.Last][i], alpha) *
                        Math.Pow(1.0 / ant.Problem.EdgeLengths[ant.Last][i], beta);
                }
            }

            for (int i = 0; i < ant.Problem.Dimension; i++)
            {
                if (ant.Contains(i))
                {
                    _probabilities[i] = 0.0;
                }
                else
                {
                    double numerator = Math.Pow(_pheromones[ant.Last][i], alpha) *
                        Math.Pow(1.0 / ant.Problem.EdgeLengths[ant.Last][i], beta);
                    _probabilities[i] = numerator / pheromone;
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
                    return ant;
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
                _pheromones[ant.Nodes[ant.Length - 1]] [ant.Nodes[0]] += contribution;
            }
        }

        public Result Run(Problem problem)
        {
            Init(problem);

            for (int i = 0; i < _evaluationBudget; i++)
            {
                for (int j = 0; j < _populationSize; j++)
                {
                    for (int k = 0; k < problem.Dimension; k++)
                    {
                        _population[k] = Move(_population[k]);
                    }
                }

                UpdatePheromones(problem);
            }

            return ResultFactory.FactoryMethod(problem.Name, "ACO", _population.OrderBy(x => x.Fitness).ElementAt(0).Fitness,
                _population.OrderBy(x => x.Fitness).ElementAt(0).Path(), 20000);
        }
    }
}
