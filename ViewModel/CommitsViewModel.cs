using System.ComponentModel;
using System.Timers;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Reflection;

namespace TeamDashboard
{
    public class CommitsViewModel : INotifyPropertyChanged
    {
        string filePath = @"content\Commits.json";

        public CommitInfo CommitInfo1 { get; set; }
        public CommitInfo CommitInfo2 { get; set; }
        public CommitInfo CommitInfo3 { get; set; }

        public CommitsViewModel()
        {
            Initialize();

            var timer = new Timer();
            timer.Interval = 60 * 1000; // 1 minute update
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        void Initialize()
        {
            CommitInfo1 = new CommitInfo();
            CommitInfo2 = new CommitInfo();
            CommitInfo3 = new CommitInfo();

            if (!File.Exists(filePath))
                CreateDefaultFile();

            Update();
        }

        void Update()
        {
            ReadFromFile();

            NotifyPropertyChanged("CommitInfo1");
            NotifyPropertyChanged("CommitInfo2");
            NotifyPropertyChanged("CommitInfo3");
        }

        void CreateDefaultFile()
        {
            CommitInfo1.UserImage = Resources.DefaultUserImage;
            var imageConverter = new ImageConverter();

            JArray jArray = new JArray();
            jArray.Add(JsonConvert.SerializeObject(CommitInfo1, imageConverter));
            jArray.Add(JsonConvert.SerializeObject(CommitInfo2, imageConverter));
            jArray.Add(JsonConvert.SerializeObject(CommitInfo3, imageConverter));
            
            JObject jObject = new JObject();
            jObject["Commits"] = jArray;

            string jsonString = jObject.ToString();

            PathHelper.WriteFile(filePath, jsonString);
        }

        private void ReadFromFile()
        {
            var jsonString = File.ReadAllText(filePath);
            var jObject = JObject.Parse(jsonString);

            var imageConverter = new ImageConverter();

            var jCommits = jObject["Commits"].Values<string>().ToList();
            CommitInfo1 = JsonConvert.DeserializeObject<CommitInfo>(jCommits[0], imageConverter);
            CommitInfo2 = JsonConvert.DeserializeObject<CommitInfo>(jCommits[1], imageConverter);
            CommitInfo3 = JsonConvert.DeserializeObject<CommitInfo>(jCommits[2], imageConverter);

            if (CommitInfo1.UserImage == null)
                CommitInfo1.UserImage = Resources.DefaultUserImage;

            if (CommitInfo2.UserImage == null)
                CommitInfo2.UserImage = Resources.DefaultUserImage;

            if (CommitInfo3.UserImage == null)
                CommitInfo3.UserImage = Resources.DefaultUserImage;
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Update();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
