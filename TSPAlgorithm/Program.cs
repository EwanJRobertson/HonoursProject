using System;
using System.Xml.Serialization;

namespace TSPAlgorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // setup
            List<Problem> problems = new List<Problem>();

            Console.WriteLine("Honours Project: TITLE");
            Console.WriteLine("Author: Ewan Robertson 40451077");
            Console.WriteLine("Framework for the comparison of algorithms for solving " +
                "benchmark Travelling Salesman Problems.\n");
            // Console.WriteLine($"Output writing to {_outputFile}.");

            // problem file
            string[] input = FileIO.Read(Parameters.FilePath + Parameters.ProblemName);
            Problem problem = Problem.ParseTSP(input);

            Result[] results = new Result[Parameters.NumberOfRuns];

            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                // NearestNeighbour algorithm = new NearestNeighbour("nn", problem);
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("ea", problem);
                // AntColonyOptimisation algorithm = new AntColonyOptimisation("aco", problem);
                // SimulatedAnnealing algorithm = new SimulatedAnnealing("sa", problem);
                results[i] = algorithm.Run();
            }

            FileIO.Write(Parameters.FilePath + "result.csv", Result.ToOutput(results));

            // END
            Console.WriteLine("Exiting");
        }
    }
}