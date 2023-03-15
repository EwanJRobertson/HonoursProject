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
    }
}
