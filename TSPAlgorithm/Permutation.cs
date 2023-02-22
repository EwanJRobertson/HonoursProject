/*
 * Author: Ewan Robertson
 * Result for single execution of an algorithm on a
 * benchmark travelling salseman problem.
 */

using System.Collections.Generic;

namespace TSPAlgorithm
{
    internal class Permutation
    {
        /// <summary>
        /// Ordered list of nodes visited in the Hamiltonian Cycle.
        /// </summary>
        private List<int> _nodes;

        public List<int> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// The Hamiltonian Cycle distance from node[0] -> .. -> node [n] -> node[0].
        /// </summary>
        private double _fitness;

        /// <summary>
        /// Get Fitness.
        /// </summary>
        public double Fitness
        { 
            get { return _fitness; }
        }
    }
}
