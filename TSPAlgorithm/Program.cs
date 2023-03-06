using System.Xml.Serialization;

namespace TSPAlgorithm
{
    internal class Program
    {
        private static string _outputFile = "";
        static void Main(string[] args)
        {
            // setup
            FileIO fileIO = FileIO.Instance;
            ProblemFactory problemFactory = ProblemFactory.Instance;
            ResultFactory resultFactory = ResultFactory.Instance;
            List<Problem> problems = new List<Problem>();

            Console.WriteLine("Honours Project: TITLE");
            Console.WriteLine("Author: Ewan Robertson 40451077");
            Console.WriteLine("Framework for the comparison of algorithms for solving " +
                "benchmark Travelling Salesman Problems.\n");
            Console.WriteLine($"Output writing to {_outputFile}.");

            /*
            while (true)
            {
                int choice = 0;
                // get user choice
                if (choice == 0)
                    break;
                // execute algorithm
                // save results
            }
            */

            string[] input = fileIO.Read("C:\\Users\\nuker\\Documents\\Honours Project\\TSPAlgorithm\\TSPAlgorithm\\Problems\\berlin52.tsp");
            string probName = input[0];
            string comment = input[1];
            int dimension = 52;
            string edgeWeightType = input[3];
            (int, int)[] nodes = new (int, int)[input.Length - 5];
            string[] a;
            for (int i = 6; i < 58; i++)
            {
                a = input[i].Split(' ');
                nodes[i - 4] = ((int)double.Parse(a[1]), (int)double.Parse(a[2]));
            }
            Problem p = ProblemFactory.FactoryMethod(probName, comment, dimension, edgeWeightType, nodes);

            // NearestNeighbour n = new NearestNeighbour();
            ACO n = new ACO();
            Console.WriteLine(n.Run(p).BestFitness);

            // END
            Console.WriteLine("Exiting");
        }
    }
}