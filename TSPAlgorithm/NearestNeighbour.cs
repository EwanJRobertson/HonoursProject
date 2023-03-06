/*
 * Author: Ewan Robertson
 * Nearest Neighnour algorithm for solving Travelling Salesman Problems.
 */

namespace TSPAlgorithm
{
    internal class NearestNeighbour : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Algorithm name.
        /// </summary>
        private string _name = "NearestNeighbour";

        public NearestNeighbour()
        {  }

        public Result Run(Problem problem)
        {
            Permutation permutation = new Permutation(problem);
            Random rand = new Random();

            // first node random
            permutation.Add(rand.Next(0, problem.Dimension));
            // order nodes by distance from nodes.head, pick closest node that is not in nodes

            for (int i = 0; i < problem.Dimension - 1; i++)
            {
                
                int nearestNeighbourIndex = 0;
                while (permutation.Contains(nearestNeighbourIndex))
                {
                    nearestNeighbourIndex++;
                }

                for (int j = 0; j < problem.Dimension; j++)
                {
                    if (!permutation.Contains(j) && 
                        problem.EdgeLengths[permutation.Last][j] < problem.EdgeLengths[permutation.Last][nearestNeighbourIndex])
                    {
                        nearestNeighbourIndex = j;
                    }
                }
                permutation.Add(nearestNeighbourIndex);
            }
            
            return ResultFactory.FactoryMethod(problem.Name, _name, permutation.Fitness, permutation.Path(), 1);
        }
    }
}


/*
                Comparer<double> comparer = Comparer<double>.Default;
                Array.Sort<double[]>(edgeLengths, (x,y) => comparer.Compare(x[column], y[column]));
                int currentNode = nodes.Last();
                Array.Sort<double[]>(edgeLengths, (x, y) => comparer.Compare(x[currentNode-1], y[currentNode-1]));
                int count = 0;
                edgeLengths.
                while (nodes.Contains(edgeLengths[currentNode - 1][count]))
                {

                }
                */

// find min in column not already used
// edge[current][i] min
