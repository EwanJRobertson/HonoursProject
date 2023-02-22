/*
 * Author: Ewan Robertson
 * A problem factory for generating travelling saleseman problem objects.
 * Singleton Design Pattern
 * Factory Design Pattern
 */

namespace TSPAlgorithm
{
    internal class ProblemFactory
    {
        /// <summary>
        /// Contains a reference to the single instance of the ProblemFactory class.
        /// </summary>
        private static ProblemFactory? _instance;

        /// <summary>
        /// Gets a reference to the instance, creates a new instance if none exists.
        /// </summary>
        /// <returns>
        /// A reference to the ProblemFactory.
        /// </returns>
        public static ProblemFactory Instance
        {
            get
            {
                if (_instance == null) _instance = new ProblemFactory();
                return _instance;
            }
        }

        /// <summary>
        /// Factory method for creating Problem objects containing the details of a benchmark
        /// travelling salesman problem.
        /// </summary>
        /// <param name="name">Name of the Problem.</param>
        /// <param name="comment">Comment from file.</param>
        /// <param name="dimension">Number of nodes in the problem.</param>
        /// <param name="edgeWeightType">Edge weight type from file.</param>
        /// <param name="nodes">Node coordinates.</param>
        /// <returns>A problem object containing the details of a benchmark travelling salesman problem.</returns>
        public static Problem FactoryMethod(string name, string comment, int dimension, string edgeWeightType, (int, int)[] nodes)
        {
            return new Problem(name, comment, dimension, edgeWeightType, nodes);
        }
    }
}
