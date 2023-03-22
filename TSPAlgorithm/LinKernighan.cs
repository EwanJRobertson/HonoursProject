using System.Dynamic;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;

namespace TSPAlgorithm
{
    internal class LinKernighan : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Tour.
        /// </summary>
        private Permutation tour;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the algorithm.</param>
        /// <param name="problem">Problem algorithm is to be run on.</param>
        public LinKernighan (string name, Problem problem) : 
            base (name, problem)
        {
            tour = new Permutation(problem);
        }

        /// <summary>
        /// Creates a random, valid tour for the problem.
        /// </summary>
        /// <returns>Valid tour.</returns>
        public Permutation CreateRandomTour()
        {
            Permutation tour = new Permutation(Problem);
            while (tour.Length < Problem.Dimension)
            {
                int nextNode = Parameters.random.Next(Problem.Dimension);
                while (tour.Contains(nextNode))
                {
                    nextNode = Parameters.random.Next(Problem.Dimension);
                }
                tour.Add(nextNode);
            }
            return tour;
        }

        private void Improve()
        {
            for (int i = 0; i < Problem.Dimension; ++i)
            {
                Improve(i);
            }
        }

        private void Improve(int x)
        {
            Improve(x, false);
        }

        private void Improve(int t1, bool previous)
        {
            int t2 = previous ? GetPreviousIdx(t1) : GetNextIdx(t1);
            int t3 = GetNearestNeighbour(t2);

            if (t3 != -1 && Problem.EdgeLengths[t2][t3] < Problem.EdgeLengths[t1][t2])
            {
                StartAlgorithm(t1, t2, t3);
            } 
            else if (!previous)
            {
                Improve(t1, true);
            }
        }

        private int GetPreviousIdx(int index)
        {
            return index == 0 ? Problem.Dimension - 1: index - 1;
        }

        private int GetNextIdx(int index)
        {
            return (index + 1) % Problem.Dimension;
        }

        private int GetNearestNeighbour(int index)
        {
            double minDistance = double.MaxValue;
            int nearestNode = -1;
            int actualNode = tour.GetNode(index);

            for (int i = 0; i < Problem.Dimension; ++i)
            {
                if (i != actualNode)
                {
                    double distance = Problem.EdgeLengths[i][actualNode];
                    if (distance < minDistance)
                    {
                        nearestNode = tour.GetIndex(i);
                        minDistance = distance;
                    }
                }
            }
            return nearestNode;
        }

        private void StartAlgorithm(int t1, int t2, int t3)
        {
            List<int> tIndex = new List<int>();

            tIndex.Insert(0, -1);
            tIndex.Insert(1, t1);
            tIndex.Insert(2, t2);
            tIndex.Insert(3, t3);

            double initialGain = Problem.EdgeLengths[t2][t1] - Problem.EdgeLengths[t3][t2];
            double GStar = 0;
            double Gi = initialGain;
            int k = 3;

            for (int i = 4; ; i+=2)
            {
                int newT = SelectNewT(tIndex);
                // fail safe
                if (newT == -1)
                {
                    break;
                }

                tIndex.Insert(i, newT);
                int tiplus1 = GetNextPossibleY(tIndex);
                if (tiplus1 == -1)
                {
                    break;
                }

                // step 4.f
                Gi += Problem.EdgeLengths[tIndex.ElementAt(tIndex.Count - 2)][newT];
                if (Gi - Problem.EdgeLengths[newT][t1] > GStar)
                {
                    GStar = Gi - Problem.EdgeLengths[newT][t1];
                    k = i;
                }

                tIndex.Add(tiplus1);
                Gi -= Problem.EdgeLengths[newT][tiplus1];
            }

            if (GStar > 0)
            {
                tIndex.Insert(k + 1, tIndex.ElementAt(1));
                int[] nodes = GetTPrime(tIndex, k);
                Permutation newTour = new Permutation(Problem);
                foreach (int node in nodes)
                {
                    newTour.Add(node);
                }
                tour = newTour.Clone();
            }
        }

