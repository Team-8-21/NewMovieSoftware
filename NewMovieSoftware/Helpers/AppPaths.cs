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
        public static string EnsureAppFile(string fileName, string seedName, string defaultContent = "[]")
        {
            var appDir = GetAppDirectory();
            Directory.CreateDirectory(appDir);

            var target = Path.Combine(appDir, fileName);
            if (File.Exists(target)) return target;

            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var seed = Path.Combine(baseDir, "Data", seedName);

            try
            {
                if (File.Exists(seed))
                    File.Copy(seed, target, overwrite: false);
                else
                    File.WriteAllText(target, defaultContent);
            }
            catch
            {
                // Allersidste fallback
                if (!File.Exists(target))
                    File.WriteAllText(target, defaultContent);
            }

            return target;
        }
    }

}
