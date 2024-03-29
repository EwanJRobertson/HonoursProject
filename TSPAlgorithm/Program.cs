﻿using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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

            // Run Algorithm
            //RunNN();
            //RunEA();
            //RunACO();
            //RunSA();
            //RunLK();

            // Parameter Tuning
            //EAPopulationSizeTuning();
            //EACrossoverRateTuning();
            //EAMutationRateTuning();

            //ACOPopulationSizeTuning();
            //ACOMutationRateTuning();
            //ACOEvaporationFactorTuning();
            //ACOBetaTuning();
            //ACOCTuning();
            //ACOQTuning();

            //SAInitialTemperatureTuning();
            //SACoolingRateTuning();

            //EvaluationBudgetExperiment();

            //Experiment1();
            //Experiment2();
            //Experiment3();
            //Experiment4();
            //Experiment5();

            // END
            Console.WriteLine("Exiting");
        }

        /// <summary>
        /// Execute the Nearest Neighbour algorithm.
        /// </summary>
        public static void RunNN()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");
            NearestNeighbour algorithm = new NearestNeighbour("ACO", problem);
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                results.Add(algorithm.Run());
            }
            FileIO.Write(Parameters.FilePath + "NN" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// Execute the Evolutionary Algorithm.
        /// </summary>
        public static void RunEA()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");
            EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("EA", problem);
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                results.Add(algorithm.Run());
            }
            FileIO.Write(Parameters.FilePathOutput + "EATournamentSelection" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// Execute the Ant Colony Optimisation algorithm.
        /// </summary>
        public static void RunACO()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");
            AntColonyOptimisation algorithm = new AntColonyOptimisation("ACO", problem);
            for (int i = 0; i < 1; i++)
            {
                results.Add(algorithm.Run());
            }
            //FileIO.Write(Parameters.FilePathOutput + "ACO" + ".csv",
            //    Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// Execute the Simulated Annealing algorithm.
        /// </summary>
        public static void RunSA()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");
            SimulatedAnnealing algorithm = new SimulatedAnnealing("ACO", problem);
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                results.Add(algorithm.Run());
            }
            FileIO.Write(Parameters.FilePath + "SA" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// Execute the Lin-Kernighan Heuristic algorithm.
        /// </summary>
        public static void RunLK()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");
            LinKernighan algorithm = new LinKernighan("LK", problem);
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                results.Add(algorithm.Run());
            }
            FileIO.Write(Parameters.FilePath + "LK" + ".csv",
            Result.ToOutput(results.ToArray()));
        }


        /// <summary>
        /// EA population size tuning experiment.
        /// </summary>
        public static void EAPopulationSizeTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // population size tuning
            foreach (int popSize in new int[] { 25, 50, 75, 100, 125 })
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
        }

        /// <summary>
        /// EA crossover rate tuning experiment.
        /// </summary>
        public static void EACrossoverRateTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // crossover rate tuning
            foreach (double cxRate in new double[] { 0.6, 0.7, 0.8, 0.9, 1.0 })
            {
                Console.WriteLine(cxRate);
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("cxRate" + cxRate, problem);
                algorithm.CrossoverRate = cxRate;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "EACrossoverRateTuning" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// EA mutation rate experiment.
        /// </summary>
        public static void EAMutationRateTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // mutation rate tuning
            foreach (double mutRate in new double[] { 0.6, 0.7, 0.8, 0.9, 1.0 })
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

        /// <summary>
        /// ACO population size tuning experiment.
        /// </summary>
        public static void ACOPopulationSizeTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // population size tuning
            foreach (int popSize in new int[] { 25, 50, 75, 100, 125 })
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
        }

        /// <summary>
        /// ACO mutation rate tuning experiment.
        /// </summary>
        public static void ACOMutationRateTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // mutation rate tuning
            foreach (double mutRate in new double[] { 0.01, 0.05, 0.10, 0.15, 0.20 })
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("mutRate" + mutRate, problem);
                algorithm.MutationRate = mutRate;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "ACOMutationRate" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// ACO evaporation factor tuning experiment.
        /// </summary>
        public static void ACOEvaporationFactorTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // evaporation factor tuning
            foreach (double evapFactor in new double[] { 0.1, 0.25, 0.5, 0.75, 0.9 })
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
        }

        /// <summary>
        /// ACO beta tuning experiment.
        /// </summary>
        public static void ACOBetaTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // beta tuning
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
        }

        /// <summary>
        /// ACO C tuning experiment.
        /// </summary>
        public static void ACOCTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // c tuning
            foreach (double c in new double[] { 0.5, 1.0, 2.0, 5.0, 10.0 })
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("c" + c, problem);
                algorithm.C = c;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "ACOC" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// ACO Q tuning experiment.
        /// </summary>
        public static void ACOQTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // q tuning
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

        /// <summary>
        /// SA starting temperature tuning experiment.
        /// </summary>
        public static void SAInitialTemperatureTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // q tuning
            foreach (double t in new double[] { 500, 1000, 1500, 2000, 2500 })
            {
                SimulatedAnnealing algorithm = new SimulatedAnnealing("initTemp" + t, problem);
                algorithm.InitialTemperature = t;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "SATemp" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// SA cooling rate tuning experiment.
        /// </summary>
        public static void SACoolingRateTuning()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("berlin52");

            // q tuning
            foreach (double propto in new double[] { 0.05, 0.10, 0.15, 0.20, 0.25 })
            {
                SimulatedAnnealing algorithm = new SimulatedAnnealing("coolingRate" + propto, problem);
                algorithm.CoolingRate = propto;
                for (int i = 0; i < Parameters.NumberOfRuns; i++)
                {
                    results.Add(algorithm.Run());
                }
            }
            FileIO.Write(Parameters.FilePath + "SACoolingRate" + ".csv",
                Result.ToOutput(results.ToArray()));
        }


        /// <summary>
        /// Evaluation budget experiment.
        /// </summary>
        public static void EvaluationBudgetExperiment()
        {
            Problem problem = FileIO.ParseTSPLIB("bier127");
            List<Result> results = new List<Result>();

            // NN
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                NearestNeighbour algorithm = new NearestNeighbour("NNEval", problem);
                results.Add(algorithm.Run());
            }

            // EA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("EAEval", problem);
                results.Add(algorithm.Run());
            }

            // ACO
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("ACOEval", problem);
                results.Add(algorithm.Run());
            }

            // SA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                SimulatedAnnealing algorithm = new SimulatedAnnealing("SAEval", problem);
                results.Add(algorithm.Run());
            }

            // LK
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                LinKernighan algorithm = new LinKernighan("LKEval", problem);
                results.Add(algorithm.Run());
            }

            FileIO.Write(Parameters.FilePath + "EvalExperiment" + ".csv",
                Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// berlin52 experiment.
        /// </summary>
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

            // LK
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                LinKernighan algorithm = new LinKernighan("LK", problem);
                results.Add(algorithm.Run());
            }

            FileIO.Write(Parameters.FilePathOutput + "experiment1.csv", Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// eil101 experiment.
        /// </summary>
        public static void Experiment2()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("eil101");

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

            // LK
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                LinKernighan algorithm = new LinKernighan("LK", problem);
                results.Add(algorithm.Run());
            }

            FileIO.Write(Parameters.FilePathOutput + "experiment2.csv", Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// pr264 experiment.
        /// </summary>
        public static void Experiment3()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("u574");

            // NN
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                NearestNeighbour algorithm = new NearestNeighbour("NN", problem);
                results.Add(algorithm.Run());
                FileIO.Write(Parameters.FilePathOutput + $"experiment3NN+{i}.csv", Result.ToOutput(results.ToArray()));
            }

            // EA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                EvolutionaryAlgorithm algorithm = new EvolutionaryAlgorithm("EA", problem);
                results.Add(algorithm.Run());
                FileIO.Write(Parameters.FilePathOutput + $"experiment3EA+{i}.csv", Result.ToOutput(results.ToArray()));
            }

            // ACO
            for (int i = 0; i < 0; i++)
            {
                AntColonyOptimisation algorithm = new AntColonyOptimisation("ACO", problem);
                results.Add(algorithm.Run());
                FileIO.Write(Parameters.FilePathOutput + $"experiment3ACO+{i}.csv", Result.ToOutput(results.ToArray()));
            }

            // SA
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                SimulatedAnnealing algorithm = new SimulatedAnnealing("SA", problem);
                results.Add(algorithm.Run());
                FileIO.Write(Parameters.FilePathOutput + $"experiment3SA+{i}.csv", Result.ToOutput(results.ToArray()));
            }

            // LK
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                LinKernighan algorithm = new LinKernighan("LK", problem);
                results.Add(algorithm.Run());
                FileIO.Write(Parameters.FilePathOutput + $"experiment3LK+{i}.csv", Result.ToOutput(results.ToArray()));
            }

            FileIO.Write(Parameters.FilePathOutput + "u574 experiment3.csv", Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// gr431 experiment.
        /// </summary>
        public static void Experiment4()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("pr264");

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

            // LK
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                LinKernighan algorithm = new LinKernighan("LK", problem);
                results.Add(algorithm.Run());
            }

            FileIO.Write(Parameters.FilePathOutput + "experiment4.csv", Result.ToOutput(results.ToArray()));
        }

        /// <summary>
        /// u574 experiment.
        /// </summary>
        public static void Experiment5()
        {
            List<Result> results = new List<Result>();
            Problem problem = FileIO.ParseTSPLIB("gr431");

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
            for (int i = 0; i < 0; i++)
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

            // LK
            for (int i = 0; i < Parameters.NumberOfRuns; i++)
            {
                LinKernighan algorithm = new LinKernighan("LK", problem);
                results.Add(algorithm.Run());
            }

            FileIO.Write(Parameters.FilePathOutput + "gr431-experiment5.csv", Result.ToOutput(results.ToArray()));
        }
    }
}