using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TSPAlgorithm
{
    internal class NearestNeighbour : TravellingSalesmanAlgorithm
    {
        private string _name = "NearestNeighbour";
        private int _evalBudget = 0;
        public NearestNeighbour()
        {
        }

        public Result Run(Problem problem)
        {
            List<int> nodes = new List<int>();
            Random rand = new Random();
            nodes.Add(rand.Next(0, problem.Nodes.Length));
            // order nodes by distance from nodes.head, pick closest node that is not in nodes
            double[][] edgeLengths = problem.EdgeLengths;

            for (int i=0; i<problem.Nodes.Length-1; i++)
            {
                int nearestNeighbourIndex = 0;
                for (int j=0; j<problem.Nodes.Length; j++)
                {
                    if (!nodes.Contains(j) && edgeLengths[nodes.Last()][j] < edgeLengths[nodes.Last()][nearestNeighbourIndex])
                    {
                        nearestNeighbourIndex = j;
                    }
                }
                nodes.Add(nearestNeighbourIndex);
            }
            return null;
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
