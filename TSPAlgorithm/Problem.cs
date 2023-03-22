/*
 * Author: Ewan Robertson
 * Information required to solve a benchmark travelling salesman problem
 * based on the problems from TSPLIB 
 * http://comopt.ifi.uni-heidelberg.de/software/TSPLIB95/.
 */

namespace TSPAlgorithm
{
    /// <summary>
    /// Problem instance of TSP.
    /// </summary>
    internal class Problem
    {
        /// <summary>
        /// Name of the problem.
        /// </summary>
        private string _name;

        /// <summary>
        /// Name of the problem.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Problem description.
        /// </summary>
        private string _comment;


        /// <summary>
        /// Problem description.
        /// </summary>
        public string Comment
        {
            get { return _comment; }
        }

        /// <summary>
        /// Number of nodes in the problem.
        /// </summary>
        private int _dimension;

        /// <summary>
        /// Number of nodes in the problem.
        /// </summary>
        public int Dimension
        {
            get { return _dimension; }
        }

        /// <summary>
        /// Edge weight type from file.
        /// </summary>
        private string _edgeWeightType;

        /// <summary>
        /// Edge weight type from file.
        /// </summary>
        public string EdgeWeightType
        {
            get { return _edgeWeightType; }
        }

        /// <summary>
        /// Format edge weights are given in.
        /// </summary>
        private string _edgeWeightFormat;

        /// <summary>
        /// Format edge weights are given in.
        /// </summary>
        public string EdgeWeightFormat
        {
            get { return _edgeWeightFormat; }
        }

        /// <summary>
        /// Matrix of edge weights between nodes.
        /// </summary>
        private double[][] _edgeLengths;

        /// <summary>
        /// Matrix of edge weights between nodes.
        /// </summary>
        public double[][] EdgeLengths
        {
            get { return _edgeLengths; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Name of the Problem.</param>
        /// <param name="comment">Comment from file.</param>
        /// <param name="dimension">Number of nodes in the problem.</param>
        /// <param name="edgeWeightType">Edge weight type from file.</param>
        /// <param name="edgeWeightFormat">Format edge weights are given in.
        /// </param>
        /// <param name="edgeWeights">Edge weight matrix.</param>
        public Problem(string name, string comment, int dimension, 
            string edgeWeightType, string edgeWeightFormat, 
            double[][] edgeWeights)
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