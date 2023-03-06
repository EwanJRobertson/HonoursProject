/*
 * Author: Ewan Robertson
 * Permuation of a Hamiltonian Cycle for a benchmark travelling salseman problem.
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

        /// <summary>
        /// Getter for permutation nodes.
        /// </summary>
        public List<int> Nodes
        {
            get { return _nodes; }
        }

        /// <summary>
        /// A reference to the problem which the permutation is an answer to.
        /// </summary>
        private Problem _problem;

        /// <summary>
        /// Gets Problem.
        /// </summary>
        public Problem Problem
        {
            get { return _problem; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="problem">Reference to the problem the permuation is an answer to.</param>
        public Permutation(Problem problem)
        {
            _nodes = new List<int>();
            _problem = problem;
            _fitness = -1;
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
            get { if (_fitness < 0) { FitnessFunction(); } return _fitness; }
        }

        /// <summary>
        /// Calculates the fitness of the solution as the distance around the nodes,
        /// starting and ending at the same node.
        /// </summary>
        /// <returns>The distnace around the nodes, starting and finishing at 
        /// the same node.</returns>
        private void FitnessFunction()
        {
            if (Length == 0)
                return;
            _fitness = 0;
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                _fitness += _problem.EdgeLengths[Nodes[i]][Nodes[i + 1]];
            }
            _fitness += _problem.EdgeLengths[Nodes[Nodes.Count - 1]][Nodes[0]];
            _fitness = Math.Round(_fitness);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        public void Add(int next)
        {
            if (!_nodes.Contains(next))
            {
                _nodes.Add(next);
            }
        }

        public int Length
        {
            get { return _nodes.Count; }
        }

        public bool Contains(int search)
        {
            return _nodes.Contains(search);
        }

        public int Last
        {
            get { return _nodes.Last(); }
        }

        public string Path()
        {
            string returnStr = "";
            foreach (int i in _nodes)
            {
                returnStr += ($"{i},");
            }
            return returnStr.Substring(0, returnStr.Length - 1);
        }
    }
}
