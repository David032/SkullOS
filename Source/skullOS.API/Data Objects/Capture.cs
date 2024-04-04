namespace skullOS.API.Data_Objects
{
    public class Capture
    {
        private string filename;
        private string? date;
        private string time;

        public Capture(string Name, string time, string? date = null)
        {
            filename = Name;
            this.date = date;
            this.time = time;
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

        public string Date
        {
            get
            {
                if (date != null)
                {
                    return date;
                }
                else
                {
                    return "No date";
                }
            }
            set { date = value; }
        }
    }
}
