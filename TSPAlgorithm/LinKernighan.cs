/*
 * Author: Ewan Robertson
 * Implementation of Lin-Kernighan Algorithm for solving benchmark Travelling
 * Salesman Problems (TSP).
 */

namespace TSPAlgorithm
{
    /// <summary>
    /// Lin-Kernighan
    /// </summary>
    internal class LinKernighan : TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Current tour.
        /// </summary>
        private int[] tour;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the algorithm.</param>
        /// <param name="problem">Problem algorithm is to be run on.</param>
        public LinKernighan (string name, Problem problem) : 
            base (name, problem)
        {
            tour = new int[0];
        }

        /// <summary>
        /// Fitness Function.
        /// </summary>
        /// <returns>Distance around current tour.</returns>
        private double GetFitness()
        {
            double sum = 0;

            for (int i = 0; i < Problem.Dimension; i++)
            {
                int a = tour[i];
                int b = tour[(i + 1) % Problem.Dimension];
                sum += Problem.EdgeLengths[a][b];
            }

            return sum;
        }

        /// <summary>
        /// Returns the distance between two nodes in the tour.
        /// </summary>
        /// <param name="x">Index of the first node.</param>
        /// <param name="y">Index of the second node.</param>
        /// <returns>Distance between two input indices.</returns>
        private double GetDistance(int x, int y)
        {
            return Problem.EdgeLengths[tour[x % Problem.Dimension]]
                [tour[y % Problem.Dimension]];
        }

        /// <summary>
        /// Get the index of a value in the tour.
        /// </summary>
        /// <param name="value">Search value.</param>
        /// <returns>Index of value in tour.</returns>
        private int GetIndex(int value)
        {
            int i = 0;
            foreach (int index in tour)
            {
                if (value == index)
                {
                    return index;
                }
                i++;
            }
            return -1;
        }

        /// <summary>
        /// Creates a random, valid tour for the problem using the drunken
        /// sailor algorithm.
        /// </summary>
        /// <returns>Random, valid tour.</returns>
        public int[] CreateRandomTour()
        {
            int[] array = new int[Problem.Dimension];
            for (int i = 0; i < Problem.Dimension; i++)
            {
                array[i] = i;
            }
            
            for (int i = 0; i < Problem.Dimension; ++i)
            {
                int index = Parameters.random.Next(i + 1);
                int a = array[index];
                array[index] = array[i];
                array[i] = a;
            }
            
            return array;
        }

        /// <summary>
        /// Executes the algorithm on the given problem.
        /// </summary>
        /// <returns>Run results.</returns>
        public override Result Run()
        {
            // init new tour
            tour = CreateRandomTour();

            // init distances
            double oldDistance = 0;
            double newDistance = GetFitness();

            // main loop
            do
            {
                oldDistance = newDistance;
                ImproveAll();
                newDistance = GetFitness();
                Evaluations++;
            }
            while (newDistance < oldDistance && Evaluations < Parameters.EvaluationBudget);

            // set Best
            Best = new Permutation(Problem);
            foreach (int node in tour)
            {
                Best.Add(node);
            }

            Console.WriteLine($"Best fitness found: {GetFitness()}");
            return Result();
        }

        /// <summary>
        /// Improves the tour.
        /// </summary>
        private void ImproveAll()
        {
            for (int i = 0; i < Problem.Dimension; ++i)
            {
                Improve(i);
            }
        }

        /// <summary>
        /// Improves the tour, starting from a particular node.
        /// </summary>
        /// <param name="t1">Node to start with.</param>
        private void Improve(int t1)
        {
            Improve(t1, false);
        }

        /// <summary>
        /// Improves the tour, starting from a particular node.
        /// </summary>
        /// <param name="t1">Node to start with.</param>
        /// <param name="previous">Flad, is there a previous node in the tour.
        /// </param>
        private void Improve(int t1, bool previous)
        {
            int t2 = previous ? GetPreviousIdx(t1) : GetNextIdx(t1);
            int t3 = GetNearestNeighbour(t2);

            // implements gain criteria
            if (t3 != -1 && GetDistance(t2, t1) > GetDistance(t3, t2))
            {
                StartAlgorithm(t1, t2, t3);
            } 
            else if (!previous)
            {
                Improve(t1, true);
            }
        }

