/*
 * Author: Ewan Robertson
 * Abstract Class for the implementation of Algorithms for solving
 * benchmark Travelling Salesman Problems.
 */

using System.Xml.Linq;

namespace TSPAlgorithm
{
    internal abstract class TravellingSalesmanAlgorithm
    {
        private string _name;
        private int _evalBudget;

        public double getFitness(Problem problem, Permutation permutation)
        {
            double fitness = 0;
            for (int i = 1; i < permutation.Nodes.Count; i++)
            {
                fitness += problem.EdgeLengths[permutation.Nodes.ElementAt(i)][permutation.Nodes.ElementAt(i - 1)];
            }
            fitness += problem.EdgeLengths[permutation.Nodes.ElementAt(permutation.Nodes.Count)][permutation.Nodes.ElementAt(0)];
            return fitness;
        }
    }
}
