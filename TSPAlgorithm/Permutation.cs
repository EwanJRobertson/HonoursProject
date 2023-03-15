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
            _fitness = int.MaxValue;
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
            get { if (_fitness == int.MaxValue) { FitnessFunction(); } return _fitness; }
            // get { FitnessFunction(); return _fitness; }
            set { _fitness = value; }
        }

        /// <summary>
        /// Calculates the fitness of the solution as the distance around the nodes,
        /// starting and ending at the same node.
        /// </summary>
        /// <returns>The distnace around the nodes, starting and finishing at 
        /// the same node.</returns>
        public void FitnessFunction()
        {
            if (Length != Problem.Dimension || Length != _nodes.Distinct().Count())
            {
                _fitness = int.MaxValue;
                return;
            }    

            _fitness = 0;
            for (int i = 0; i < Length - 1; i++)
            {
                _fitness += _problem.EdgeLengths[_nodes[i]][_nodes[i + 1]];
            }
            _fitness += _problem.EdgeLengths[_nodes[Length - 1]][_nodes[0]];
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

        public int GetNode(int index)
        {
            return _nodes[index];
        }

        public void SetNode(int index, int value)
        {
            _nodes[index] = value;
            _fitness = int.MaxValue;
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

        public Permutation Clone()
        {
            Permutation clone = new Permutation(Problem);
            foreach (int i in _nodes)
            {
                clone.Add(i);
            }
            clone.Fitness = _fitness;

            return clone;
        }
    }
}
