/*
 * Author: Ewan Robertson
 * Result for single execution of an algorithm on a
 * benchmark travelling salseman problem.
 */
namespace TSPAlgorithm
{
    /// <summary>
    /// Result for a single execution of an algorithm on a TSP problem.
    /// </summary>
    internal class Result
    {
        /// <summary>
        /// Name of the problem instance.
        /// </summary>
        private string _problemName;

        /// <summary>
        /// Name of the problem instance.
        /// </summary>
        public string ProblemName
        {
            get { return _problemName; }
        }

        /// <summary>
        /// Name of the algorithm used.
        /// </summary>
        private string _algorithmName;

        /// <summary>
        /// Name of the algorithm used.
        /// </summary>
        public string AlgorithmName
        {
            get { return _algorithmName; }
        }

        /// <summary>
        /// Fitness (tour distance) of the best solution found.
        /// </summary>
        private double _bestFitness;

        /// <summary>
        /// Fitness (tour distance) of the best solution found.
        /// </summary>
        public double BestFitness
        {
            get { return _bestFitness; }
        }

        /// <summary>
        /// Path (node order) of the best solution found.
        /// </summary>
        private string _bestPath;

        /// <summary>
        /// Path (node order) of the best solution found.
        /// </summary>
        public string BestPath
        {
            get { return _bestPath; }
        }

        /// <summary>
        /// Number of evaluations allowed in the execution.
        /// </summary>
        private int _evaluationBudget;

        /// <summary>
        /// Number of evaluations allowed in the execution.
        /// </summary>
        public int EvaluationBudget
        {
            get { return _evaluationBudget; }
        }

        /// <summary>
        /// Number of evaluations until the best result is found.
        /// </summary>
        private int _evalsForBest;

        /// <summary>
        /// Number of evaluations until the best result is found.
        /// </summary>
        public int EvalsForBest
        {
            get { return _evalsForBest; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="problemName">Name of the problem.</param>
        /// <param name="algorithmName">Name of the algorithm used.</param>
        /// <param name="bestFitness">Fitness (tour distance) of the best
        /// solution found.</param>
        /// <param name="bestPath">Best path found.</param>
        /// <param name="evaluationBudget">Number of evaluations allowed in the
        /// execution.</param>
        /// <param name="evalsForBest">Number of evaluations taken to fund the
        /// best solution.</param>
        public Result(string problemName, string algorithmName, 
            double bestFitness, string bestPath, int evaluationBudget,
            int evalsForBest)
        {
            _problemName = problemName;
            _algorithmName = algorithmName;
            _bestFitness = bestFitness;
            _bestPath = bestPath;
            _evaluationBudget = evaluationBudget;
            _evalsForBest = evalsForBest;
        }

        /// <summary>
        /// Returns string representation of the Result object.
        /// </summary>
        /// <returns>String representation of result.</returns>
        public override string ToString()
        {
            return $"{_problemName},{_algorithmName},{_bestFitness}," +
                $"\"{_bestPath}\",{_evaluationBudget},{_evalsForBest}";
        }

        /// <summary>
        /// Converts results to string for wriing to csv.
        /// </summary>
        /// <param name="results">Array of experiment results.</param>
        /// <returns>String array of experiment results.</returns>
        public static string[] ToOutput(Result[] results)
        {
            string[] outputStr = new string[results.Length];
            for (int i = 0; i < results.Length; i++)
            {
                outputStr[i] = results[i].ToString();
            }
            return outputStr;
        }
    }
}
