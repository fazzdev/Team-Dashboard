using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamDashboard
{
    public static class PathHelper
    {
        public static void WriteFile(string filePath, string content)
        {
            var directoryPath = Path.GetDirectoryName(filePath);
            Directory.CreateDirectory(directoryPath);

            File.WriteAllText(filePath, content);
        }
    }
}
