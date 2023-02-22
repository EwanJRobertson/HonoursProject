/*
 * Author: Ewan Robertson
 * A file reader for reading text files.
 * Singleton Design Pattern
 */

using System.Collections.Generic;
using System.IO;

namespace TSPAlgorithm
{
    internal class FileIO
    {
        /// <summary>
        /// Contains a reference to the single instance of the FileReader class.
        /// </summary>
        private static FileIO? _instance;

        /// <summary>
        /// Gets a reference to the instance, creates a new instance if none exists.
        /// </summary>
        /// <returns>
        /// A reference to the FileReader.
        /// </returns>
        public static FileIO Instance
        {
            get
            {
                if (_instance == null) _instance = new FileIO();
                return _instance;
            }
        }

        /// <summary>
        /// Reads in text from file.
        /// </summary>
        /// <param name="filepath">Filepath from the root directory.</param>
        /// <returns>Array containing each line from the file provided.</returns>
        public string[] Read(string filepath)
        {
            return File.ReadAllLines(filepath);
        }

        /// <summary>
        /// Writes text to file.
        /// </summary>
        /// <param name="filepath">Filepath from the root directory.</param>
        /// <param name="lines">Lines to be written to the file.</param>
        public void Write(string filepath, string[] lines)
        {
            File.WriteAllLines(filepath, lines);
        }
    }
}
