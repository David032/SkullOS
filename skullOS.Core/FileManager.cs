using System.Runtime.InteropServices;

namespace skullOS.Core
{
    public static class FileManager
    {
        private static string rootDirectoryPath = string.Empty;

        public static void CreateSkullDirectory(bool usePersonalDir = true)
        {
            DirectoryInfo? rootDirectory = null;
            string pathToDir;
            if (usePersonalDir)
            {
                pathToDir = @Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            }
            else
            {
                pathToDir = "/media";
            }
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    rootDirectory = Directory.CreateDirectory(pathToDir + @"/skullOS",
                                    unixCreateMode: UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                                                    UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute |
                                                    UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (rootDirectory == null)
            {
                throw new Exception("Root directory not defined!");
            }
            rootDirectoryPath = rootDirectory.FullName;
        }

        public static string GetSkullDirectory()
        {
            return rootDirectoryPath;
        }

        public static void CreateSubDirectory(string directoryName)
        {
            if (rootDirectoryPath != string.Empty && RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Directory.CreateDirectory(rootDirectoryPath + @"/" + directoryName,
                    unixCreateMode: UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                                    UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute |
                                    UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute);
            }
        }
    }
}