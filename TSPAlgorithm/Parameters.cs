/*
 * Author: Ewan Robertson
 * Universal parameters, i.e. parameters that do not change regardless of
 * the algorithm being run.
 */

namespace TSPAlgorithm
{
    internal static class Parameters
    {
        /// <summary>
        /// File path to work space.
        /// </summary>
        public const string FilePath = "C:\\Users\\nuker\\Documents\\Honours Project\\Problems\\";

        /// <summary>
        /// TSPLIB file name.
        /// </summary>
        public const string ProblemName = "berlin52.tsp";

        /// <summary>
        /// The number of times the algorithm will be executed on the problem instance.
        /// </summary>
        public const int NumberOfRuns = 1;

        /// <summary>
        /// Maximum number of evaluations (fitness function calls) allowed.
        /// </summary>
        public const int EvaluationBudget = 50000;

        /// <summary>
        /// Instance of Random class.
        /// </summary>
        public static Random random = new Random();
    }
}
