/*
 * Author: Ewan Robertson
 * Nearest Neighnour algorithm for solving Travelling Salesman Problems.
 */

namespace TSPAlgorithm
{
    internal class NearestNeighbour : TravellingSalesmanAlgorithm
    {
        public NearestNeighbour(string name, Problem problem) : base (name, problem)
        { }

        public override Result Run()
        {
            Best = new Permutation(Problem);
            // first node random
            Best.Add(Parameters.random.Next(0, Problem.Dimension));
            // order nodes by distance from nodes.head, pick closest node that is not in nodes

            for (int i = 0; i < Problem.Dimension - 1; i++)
            {
                int nearestNeighbourIndex = 0;
                while (Best.Contains(nearestNeighbourIndex))
                {
                    nearestNeighbourIndex++;
                }

                for (int j = 0; j < Problem.Dimension; j++)
                {
                    if (!Best.Contains(j) && 
                        Problem.EdgeLengths[Best.Last][j] < Problem.EdgeLengths[Best.Last][nearestNeighbourIndex])
                    {
                        nearestNeighbourIndex = j;
                    }
                }
                Best.Add(nearestNeighbourIndex);
            }

            Evaluations++;
            return Result();
        }
    }
}