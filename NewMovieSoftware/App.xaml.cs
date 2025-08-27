using MovieOrganiser2000.Helpers;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace MovieOrganiser2000
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                var appDir = AppPaths.GetAppDirectory();

                if (Directory.Exists(appDir))
                {
                    // Sørg for at alle filer er lukket inden vi sletter
                    Directory.Delete(appDir, recursive: true);
                    Debug.WriteLine($"[Cleanup] Deleted entire folder: {appDir}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Cleanup] Failed to delete app data folder: {ex.Message}");
            }

            base.OnExit(e);
        }
    }

}
