﻿/*
 * Author: Ewan Robertson
 * Result for single execution of an algorithm on a
 * benchmark travelling salseman problem.
 */
namespace TSPAlgorithm
{
    internal class Result
    {
        /// <summary>
        /// Name of the Travelling Salesman Problem.
        /// </summary>
        private string _problemName;
        /// <summary>
        /// Name of the algorithm used.
        /// </summary>
        private string _algorithmName;
        /// <summary>
        /// Fitness (tour distance) of the best solution found.
        /// </summary>
        private double _bestFitness;
        public double BestFitness
        {
            get { return _bestFitness; }
        }
        /// <summary>
        /// Path (node order) of the best solution found.
        /// </summary>
        private string _bestPath;
        public string BestPath
        {
            get { return _bestPath; }
        }

        /// <summary>
        /// Number of evaluations allowed in the execution.
        /// </summary>
        private int _evaluationBudget;

        /// <summary>
        /// Constructor for initialising Result instances.
        /// </summary>
        /// <param name="problemName">Name of the problem.</param>
        /// <param name="algorithmName">Name of the algorithm used.</param>
        /// <param name="bestFitness">Fitness (tour distance) of the best solution found.</param>
        /// <param name="bestPath">Best path found.</param>
        /// <param name="evaluationBudget">Number of evaluations allowed in the execution.</param>
        public Result(string problemName, string algorithmName, double bestFitness, string bestPath, int evaluationBudget)
        {
            _problemName = problemName;
            _algorithmName = algorithmName;
            _bestFitness = bestFitness;
            _bestPath = bestPath;
            _evaluationBudget = evaluationBudget;
        }

        /// <summary>
        /// Returns string representation of the Result object.
        /// </summary>
        /// <returns>String representation of result.</returns>
        public override string ToString()
        {
            return $"{_problemName},{_algorithmName},{_bestFitness},\"{_bestPath}\",{_evaluationBudget}";
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