        /// <summary>
        /// Returns the previous index of the tour, typically x-1 unless x=0
        /// then returns last node.
        /// </summary>
        /// <param name="index">Current node.</param>
        /// <returns>Previous node.</returns>
        private int GetPreviousIdx(int index)
        {
            return index == 0 ? Problem.Dimension - 1: index - 1;
        }

        /// <summary>
        /// Returns the next index of the tour, typically x-1 unless x=n then
        /// returns first node.
        /// </summary>
        /// <param name="index">Current node.</param>
        /// <returns>Next node.</returns>
        private int GetNextIdx(int index)
        {
            return (index + 1) % Problem.Dimension;
        }

        /// <summary>
        /// Returns the nearest neighbour by edge weight.
        /// </summary>
        /// <param name="index">Current node.</param>
        /// <returns>Index of nearest neighbour</returns>
        private int GetNearestNeighbour(int index)
        {
            double minDistance = double.MaxValue;
            int nearestNode = -1;
            int actualNode = tour[index];

            for (int i = 0; i < Problem.Dimension; ++i)
            {
                if (i != actualNode)
                {
                    double distance = GetDistance(i, actualNode);
                    if (distance < minDistance)
                    {
                        nearestNode = GetIndex(i);
                        minDistance = distance;
                    }
                }
            }
            return nearestNode;
        }

        /// <summary>
        /// Step 4. from Lin-Kernighan paper.
        /// </summary>
        /// <param name="t1">Index of t1 in the tour.</param>
        /// <param name="t2">Index of t2 in the tour.</param>
        /// <param name="t3">Index of t3 in the tour.</param>
        private void StartAlgorithm(int t1, int t2, int t3)
        {
            List<int> tIndex = new List<int>();

            tIndex.Insert(0, -1);   // start with index 1 to
                                    // be consistent with paper
            tIndex.Insert(1, t1);
            tIndex.Insert(2, t2);
            tIndex.Insert(3, t3);

            double initialGain = GetDistance(t2, t1) - GetDistance(t3, t2);
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
                Gi += GetDistance(tIndex.ElementAt(tIndex.Count - 2), newT);
                if (Gi - GetDistance(newT, t1) > GStar)
                {
                    GStar = Gi - GetDistance(newT, t1);
                    k = i;
                }
                
                tIndex.Add(tiplus1);
                Gi -= GetDistance(newT, tiplus1);
            }