        private int SelectNewT(List<int> tIndex)
        {
            int option1 = GetPreviousIdx(tIndex.ElementAt(tIndex.Count - 1));
            int option2 = GetNextIdx(tIndex.ElementAt(tIndex.Count - 1));

            int[] tour1 = ConstructNewTour(tour.GetAllNodes().ToArray(), tIndex, option2);

            if (IsTour(tour1))
            {
                return option1;
            }
            else
            {
                int[] tour2 = ConstructNewTour(tour.GetAllNodes().ToArray(), tIndex, option2);
                if (IsTour(tour2))
                {
                    return option2;
                }
            }
            return -1;
        }

        private int[] ConstructNewTour(int[] tour2, List<int> tIndex, int newItem)
        {
            List<int> changes = new List<int>(tIndex);

            changes.Add(newItem);
            changes.Add(changes.ElementAt(1));

            return ConstructNewTour(tour2, changes);
        }

        private int[] ConstructNewTour(int[] tour, List<int> changes)
        {
            List<Edge> currentEdges = DeriveEdgesFromTour(tour);

            List<Edge> x = DeriveX(changes);
            List<Edge> y = DeriveY(changes);
            int s = currentEdges.Count;

            // remove xs
            foreach (Edge e in x)
            {
                for (int i = 0; i < currentEdges.Count; ++i)
                {
                    Edge m = currentEdges.ElementAt(i);
                    if (e.Equals(m))
                    {
                        s--;
                        currentEdges[i] = null!;
                        break;
                    }    
                }
            }

            // add ys
            foreach (Edge e in y)
            {
                s++;
                currentEdges.Add(e);
            }
            

            return CreateTourFromEdges(currentEdges, s);
        }

        private List<Edge> DeriveX(List<int> changes)
        {
            List<Edge> es = new List<Edge>();
            for (int i = 1; i < changes.Count - 2; i += 2)
            {
                Edge e = new Edge(
                    tour.GetAllNodes().ElementAt(changes.ElementAt(i)), 
                    tour.GetAllNodes().ElementAt(changes.ElementAt(i + 1)));
                es.Add(e);
            }           
            return es;
        }

        private List<Edge> DeriveY(List<int> changes)
        {
            List<Edge> es = new List<Edge>();
            for (int i = 2; i < changes.Count - 1; i += 2)
            {
                Edge e = new Edge(
                    tour.GetAllNodes().ElementAt(changes.ElementAt(i)),
                    tour.GetAllNodes().ElementAt(changes.ElementAt(i + 1)));
                es.Add(e);
            }
            return es;
        }

        private List<Edge> DeriveEdgesFromTour(int[] tour)
        {
            List<Edge> es = new List<Edge>();
            for (int i = 0; i < tour.Length; ++i)
            {
                Edge e = new Edge(tour[i], tour[(i + 1) % tour.Length]);
                es.Add(e);
            }

            return es;
        }

        private int[] CreateTourFromEdges (List<Edge> currentEdges, int s)
        {
            
            Console.WriteLine("\n\n\n");
            int c = 0;
            foreach (Edge e in currentEdges)
            {
                Console.WriteLine(c + "  " + e);
                c++;
            }
            

            int[] tour = new int[s];

            int i = 0;
            int last = -1;

            for (; i < currentEdges.Count; ++i)
            {
                if (currentEdges.ElementAt(i) != null!)
                {
                    tour[0] = currentEdges.ElementAt(i).FirstNode;
                    tour[1] = currentEdges.ElementAt(i).SecondNode;
                    last = tour[1];
                    break;
                }
            }

            currentEdges[i] = null!;

            int k = 2;
            while (true)
            {
                int j = 0;
                for (; j < currentEdges.Count; ++j)
                {
                    Edge e = currentEdges.ElementAt(j);
                    if (e != null! && e.FirstNode == last)
                    {
                        last = e.SecondNode;
                        break;
                    }
                    else if (e != null! && e.SecondNode == last)
                    {
                        last = e.FirstNode;
                        break;
                    }
                }
                // if the list is empty
                if (j == currentEdges.Count)
                {
                    break;
                }

                // remove new edge
                currentEdges[j] = null!;
                if (k >= s)
                {
                    break;
                }
                tour[k] = last;
                k++;
            }

            return tour;
        }

