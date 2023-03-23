using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPAlgorithm
{
    internal class Edge : Comparer<Edge>
    {
        public int _firstNode;

        public int FirstNode
        {
            get { return _firstNode; }
        }

        public int _secondNode;

        public int SecondNode
        {
            get { return _secondNode; }
        }

        public Edge(int a, int b)
        {
            _firstNode = a > b ? a : b;
            _secondNode = a > b ? b : a;
        }

        public int CompareTo(Edge e2)
        {
            if (this.FirstNode < e2.FirstNode || this.FirstNode == e2.FirstNode && this.SecondNode < e2.SecondNode)
            {
                return -1;
            }
            else if (this.Equals(e2))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        public bool Equals(Edge e2)
        {
            if (e2 == null!)
            {
                return false;
            }
            return (this.FirstNode == e2.FirstNode) && (this.SecondNode == e2.SecondNode);
        }

        public override int Compare(Edge? x, Edge? y)
        {
            return x!.CompareTo(y!);
        }

        public override string ToString()
        {
            return FirstNode + " " + SecondNode;
        }
    }
}
