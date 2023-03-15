/*
 * Author: Ewan Robertson
 * Abstract Class for the implementation of Algorithms for solving
 * benchmark Travelling Salesman Problems.
 */

namespace TSPAlgorithm
{
    /// <summary>
    /// Abstract class for the implementation of algorithms for solving benchmark TSP 
    /// problems.
    /// </summary>
    internal abstract class TravellingSalesmanAlgorithm
    {
        /// <summary>
        /// Algorithm objects name.
        /// </summary>
        private string _name;

        /// <summary>
        /// Name parameter.
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Problem instance the algorithm is initialised to solve.
        /// </summary>
        private Problem _problem;

        /// <summary>
        /// Problem parameter.
        /// </summary>
        public Problem Problem
        {
            get { return _problem; }
        }

        private Permutation _best;

        protected Permutation Best
        {
            get { return _best; }
            set { _best = value; }
        }

        private int _evaluations;

        protected int Evaluations
        {
            get { return _evaluations; }
            set { _evaluations = value; }
        }


        /// <summary>
        /// Base constructor.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="problem">Problem instance the algorithm is to solve.</param>
        protected TravellingSalesmanAlgorithm (string name, Problem problem)
        {
            _name = name;
            _problem = problem;
            _best = new Permutation(Problem);
        }

        /// <summary>
        /// Runs the algorithm on the problem instance once.
        /// </summary>
        /// <returns>Result information about the run.</returns>
        public abstract Result Run();

        protected Result Result()
        {
            Console.WriteLine($"Best fitness found: {Best.Fitness}");
            return ResultFactory.FactoryMethod(Problem.Name, Name, _best.Fitness,
                _best.Path(), _evaluations);
        }
    }
}