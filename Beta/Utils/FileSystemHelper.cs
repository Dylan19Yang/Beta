using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Beta.Model;
using Beta.Settings;

namespace Beta.Utils
{
    public static class FileSystemHelper
    {
        [DllImport("shell32.dll")]
        static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder, bool fCreate);
        const int CSIDL_COMMON_PROGRAMS = 0x17;

        #region Utils

        public static void ReadAppPaths(string path, List<Program> list)
        {
            using (var root = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path))
            {
                if (root == null) return;
                foreach (var item in root.GetSubKeyNames())
                {
                    using (var key = root.OpenSubKey(item))
                    {
                        object filepath = key.GetValue("");
                        if (filepath is string && global::System.IO.File.Exists((string)filepath))
                        {
                            Program entry = new Program((string)filepath);
                            entry.ExecuteName = item;
                            list.Add(entry);
                        }

                        key.Close();
                    }
                }
            }
        }

        public static void GetAppFromDirectory(string path, List<Program> list)
        {
            try
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (UserSetting.Suffixes.Split(';').Any(o => file.EndsWith("." + o)))
                    {
                        Program p = new Program(file);
                        list.Add(p);
                    }
                }

                foreach (var subDirectory in Directory.GetDirectories(path))
                {
                    GetAppFromDirectory(subDirectory, list);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(string.Format("Can't access to directory {0}", path));
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(string.Format("Directory {0} doesn't exist", path));
            }
            catch (PathTooLongException e)
            {
                Console.WriteLine(string.Format("File path too long: {0}", e.Message));
            }
        }

        public static string GetCommonStartMenuPath()
        {
            StringBuilder commonStartMenuPath = new StringBuilder(560);
            SHGetSpecialFolderPath(IntPtr.Zero, commonStartMenuPath, CSIDL_COMMON_PROGRAMS, false);

            return commonStartMenuPath.ToString();
        }

        #endregion
    }
}
