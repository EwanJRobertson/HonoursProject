/*
 * Author: Ewan Robertson
 * Information required to solve a benchmark travelling salesman problem
 * based on the problems from TSPLIB http://comopt.ifi.uni-heidelberg.de/software/TSPLIB95/.
 */

using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        private string _edgeWeightFormat;

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
        public Problem(string name, string comment, int dimension, string edgeWeightType, string edgeWeightFormat, double[][] edgeWeights)
        {
            _name = name;
            _comment = comment;
            _dimension = dimension;
            _edgeWeightType = edgeWeightType;
            _edgeWeightFormat = edgeWeightFormat;
            _edgeLengths = edgeWeights;
        }

        /// <summary>
        /// Returns string representation of the Problem object.
        /// </summary>
        /// <returns>Information about the Problem.</returns>
        public override string ToString()
        {
            return $"{_name},{_comment},{_dimension},{_edgeWeightType}";
        }
    }
}