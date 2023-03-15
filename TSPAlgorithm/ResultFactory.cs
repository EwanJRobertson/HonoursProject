﻿/*
 * Author: Ewan Robertson
 * A result factory for generating result objects.
 * Singleton Design Pattern
 * Factory Design Pattern
 */

namespace TSPAlgorithm
{
    internal static class ResultFactory
    {
        /// <summary>
        /// Factory method for creating Result objects containing the details of a single 
        /// execution of an algorithm on a benchmark travelling salesman problem.
        /// </summary>
        /// <param name="problemName">Name of the problem.</param>
        /// <param name="algorithmName">Name of the algorithm used.</param>
        /// <param name="bestFitness">Fitness (tour distance) of the best solution found.</param>
        /// <param name="bestPath">Number of evaluations allowed in the execution.</param>
        /// <param name="evaluationBudget">Number of evaluations allowed in the execution.</param>
        /// <returns>A problem object containing the details of a benchmark travelling salesman problem.</returns>
        public static Result FactoryMethod(string problemName, string algorithmName, double bestFitness, 
            string bestPath, int evaluationBudget)
        {
            return new Result(problemName, algorithmName, bestFitness, bestPath, evaluationBudget);
        }
    }
}
