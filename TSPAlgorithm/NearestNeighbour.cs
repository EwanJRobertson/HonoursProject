/*
 * Author: Ewan Robertson
 * Nearest Neighnour algorithm for solving Travelling Salesman Problems.
 */

namespace TSPAlgorithm
{
    /// <summary>
    /// Nearest Neighbour (NN) algorithm for TSP.
    /// </summary>
    internal class NearestNeighbour : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Algorithm name.</param>
        /// <param name="problem">Problem algorithm to be run on.</param>
        public NearestNeighbour(string name, Problem problem) : 
            base (name, problem)
        { 
        }

        /// <summary>
        /// Executes the algorithm on the given problem.
        /// </summary>
        /// <returns>Run results.</returns>
        public override Result Run()
        {
            // init permutation
            Best = new Permutation(Problem);

            // add first node arbitrary
            Best.Add(Parameters.random.Next(0, Problem.Dimension));

            // order nodes by distance from nodes.head, pick closest node that
            // is not in nodes
            while (Best.Length != Problem.Dimension)
            {
                // get first index not in permutation
                int nearestNeighbourIndex = 0;
                while (Best.Contains(nearestNeighbourIndex))
                {
                    nearestNeighbourIndex++;
                }

                // get index of nearest neighbour
                for (int j = 0; j < Problem.Dimension; j++)
                {
                    if (!Best.Contains(j) && 
                        Problem.EdgeWeights[Best.Last][j] < 
                        Problem.EdgeWeights[Best.Last][nearestNeighbourIndex])
                    {
                        nearestNeighbourIndex = j;
                    }
                }
                
                // add next node to permutation
                Best.Add(nearestNeighbourIndex);
            }

            // increment evaluations
            Evaluations++;

            // return result
            return Result();
        }
    }
}