/*
 * Author: Ewan Robertson
 * A file reader for reading text files.
 * Singleton Design Pattern
 */

namespace TSPAlgorithm
{
    internal static class FileIO
    {
        /// <summary>
        /// Reads in text from file.
        /// </summary>
        /// <param name="filepath">Filepath from the root directory.</param>
        /// <returns>Array containing each line from the file provided.</returns>
        public static string[] Read(string filepath)
        {
            return File.ReadAllLines(filepath);
        }

        /// <summary>
        /// Writes text to file.
        /// </summary>
        /// <param name="filepath">Filepath from the root directory.</param>
        /// <param name="lines">Lines to be written to the file.</param>
        public static void Write(string filepath, string[] lines)
        {
            File.WriteAllLines(filepath, lines);
        }

        public static Problem ParseTSPLIB()
        {
            string problemName = "";
            string comment = "";
            int dimension = -1;
            string edgeWeightType = "";
            string edgeWeightFormat = "";
            double[][] edgeWeights = new double[0][];

            string[] input = Read(Parameters.FilePath + Parameters.ProblemName);
            int count = 0;
            while (input[count].Trim() != "EOF")
            {
                input[count] = input[count].Trim();
                string[] split = input[count].Split(' ');
                switch (split[0])
                {
                    case "NAME":
                    case "NAME:":
                        problemName = split.Last();
                        break;

                    case "COMMENT":
                    case "COMMENT:":
                        comment = input[count].Substring(split[0].Length);
                        break;

                    case "DIMENSION":
                    case "DIMENSION:":
                        dimension = int.Parse(split.Last());
                        break;

                    case "EDGE_WEIGHT_TYPE":
                    case "EDGE_WEIGHT_TYPE:":
                        edgeWeightType = split.Last();
                        break;

                    case "EDGE_WEIGHT_FORMAT":
                    case "EDGE_WEIGHT_FORMAT:":
                        edgeWeightFormat = split.Last();
                        break;

                    case "NODE_COORD_SECTION":
                    case "EDGE_WEIGHT_SECTION":
                        switch (edgeWeightType)
                        {
                            case "EXPLICIT":
                                switch (edgeWeightFormat)
                                {
                                    case "FULL_MATRIX":
                                        edgeWeights = LowerDiagRow(input, count, dimension);
                                        break;

                                    case "UPPER_ROW":
                                        edgeWeights = UpperRow(input, count, dimension);
                                        break;

                                    case "LOWER_ROW":
                                        edgeWeights = LowerRow(input, count, dimension);
                                        break;

                                    case "UPPER_DIAG_ROW":
                                        edgeWeights = UpperDiagRow(input, count, dimension);
                                        break;

                                    case "LOWER_DIAG_ROW":
                                        edgeWeights = LowerDiagRow(input, count, dimension);
                                        break;

                                    case "UPPER_COL":
                                        edgeWeights = UpperCol(input, count, dimension);
                                        break;

                                    case "LOWER_COL":
                                        edgeWeights = LowerCol(input, count, dimension);
                                        break;

                                    case "UPPER_DIAG_COL":
                                        edgeWeights = UpperDiagCol(input, count, dimension);
                                        break;

                                    case "LOWER_DIAG_COL":
                                        edgeWeights = LowerDiagCol(input, count, dimension);
                                        break;
                                }
                                break;

                            case "EUC_2D":
                                edgeWeights = Euc2D(input, count, dimension);
                                break;

                            case "EUC_3D":
                                edgeWeights = Euc3D(input, count, dimension);
                                break;

                            case "MAX_2D":
                                edgeWeights = Max2D(input, count, dimension);
                                break;

                            case "MAX_3D":
                                edgeWeights = Max3D(input, count, dimension);
                                break;

                            case "MAN_2D":
                                edgeWeights = Man2D(input, count, dimension);
                                break;

                            case "MAN_3D":
                                edgeWeights = Man3D(input, count, dimension);
                                break;

                            case "CEIL_2D":
                                edgeWeights = Ceil2D(input, count, dimension);
                                break;

                            case "GEO":
                                edgeWeights = Geo(input, count, dimension);
                                break;

                            default:
                                break;
                        }
                        count += dimension;
                        break;

                    default:
                        break;
                }
                count++;
            }

            return ProblemFactory.FactoryMethod(problemName, comment, dimension, edgeWeightType, edgeWeightFormat, edgeWeights);
        }