            if (GStar > 0)
            {
                tIndex.Insert(k + 1, tIndex.ElementAt(1));
                tour = GetTPrime(tIndex, k);
            }
        }

        /// <summary>
        /// Returns closest y that meets criteria for step 4.
        /// </summary>
        /// <param name="tIndex">List of all t's.</param>
        /// <returns>Closest possible y.</returns>
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
                if (GetDistance(ti, i) < minDistance)
                {
                    minNode = i;
                    minDistance = GetDistance(ti, i);
                }
            }

            return minNode;
        }

        /// <summary>
        /// Part e. of point 4.
        /// </summary>
        /// <param name="tIndex"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private bool NextXPossible(List<int> tIndex, int i)
        {
            return IsConnected(tIndex, i, GetNextIdx(i)) || 
                IsConnected(tIndex, i, GetPreviousIdx(i));
        }

        /// <summary>
        /// Determines whether the current tour has an edge linking x and y.
        /// </summary>
        /// <param name="tIndex">All t's.</param>
        /// <param name="x">Node 1.</param>
        /// <param name="y">Node 2.</param>
        /// <returns></returns>
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
                else if (tIndex.ElementAt(i) == y && tIndex.ElementAt(i + 1) 
                    == x)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines whether ditance gain would be positive.
        /// </summary>
        /// <param name="tIndex">All t's.</param>
        /// <param name="ti"></param>
        /// <returns>True if gain would be positive.</returns>
        private bool IsPositiveGain(List<int> tIndex, int ti)
        {
            double gain = 0;
            for (int i = 1; i < tIndex.Count - 2; ++i)
            {
                int t1 = tIndex.ElementAt(i);
                int t2 = tIndex.ElementAt(i + 1);
                int t3 = i == tIndex.Count - 3 ? ti : tIndex.ElementAt(i + 2);

                gain += GetDistance(t2, t3) - GetDistance(t1, t2);
            }

            return gain > 0;
        }

        /// <summary>
        /// Gets new t with characteristics from step 4.a
        /// </summary>
        /// <param name="tIndex">All t's.</param>
        /// <returns>New t.</returns>
        private int SelectNewT(List<int> tIndex)
        {
            int option1 = GetPreviousIdx(tIndex.ElementAt(tIndex.Count - 1));
            int option2 = GetNextIdx(tIndex.ElementAt(tIndex.Count - 1));

            int[] tour1 = ConstructNewTour(tour, tIndex, option1);

            if (IsTour(tour1))
            {
                return option1;
            }
            else
            {
                int[] tour2 = ConstructNewTour(tour, tIndex, option2);
                if (IsTour(tour2))
                {
                    return option2;
                }
            }
            return -1;
        }

        /// <summary>
        /// Constructs new tour.
        /// </summary>
        /// <param name="tour2">New tour.</param>
        /// <param name="tIndex">All t's.</param>
        /// <param name="newItem">New item to add to t's.</param>
        /// <returns>New tour from changes.</returns>
        private int[] ConstructNewTour(int[] tour2, List<int> tIndex, 
            int newItem)
        {
            List<int> changes = new List<int>(tIndex);

            changes.Add(newItem);
            changes.Add(changes.ElementAt(1));

            return ConstructNewTour(tour2, changes);
        }

        /// <summary>
        /// Constructs new tour.
        /// </summary>
        /// <param name="tour">Tour.</param>
        /// <param name="changes">Changes.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Get list of x's from changes.
        /// </summary>
        /// <param name="changes">The changes proposed for the tour.</param>
        /// <returns>List of edges to be removed.</returns>
        private List<Edge> DeriveX(List<int> changes)
        {
            List<Edge> es = new List<Edge>();
            for (int i = 1; i < changes.Count - 2; i += 2)
            {
                Edge e = new Edge(
                    tour[changes.ElementAt(i)], 
                    tour[changes.ElementAt(i + 1)]);
                es.Add(e);
            }           
            return es;
        }

        /// <summary>
        /// Get list of y's from changes.
        /// </summary>
        /// <param name="changes">The changes proposed for the tour.</param>
        /// <returns>List of edges to be added.</returns>
        private List<Edge> DeriveY(List<int> changes)
        {
            List<Edge> es = new List<Edge>();
            for (int i = 2; i < changes.Count - 1; i += 2)
            {
                Edge e = new Edge(
                    tour[changes.ElementAt(i)],
                    tour[changes.ElementAt(i + 1)]);
                es.Add(e);
            }
            return es;
        }

        /// <summary>
        /// Converts tour to edge list.
        /// </summary>
        /// <param name="tour">Current tour.</param>
        /// <returns>List of edges in tour.</returns>
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

        /// <summary>
        /// Converts list of edges into tour.
        /// </summary>
        /// <param name="currentEdges">List of edges.</param>
        /// <param name="s">Problem size.</param>
        /// <returns>New tour from edges.</returns>
        private int[] CreateTourFromEdges (List<Edge> currentEdges, int s)
        {
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

        /// <summary>
        /// Constructs T prime.
        /// </summary>
        /// <param name="tIndex">Indices of t's.</param>
        /// <param name="k">Index.</param>
        /// <returns></returns>
        private int[] GetTPrime(List<int> tIndex, int k)
        {
            List<int> al2 = new List<int>(tIndex.GetRange(0, k + 2));
            return ConstructNewTour(tour, al2);
        }

        /// <summary>
        /// Determines whether a tour is a valid permutation.
        /// </summary>
        /// <param name="tour">Tour.</param>
        /// <returns>True if tour is valid permutation.</returns>
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

        /// <summary>
        /// Determines whether edge is on X or Y.
        /// </summary>
        /// <param name="tIndex">Indices of nodes in tour.</param>
        /// <param name="x">Node 1.</param>
        /// <param name="y">Node 2.</param>
        /// <returns>True if disjunctive.</returns>
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
    }
}
