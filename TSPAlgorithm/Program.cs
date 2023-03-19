using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace TSPAlgorithm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Honours Project: TITLE");
            Console.WriteLine("Author: Ewan Robertson 40451077");
            Console.WriteLine("Framework for the comparison of algorithms for solving " +
                "benchmark Travelling Salesman Problems.\n");
            // Console.WriteLine($"Output writing to {_outputFile}.");

            /*
            // problem file
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            Result[] results = new Result[Parameters.NumberOfRuns];

            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                NearestNeighbour algorithm = new NearestNeighbour("nn", problem);
                // EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("ea", problem);
                // AntColonyOptimisation algorithm = new AntColonyOptimisation("aco", problem);
                // SimulatedAnnealing algorithm = new SimulatedAnnealing("sa", problem);
                // results[i] = algorithm.Run();
            }
            */

            // EAParameterTuning();
            // ACOParameterTuning();
            // EvaluationBudgetExperiment();
            Problem problem = FileIO.ParseTSPLIB("berlin52");
            SimulatedAnnealing algorithm = new SimulatedAnnealing("ea", problem);
            Console.WriteLine(algorithm.Run().BestFitness); 

            // END
            Console.WriteLine("Exiting");
        }

        public static void EAParameterTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // population size tuning
            foreach (int popSize in new int[] { 20, 40, 60, 80, 100 })
            {
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("pop" + popSize, problem);
                algorithm.PopulationSize = popSize;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "EAPopulationSizeTuning" + ".csv",
                Result.ToOutput(results.ToArray()));

            // crossover rate tuning
            results.Clear();
            foreach (int cxRate in new double[] { 0.6, 0.7, 0.8, 0.9, 1.0 })
            {
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("cxRate" + cxRate, problem);
                algorithm.CrossoverRate = cxRate;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "EACrossoverRateTuning" + ".csv",
                Result.ToOutput(results.ToArray()));

            // mutation rate tuning
            results.Clear();
            foreach (int mutRate in new double[] { 0.1, 0.25, 0.5, 0.75, 1.0 })
            {
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("mutRate" + mutRate, problem);
                algorithm.MutationRate = mutRate;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "EAMutationRateTuning" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        public static void ACOParameterTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // population size tuning
            foreach (int popSize in new int[] { 50, 75, 100, 125, 150 })
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("pop" + popSize, problem);
                algorithm.PopulationSize = popSize;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "ACOPopulationSizeTuning" + ".csv",
                Result.ToOutput(results.ToArray()));

            // evaporation factor tuning
            results.Clear();
            foreach (int evapFactor in new double[] { 0.1, 0.333, 0.5, 0.666, 0.9 })
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("evapFactor" + evapFactor, problem);
                algorithm.EvaporationFactor = evapFactor;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "ACOEvapFactorTuning" + ".csv",
                Result.ToOutput(results.ToArray()));

            // beta tuning
            results.Clear();
            foreach (int beta in new double[] { 2, 4, 6, 8, 10 })
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("beta" + beta, problem);
                algorithm.Beta = beta;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "ACOBeta" + ".csv",
                Result.ToOutput(results.ToArray()));

            // q tuning
            results.Clear();
            foreach (int q in new double[] { 100, 200, 300, 400, 500 })
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("q" + q, problem);
                algorithm.Q = q;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "ACOQ" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        public static void EvaluationBudgetExperiment()
        {
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // EA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("EAEval", problem);
                algorithm.Run();
            }

            // ACO
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("ACOEval", problem);
                algorithm.Run();
            }

            // SA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                SimulatedAnnealing algorithm = new SimulatedAnnealing("SAEval", problem);
                algorithm.Run();
            }
        }

        public static void Experiment1()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // NN
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                NearestNeighbour algorithm = new NearestNeighbour("NN", problem);
                results.Add(algorithm.Run());
            }

            // EA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("EA", problem);
                results.Add(algorithm.Run());
            }

            // ACO
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("ACO", problem);
                results.Add(algorithm.Run());
            }

            // SA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                SimulatedAnnealing algorithm = new SimulatedAnnealing("SA", problem);
                results.Add(algorithm.Run());
            }

            FileIO.Write(Parameters.FilePath + "experiment1.csv", Result.ToOutput(results.ToArray()));
        }
    }
}