        /// <summary>
        /// Parse TSPLIB euc2D input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Euc2D(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double)[] nodes = new (double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                nodes[i] = (double.Parse(line[1]), double.Parse(line[2]));
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    edgeWeights[i][j] = Math.Round(Math.Sqrt(Math.Pow(
                        nodes[j].Item1 - nodes[i].Item1, 2) + 
                        Math.Pow(nodes[j].Item2 - nodes[i].Item2, 2)));
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB euc3D input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Euc3D(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double, double)[] nodes = new (double, double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                nodes[i] = (double.Parse(line[1]), double.Parse(line[2]), 
                    double.Parse(line[3]));
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    edgeWeights[i][j] = Math.Round(Math.Sqrt(
                        Math.Pow(nodes[j].Item1 - nodes[i].Item1, 2) +
                        Math.Pow(nodes[j].Item2 - nodes[i].Item2, 2) +
                        Math.Pow(nodes[j].Item3 - nodes[i].Item3, 2)));
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB max2D input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Max2D(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double)[] nodes = new (double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                nodes[i] = (double.Parse(line[1]), double.Parse(line[2]));
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    edgeWeights[i][j] = Math.Max((nodes[j].Item1 - nodes[i].Item1),
                        (nodes[j].Item2 - nodes[i].Item2));
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB max3D input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Max3D(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double, double)[] nodes = new (double, double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                nodes[i] = (double.Parse(line[1]), double.Parse(line[2]),
                    double.Parse(line[3]));
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    edgeWeights[i][j] = Math.Max(Math.Max(
                        nodes[j].Item1 - nodes[i].Item1,
                        nodes[j].Item2 - nodes[i].Item2),
                        nodes[j].Item3 - nodes[i].Item3);
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB man2D input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Man2D(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double)[] nodes = new (double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                nodes[i] = (double.Parse(line[1]), double.Parse(line[2]));
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    edgeWeights[i][j] = (nodes[j].Item1 - nodes[i].Item1) + 
                        (nodes[j].Item2 - nodes[i].Item2);
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB man3D input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Man3D(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double, double)[] nodes = new (double, double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                nodes[i] = (double.Parse(line[1]), double.Parse(line[2]), 
                    double.Parse(line[3]));
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    edgeWeights[i][j] = (nodes[j].Item1 - nodes[i].Item1) +
                        (nodes[j].Item2 - nodes[i].Item2) +
                        (nodes[j].Item3 - nodes[i].Item3);
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB ceil2D input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Ceil2D(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double)[] nodes = new (double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                nodes[i] = (double.Parse(line[1]), double.Parse(line[2]));
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    edgeWeights[i][j] = Math.Ceiling(Math.Sqrt(Math.Pow(
                        nodes[j].Item1 - nodes[i].Item1, 2) + 
                        Math.Pow(nodes[j].Item2 - nodes[i].Item2, 2)));
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB geo input into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] Geo(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            (double, double)[] nodes = new (double, double)[dimension];
            for (int i = 0; i < dimension; i++)
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index + i], @"\s+", " ").Trim().Split(' ');
                double x = double.Parse(line[1]);
                double y = double.Parse(line[2]);
                nodes[i] = (Math.PI * 
                    (x - Math.Round(x) + 5.0 * x - Math.Round(x) / 3.0) / 180,
                    Math.PI * 
                    (y - Math.Round(y) + 5.0 * y - Math.Round(y) / 3.0) / 180);
            }

            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    double q1 = Math.Cos(nodes[j].Item2 - nodes[i].Item2);
                    double q2 = Math.Cos(nodes[j].Item1 - nodes[i].Item1);
                    double q3 = Math.Cos(nodes[j].Item1 + nodes[i].Item1);
                    edgeWeights[i][j] = 6378.388 * Math.Acos(
                        0.5 * ((1.0 + q1) * q2 - (1.0 - q1) * q3)) + 1.0;
                }
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in upper row form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] UpperRow(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = 0;
            int col = dimension - 1;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    if (row == col)
                    {
                        row++;
                        col = dimension - 1;
                    }

                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    col--;
                }
                index++;
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in lower row form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] LowerRow(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = 0;
            int col = 0;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    if (row == col)
                    {
                        row++;
                        col = 0;
                    }

                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    row++;
                }
                index++;
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in upper diagonal row form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] UpperDiagRow(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = 0;
            int col = dimension - 1;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    col--;
                    if (str == "0")
                    {
                        row++;
                        col = dimension - 1;
                    }
                }
                index++;
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in lower diagonal row form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] LowerDiagRow(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = 0;
            int col = 0;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    col++;
                    if (str == "0")
                    {
                        row++;
                        col = 0;
                    }
                }
                index++;
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in upper column form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] UpperCol(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = dimension - 1;
            int col = dimension - 1;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    if (row == col)
                    {
                        row = dimension - 1;
                        col--;
                    }

                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    row--;
                }
                index++;
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in lower column form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] LowerCol(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = dimension - 1;
            int col = 0;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    if (row == col)
                    {
                        row = dimension - 1;
                        col++;
                    }

                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    row--;
                }
                index++;
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in upper diagonal column form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] UpperDiagCol(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = dimension - 1;
            int col = dimension - 1;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    row--;
                    if (str == "0")
                    {
                        row = dimension - 1;
                        col--;
                    }
                }
                index++;
            }

            return edgeWeights;
        }

        /// <summary>
        /// Parse TSPLIB matrix input in lower diagonal column form into edge weight matrix.
        /// </summary>
        /// <param name="input">String stream.</param>
        /// <param name="index">Index at which coordinates start.</param>
        /// <param name="dimension">Problem dimension.</param>
        /// <returns>Edge weight matrix.</returns>
        public static double[][] LowerDiagCol(string[] input, int index, int dimension)
        {
            double[][] edgeWeights = new double[dimension][];
            for (int i = 0; i < dimension; i++)
            {
                edgeWeights[i] = new double[dimension];
            }

            int row = dimension - 1;
            int col = 0;

            while (input[index] != "DISPLAY_DATA_SECTION" || input[index] != "EOF")
            {
                string[] line = System.Text.RegularExpressions.Regex.Replace(
                    input[index], @"\s+", " ").Trim().Split(' ');

                foreach (string str in line)
                {
                    edgeWeights[row][col] = double.Parse(str);
                    edgeWeights[col][row] = edgeWeights[row][col];

                    row--;
                    if (str == "0")
                    {
                        row = dimension - 1;
                        col++;
                    }
                }
                index++;
            }

            return edgeWeights;
        }
    }
}
