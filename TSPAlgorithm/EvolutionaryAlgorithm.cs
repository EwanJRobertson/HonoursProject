using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPAlgorithm
{
    internal class EvolutionaryAlgorithm : TravellingSalesmanAlgorithm
    {
        private Permutation[] _population;
        private int _populationSize;
        private int _tournamentSize;
        double _mutationRate;

        int _evalBudget;

        private Random random = new Random();

        private Parameters parameters = new Parameters();

        public EvolutionaryAlgorithm()
        {
            _populationSize = parameters.PopulationSize;
            _tournamentSize = parameters.TournamentSize;
            _mutationRate = parameters.MutationRate;
            _population = new Permutation[_populationSize];

            _evalBudget = parameters.EvaluationBudget;
        }

        public double BestFitness()
        {
            return _population.OrderBy(x => x.Fitness).ElementAt(0).Fitness;
        }

        public Permutation TournamentSelection()
        {
            int bestIndex = random.Next(0, _populationSize);

            for (int i = 1; i < _tournamentSize; i++)
            {
                int selected = random.Next(0, _populationSize);
                if (_population[selected].Fitness < _population[bestIndex].Fitness)
                {
                    bestIndex = selected;
                }
            }
            return _population[bestIndex];
        }

        /*
        public Permutation OnePointCrossover(Permutation parent1, Permutation parent2)
        {
            int crossoverIndex = random.Next(parent1.Nodes.Count);

            Permutation child = new Permutation();

            for (int i = 0; i < parent1.Nodes.Count; i++)
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
        */

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

        /*
        public void Run()
        {
            Permutation p1, p2, child = new Permutation();
            while (_evalBudget > 0)
            {
                p1 = TournamentSelection();
                p2 = TournamentSelection();

                child = OnePointCrossover(p1, p2);
                child = Mutate(child);
                Replace(child);
            }
        }
        */
    }
}