        private bool IsTour(int[] tour)
        {
            if (tour.Length != Problem.Dimension)
            {
                return false;
            }

            for (int i = 0; i < Problem.Dimension - 1; ++i)
            {
                for (int j = i + 1; j < Problem.Dimension; ++j)
                {
                    if (tour[i] == tour[j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int GetNextPossibleY(List<int> tIndex)
        {
            int ti = tIndex.ElementAt(tIndex.Count - 1);

            List<int> ys = new List<int>();
            for (int i = 0; i < Problem.Dimension; ++i)
            {
                if (!IsDisjunctive(tIndex, i, ti))
                {
                    continue;
                }

                if (!IsPositiveGain(tIndex, i))
                {
                    continue;
                }
                if (!NextXPossible(tIndex, i))
                {
                    continue;
                }
                ys.Add(i);
            }

            // get closest y
            double minDistance = double.MaxValue;
            int minNode = -1;

            foreach (int i in ys)
            {
                if (Problem.EdgeLengths[ti][i] < minDistance)
                {
                    minNode = i;
                    minDistance = Problem.EdgeLengths[ti][i];
                }
            }

            return minNode;
        }

        private bool IsDisjunctive(List<int> tIndex, int x, int y)
        {
            if (x == y)
            {
                return false;
            }
            for (int i = 0; i < tIndex.Count - 1; i++)
            {
                if (tIndex.ElementAt(i) == x && tIndex.ElementAt(i + 1) == y)
                {
                    return false;
                }
                if (tIndex.ElementAt(i) == y && tIndex.ElementAt(i + 1) == x)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsPositiveGain(List<int> tIndex, int ti)
        {
            int gain = 0;
            for (int i = 1; i < tIndex.Count - 2; ++i)
            {
                int t1 = tIndex.ElementAt(i);
                int t2 = tIndex.ElementAt(i + 1);
                int t3 = i == tIndex.Count - 3 ? ti : tIndex.ElementAt(i + 2);

                gain += (int)Math.Round(Problem.EdgeLengths[t2][t3] - Problem.EdgeLengths[t1][t2]);
            }

            return gain > 0;
        }

        private bool NextXPossible(List<int> tIndex, int i)
        {
            return IsConnected(tIndex, i, GetNextIdx(i)) || IsConnected(tIndex, i, GetPreviousIdx(i));
        }

        private bool IsConnected(List<int> tIndex, int x, int y)
        {
            if (x == y)
            {
                return false;
            }

            for (int i = 1; i < tIndex.Count - 1; i += 2)
            {
                if (tIndex.ElementAt(i) == x && tIndex.ElementAt(i + 1) == y)
                {
                    return false;
                }
                else if (tIndex.ElementAt(i) == y && tIndex.ElementAt(i + 1) == x)
                {
                    return false;
                }
            }

            return true;
        }

        private int[] GetTPrime(List<int> tIndex, int k)
        {
            List<int> al2 = new List<int>(tIndex.GetRange(0, k + 2));
            return ConstructNewTour(tour.GetAllNodes().ToArray(), al2);
        }

        /// <summary>
        /// Executes the algorithm on the given problem.
        /// </summary>
        /// <returns>Run results.</returns>
        public override Result Run()
        {
            // init
            tour = CreateRandomTour();

            double oldDistance = 0;
            double newDistance = tour.Fitness;

            do
            {
                oldDistance = newDistance;
                Improve();
                newDistance = tour.Fitness;
            } 
            while (newDistance < oldDistance);

            Best = tour.Clone();
            Evaluations++;

            return Result();
        }
    }
}
