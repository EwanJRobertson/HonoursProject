/*
 * Author: Ewan Robertson
 */

namespace TSPAlgorithm
{
    internal class EvolutionaryAlgorithm : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Maximum number of fitness function calls allowed before exiting.
        /// </summary>
        int _evaluationBudget;

        /// <summary>
        /// Population of permutations.
        /// </summary>
        private Permutation[] _population;

        /// <summary>
        /// Number of permutations in the population.
        /// </summary>
        private int _populationSize;

        /// <summary>
        /// Tournament selection, tournament size.
        /// </summary>
        private int _tournamentSize;

        /// <summary>
        /// Chance for the mutation function to be applied to a child permutation.
        /// </summary>
        double _mutationRate;

        /// <summary>
        /// Instance of Random class.
        /// </summary>
        private Random random = new Random();

        private Parameters parameters = new Parameters();

        /// <summary>
        /// Constructor.
        /// </summary>
        public EvolutionaryAlgorithm()
        {
            _populationSize = parameters.PopulationSize;
            _tournamentSize = parameters.TournamentSize;
            _mutationRate = parameters.MutationRate;
            _population = new Permutation[_populationSize];

            _evaluationBudget = parameters.EvaluationBudget;
        }

        /// <summary>
        /// Gets the fitness value for the permutation with the best fitness in the
        /// population.
        /// </summary>
        /// <returns>The best fitness value in the population.</returns>
        public double BestFitness()
        {
            return _population.OrderBy(x => x.Fitness).ElementAt(0).Fitness;
        }

        /// <summary>
        /// Picks n random members of the population and returns the permutation with
        /// the best fitness.
        /// </summary>
        /// <returns>A random(astrix) member of the population.</returns>
        public Permutation TournamentSelection()
        {
            // select initial permutation
            int bestIndex = random.Next(0, _populationSize);

            // iterate through tournament size - 1 number of random permutations, if
            // selected permutation is more fit then select it
            for (int i = 1; i < _tournamentSize; i++)
            {
                int selected = random.Next(_populationSize);
                if (_population[selected].Fitness < _population[bestIndex].Fitness)
                {
                    bestIndex = selected;
                }
            }

            return _population[bestIndex];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent1"></param>
        /// <param name="parent2"></param>
        /// <returns></returns>
        public Permutation OnePointCrossover(Permutation parent1, Permutation parent2)
        {
            int crossoverIndex = random.Next(parent1.Length);

            Permutation child = new Permutation(parent1.Problem);

            for (int i = 0; i < parent1.Length; i++)
            {
                child.Add(parent1.Nodes[i]);
            }

            for (int i = crossoverIndex; i < child.Nodes.Count; i++)
            {
                // index of the new value in child
                int swapIndex = child.Nodes.FindIndex(x => parent2.Nodes[i] == parent2.Nodes[i]);

                // set temp to oldval
                // set i to newval
                // set index to temp
                int swapVal = child.Nodes[i];
                child.Nodes[i] = parent2.Nodes[i];
                child.Nodes[swapIndex] = swapVal;
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
            if (random.NextDouble() < _mutationRate)
            {
                int swapPos1 = random.Next(child.Nodes.Count);
                int swapVal1 = child.Nodes[swapPos1];

                int swapPos2 = random.Next(child.Nodes.Count);
                int swapVal2 = child.Nodes[swapPos2];

                child.Nodes[swapPos2] = swapVal1;
                child.Nodes[swapPos1] = swapVal2;
            }
            
            return child;
        }

        public void Replace(Permutation child)
        {
            _population.OrderBy(x => x.Fitness);
            if (_population[0].Fitness < child.Fitness)
            {
                _population[0] = child;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problem"></param>
        public Result Run(Problem problem)
        {
            Permutation p1, p2, child = new Permutation(problem);
            while (_evaluationBudget > 0)
            {
                p1 = TournamentSelection();
                p2 = TournamentSelection();

                child = OnePointCrossover(p1, p2);
                child = Mutate(child);
                Replace(child);
            }
            return null;
        }
    }
}
