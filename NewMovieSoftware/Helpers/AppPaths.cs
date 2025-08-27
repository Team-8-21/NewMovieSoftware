using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieOrganiser2000.Helpers
{
    public static class AppPaths
    {
        // Brug samme rod som du bruger til at oprette filer (LocalApplicationData eller ApplicationData)
        public static string GetAppDirectory()
        {
            var root = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(root, "MovieOrganiser2000");
        }
    }
}
