using System.Runtime.InteropServices;

namespace skullOS.Core
{
    public static class FileManager
    {
        private static string rootDirectoryPath = string.Empty;

        public static void CreateSkullDirectory(bool usePersonalDir = true, bool isTest = false)
        {
            if (!isTest)
            {
                DirectoryInfo? rootDirectory = null;
                string pathToDir;
                if (usePersonalDir)
                {
                    //pathToDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    pathToDir = Environment.GetEnvironmentVariable("HOME");
                    Console.WriteLine("Path to personal dir is " + pathToDir);
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
            else
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    DirectoryInfo rootDirectory = Directory.CreateDirectory("/skullOS-TestData", unixCreateMode:
                                UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute |
                                UnixFileMode.GroupRead | UnixFileMode.GroupWrite | UnixFileMode.GroupExecute |
                                UnixFileMode.OtherRead | UnixFileMode.OtherWrite | UnixFileMode.OtherExecute);
                    rootDirectoryPath = rootDirectory.FullName;
                }

            }

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

        public static void DeleteDirectory()
        {
            Directory.Delete(rootDirectoryPath, true);
        }
    }
}