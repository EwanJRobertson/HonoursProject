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

            _edgeLengths = new double[dimension][];
            for (int i = 0; i < _nodes.Length; i++)
            {
                _edgeLengths[i] = new double[dimension];
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
        public override string ToString()
        {
            return $"{_name},{_comment},{_dimension},{_edgeWeightType}";
        }

        public static Problem ParseTSPLIB(string[] input)
        {
            string problemName = input[0];
            string comment = input[1];
            string[] temp = input[3].Split(' ');
            int dimension = int.Parse(temp[temp.Length - 1]);
            string edgeWeightType = input[4];
            (int, int)[] nodes = new (int, int)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                input[i + 6] = System.Text.RegularExpressions.Regex.Replace(input[i + 6], @"\s+", " ");
                temp = input[i + 6].Split(' ');
                nodes[i] = ((int)double.Parse(temp[1]), (int)double.Parse(temp[2]));
            }
            return ProblemFactory.FactoryMethod(problemName, comment, dimension, edgeWeightType, nodes);
        }

        public static Problem ParseTSP(string[] input)
        {
            string problemName = "";
            string comment = "";
            int dimension = -1;
            string edgeWeightType = "";
            (int, int)[] nodes = new (int, int)[0];

            int i = 0;
            while (input[i] != "EOF")
            {
                string[] split = input[i].Split(' ');
                switch (split[0])
                {
                    case "NAME":
                    case "NAME:":
                        problemName = split[split.Length - 1];
                        break;

                    case "COMMENT":
                    case "COMMENT:":
                        comment = split[split.Length - 1];
                        break;

                    case "DIMENSION":
                    case "DIMENSION:":
                        dimension = int.Parse(split[split.Length - 1]);
                        nodes = new (int, int)[dimension];
                        break;

                    case "EDGE_WEIGHT_TYPE":
                    case "EDGE_WEIGHT_TYPE:":
                        edgeWeightType = split[split.Length -1];
                        break;

                    case "NODE_COORD_SECTION":
                        for (int j = 0; j < dimension; j++)
                        {
                            input[j + i + 1] = System.Text.RegularExpressions.Regex.Replace(input[j + i + 1], @"\s+", " ");
                            string[] temp = input[j + i + 1].Split(' ');
                            if (temp[0] == "")
                            {
                                nodes[j] = ((int)double.Parse(temp[2]), (int)double.Parse(temp[3]));
                            }
                            else
                            {
                                nodes[j] = ((int)double.Parse(temp[1]), (int)double.Parse(temp[2]));
                            }
                        }
                        i += dimension;
                        break;

                    default:
                        break;
                }
                i++;
            }
            return ProblemFactory.FactoryMethod(problemName, comment, dimension, edgeWeightType, nodes);
        }
    }
}
