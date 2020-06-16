using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab0613
{
    static class SearchClass
    {
        /// <param name="baseDir">directory where search do</param>
        /// <param name="pattern">file compirason pattern</param>
        /// <param name="recursively">search in sub dirs</param>
        /// <returns>list of relative paths to files in base dir</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static List<string> FindFiles(string baseDir, Regex pattern, bool recursively = false)
        {
            return FindFiles(baseDir, "", pattern, recursively);
        }

        /// <param name="baseDir">directory where search do</param>
        /// <param name="relativeDir">sub path in baseDir</param>
        /// <param name="pattern">file compirason pattern</param>
        /// <param name="recursively">search in sub dirs</param>
        /// <returns>list of relative paths to files in baseDir</returns>
        private static List<string> FindFiles(string baseDir, string relativeDir, Regex pattern, bool recursively = false)
        {
            List<string> result = new List<string>();
            var dirInfo = new DirectoryInfo(Path.Combine(baseDir, relativeDir));
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                if (pattern.IsMatch(fileInfo.Name))
                    result.Add(Path.Combine(relativeDir, fileInfo.Name));
            }

            if (recursively)
            {
                foreach (var subDirInfo in dirInfo.GetDirectories())
                    result.AddRange(FindFiles(baseDir, Path.Combine(relativeDir, subDirInfo.Name), pattern, true));
            }

            return result;
        }

    }
}
