/*
 * Author: Ewan Robertson
 * Information required to solve a benchmark travelling salesman problem
 * based on the problems from TSPLIB http://comopt.ifi.uni-heidelberg.de/software/TSPLIB95/.
 */

using System.Collections.Generic;

namespace TSPAlgorithm
{
    internal class Problem
    {
        /// <summary>
        /// Name of the problem.
        /// </summary>
        private string _name;

        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Comment from file.
        /// </summary>
        private string _comment;
        /// <summary>
        /// Number of nodes in the problem.
        /// </summary>
        private int _dimension;

        public int Dimension
        {
            get { return _dimension; }
        }
        /// <summary>
        /// Edge weight type from file.
        /// </summary>
        private string _edgeWeightType;
        /// <summary>
        /// Node coordinates.
        /// </summary>
        private (int,int)[] _nodes;
        public (int,int)[] Nodes
        {
            get { return _nodes; }
        }
        /// <summary>
        /// Matrix of distances between nodes.
        /// </summary>
        private double[][] _edgeLengths;
        public double[][] EdgeLengths
        {
            get { return _edgeLengths; }
        }


        /// <summary>
        /// Constructor for initialising Problem instances.
        /// </summary>
        /// <param name="name">Name of the Problem.</param>
        /// <param name="comment">Comment from file.</param>
        /// <param name="dimension">Number of nodes in the problem.</param>
        /// <param name="edgeWeightType">Edge weight type from file.</param>
        /// <param name="nodes">Node coordinates.</param>
        public Problem(string name, string comment, int dimension, string edgeWeightType, (int,int)[] nodes)
        {            
            _name = name;
            _comment = comment;
            _dimension = dimension;
            _edgeWeightType = edgeWeightType;
            _nodes = nodes;

            _edgeLengths = new double[nodes.Length][];
            for(int i=0; i<_nodes.Length; i++)
            {
                _edgeLengths[i] = new double[_nodes.Length];
                for (int j = 0; j < _nodes.Length; j++)
                {
                    _edgeLengths[i][j] = Math.Sqrt(Math.Pow(_nodes[j].Item1 - _nodes[i].Item1, 2) +
                        Math.Pow(_nodes[j].Item2 - _nodes[i].Item2, 2));
                }
            }
        }

        /// <summary>
        /// Returns string representation of the Problem object.
        /// </summary>
        /// <returns>Information about the Problem.</returns>
        public string toString()
        {
            return $"{_name},{_comment},{_dimension},{_edgeWeightType}";
        }
    }
}
