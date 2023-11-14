namespace skullOS.Core
{
    public static class FileManager
    {
        private static string rootDirectoryPath = string.Empty;

        public static void CreateSkullDirectory()
        {
            DirectoryInfo rootDirectory = null;
            string pathToPersonalDir = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            try
            {
                rootDirectory = Directory.CreateDirectory(@pathToPersonalDir + "/skullOS", unixCreateMode: UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            rootDirectoryPath = rootDirectory.FullName;
        }

        public static string GetSkullDirectory()
        {
            return rootDirectoryPath;
        }
    }
}