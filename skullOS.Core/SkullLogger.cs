namespace skullOS.Core
{
    public class SkullLogger
    {
        string filepath;
        public SkullLogger()
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss");
            filepath = FileManager.GetSkullDirectory() + @"/" + fileName + ".txt";
        }


        public void LogMessage(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
            File.AppendAllText(filepath + "\n", message);
        }
    }
}
