namespace skullOS.API.Data_Objects
{
    public class Capture
    {
        private string filename;
        private string time;

        public Capture(string Name, string time)
        {
            filename = Name;
            this.time = time;
        }

        public Capture(string Name, DateTime time)
        {
            filename = Name;
            this.time = time.ToShortTimeString();
        }

        public string Time
        {
            get { return time; }
            set { time = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

    }
}
