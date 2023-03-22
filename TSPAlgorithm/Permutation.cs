/*
 * Author: Ewan Robertson
 * Permuation of a Hamiltonian Cycle for a benchmark travelling salseman 
 * problem.
 */

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
        /// <param name="problem">Reference to the problem the permuation is an
        /// answer to.</param>
        public Permutation(Problem problem)
        {
            _nodes = new List<int>();
            _problem = problem;
            _fitness = int.MaxValue;
        }

        /// <summary>
        /// The Hamiltonian Cycle distance from node[0] -> .. -> node [n] -> 
        /// node[0].
        /// </summary>
        private double _fitness;

        /// <summary>
        /// Get Fitness.
        /// </summary>
        public double Fitness
        {
            get { if (_fitness == int.MaxValue) { FitnessFunction(); } 
                return _fitness; }
            set { _fitness = value; }
        }

        /// <summary>
        /// Calculates the fitness of the solution as the distance around the 
        /// nodes, starting and ending at the same node.
        /// </summary>
        /// <returns>The distnace around the nodes, starting and finishing at 
        /// the same node.</returns>
        public void FitnessFunction()
        {
            // check solution is valid
            if (Length != Problem.Dimension || Length != 
                _nodes.Distinct().Count())
            {
                _fitness = int.MaxValue;
                return;
            }    

            // calculate fitness
            _fitness = 0;
            for (int i = 0; i < Length - 1; i++)
            {
                _fitness += _problem.EdgeLengths[_nodes[i]][_nodes[i + 1]];
            }
            _fitness += _problem.EdgeLengths[_nodes[Length - 1]][_nodes[0]];
            _fitness = Math.Round(_fitness);
        }

        /// <summary>
        /// Adds a node to nodes.
        /// </summary>
        /// <param name="next">Next node to be added.</param>
        public void Add(int next)
        {
            if (!_nodes.Contains(next))
            {
                _nodes.Add(next);
            }
        }

        /// <summary>
        /// Gets value of node at specified index.
        /// </summary>
        /// <param name="index">Index of the value to be returned.</param>
        /// <returns>Node value at specified index.</returns>
        public int GetNode(int index)
        {
            return _nodes[index];
        }

        /// <summary>
        /// Sets specified node to value. Resets fitness.
        /// </summary>
        /// <param name="index">Index of value to be set.</param>
        /// <param name="value">New value.</param>
        public void SetNode(int index, int value)
        {
            _nodes[index] = value;
            _fitness = int.MaxValue;
        }

        /// <summary>
        /// Returns the index of value in nodes, otherwise returns -1.
        /// </summary>
        /// <param name="value">Search value.</param>
        /// <returns>Index of value.</returns>
        public int GetIndex(int value)
        {
            int i = 0;
            foreach (int t in _nodes)
            {
                if (value == t)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Gets the length of the permutation.
        /// </summary>
        public int Length
        {
            get { return _nodes.Count; }
        }

        /// <summary>
        /// Checks whether value is in permutation.
        /// </summary>
        /// <param name="search">Search value.</param>
        /// <returns>True: value is in permutationm False: value is not in 
        /// permutation.</returns>
        public bool Contains(int search)
        {
            return _nodes.Contains(search);
        }

        /// <summary>
        /// Returns the value of the last node in the permutation.
        /// </summary>
        public int Last
        {
            get { return _nodes.Last(); }
        }

        /// <summary>
        /// Returns a copy of the nodes of the permutation.
        /// </summary>
        /// <returns>A copy of the nodes of the permutation.</returns>
        public List<int> GetAllNodes()
        {
            return new List<int>(_nodes);
        }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <returns>String representation of the path.</returns>
        public string Path()
        {
            string returnStr = "";
            foreach (int i in _nodes)
            {
                returnStr += ($"{i},");
            }
            return returnStr.Substring(0, returnStr.Length - 1);
        }

        /// <summary>
        /// Makes new object with same permutation and fitness as the specified
        /// permutation object.
        /// </summary>
        /// <returns></returns>
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
