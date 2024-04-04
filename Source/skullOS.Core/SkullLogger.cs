using skullOS.Core.Interfaces;

namespace skullOS.Core
{
    public class SkullLogger : ISkullLogger
    {
        string filepath;
        public SkullLogger()
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
            FileManager.CreateSubDirectory("logs");
            filepath = FileManager.GetSkullDirectory() + "/logs/" + fileName;
        }

        public void LogMessage(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
            File.AppendAllText(filepath, DateTime.Now.ToShortTimeString() + ":" + message + "\n");
        }
    }
}
