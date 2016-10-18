using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMessageBoard.Helpers
{
    class FileDirectoryHelpers
    {
    /// <summary>
        /// return array of file type by filter search
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string[] GetFileNames(string path, string filter)
        {
            path = path.Replace("file:///", "");
            string[] files = System.IO.Directory.GetFiles(path, filter);
            for (int i = 0; i < files.Length; i++)
                files[i] = System.IO.Path.GetFileName(files[i]);
            return files;
        }

        /// <summary>
        /// Return Array of subdirectories
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string[] GetSubdirectories(string url)
        {
            url = url.Replace("file:///", "");
            System.IO.DirectoryInfo dInfo = new System.IO.DirectoryInfo(url);
            System.IO.DirectoryInfo[] subdirs = dInfo.GetDirectories();

            string[] subdirectories = new string[3];
            for (int i = 0; i < subdirs.Length; i++)
            {
                subdirectories[i] = subdirs[i].Name;
            }

            return subdirectories;
        }
    }
}
