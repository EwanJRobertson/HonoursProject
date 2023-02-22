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

            double[][] edgeLengths = new double[3][];
            edgeLengths[0] = new double[3] { 0.0, 1.0, 2.0 };
            edgeLengths[1] = new double[3] { 1.0, 0.0, 5.0 };
            edgeLengths[2] = new double[3] { 2.0, 5.0, 0.0 };
            Comparer<double> comparer = Comparer<double>.Default;
            Array.Sort<double[]>(edgeLengths, (x,y) => comparer.Compare(x[1], y[1]));
            for(int i=0; i<3; i++)
                Console.WriteLine(edgeLengths[i][0]);

            // END
            Console.WriteLine("Exiting");
        }
    }
}